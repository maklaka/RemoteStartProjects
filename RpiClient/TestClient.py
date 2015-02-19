# Echo client program
import socket
import RPi.GPIO as GPIO
import time
import os
import subprocess, signal

carSensePin = 11
radioPin = 15
modemPwrPin = 26

GPIO.setmode(GPIO.BOARD)
GPIO.setup(carSensePin, GPIO.IN)
GPIO.setup(radioPin, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(modemPwrPin, GPIO.OUT, initial=GPIO.HIGH) 

#start wvdial for first time and wait 10s for it to finish
print 'Starting wvdial for the first time...wait 10s'
diallog = open('dial_log')
derplog = open('derp_log')
proc = subprocess.Popen(['nohup', '/usr/bin/wvdial', '&']) 
modemTimer = time.time()
while time.time() - modemTimer < 16:
	derp = 0
	
	
HOST = '54.148.253.160'    # The remote host
PORT = 24235              # The same port as used by the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
try:
	s.connect((HOST, PORT))
except:
	os.system("sudo shutdown -r now")
	
s.setblocking(0)   #make non blocking after a connection is actually made
s.sendall('<EOF>')   #makes me a real client at dah soyvah

lastPingTime = time.time()
modemTimer = time.time()
latchStartTime = time.time()

carState = 'OFF'
pinState = 0
modemState = 0

data = ''

GPIO.output(radioPin, pinState)

startMsgToConsume = False


print 'Everything initialized....'

while 1:
    # assign to carState according to input pin  
	
	if GPIO.input(carSensePin) == True and carState == 'OFF':   #captures change of car from off to on once
			carState = 'ON'
			if modemState == 0:  #tell server the car is running right now if the modem is up by some slim chance
				s.sendall('ACK_Status CarState:' + carState + ' <EOF>')  #just turned on,  send status!
	elif GPIO.input(carSensePin) == False and carState == 'ON': #captures change of car from on to off once
		carState = 'OFF'
		modemTimer = time.time()   #give modem more time to recognize that the car turned off and send status to server
	
	
	if carState == 'ON' and modemState == 1:     #the car turned on, get the modem on too if it's off for some reason (will take 20 seconds)
		GPIO.output(modemPwrPin, GPIO.HIGH)
		modemTimer = time.time()
		modemState = 2
	
	#keep sending/receiving messages while modem is up and either the car is on or modem has been running less than 10 seconds
	if ((time.time() - modemTimer < 10 or carState == 'ON') and modemState == 0  ):
		if  (time.time() - lastPingTime) > 5:                        # this  will double as connection tester, send every 5 seconds with modem up
			lastPingTime = time.time()
			try:
				s.sendall('ACK_Status CarState:' + carState + ' <EOF>')
			except:
				os.system("sudo shutdown -r now")   #will restart modem or restart RPI because a transmission error'd out?  connection must be down?
				
			print 'Sent the ping' + '     ' + 'ACK_Status CarState:' + carState + ' <EOF>'
		
		if (startMsgToConsume == True and (time.time() - latchStartTime) > 5.9):    #this amounts to about 6 seconds of on time for the remote start button
			GPIO.output(radioPin, GPIO.LOW)
			startMsgToConsume = False

		
		try:
			data = s.recv(1024)	
		except:
			PORT = 24235  #dummy command to catch non-receive error
			
		if data == 'StartCar <EOF>':
			#carState = 'ON' if carState == 'OFF' else 'OFF'
			#pinState = 1 if carState == 'ON' else 0
			#s.sendall('ACK_Status CarState:' + carState + ' <EOF>')
			if startMsgToConsume == False:
				GPIO.output(radioPin, GPIO.HIGH)
				latchStartTime = time.time()
				modemTimer = time.time()                 #reset 10 second timer to give car a chance to turn on from 6 second press
				startMsgToConsume = True
				print 'CarStart message received' 
				
		data = ''	
	
	elif modemState == 0:
		print 'Shutting off socket and killing WVDIAL'
		s.shutdown(socket.SHUT_RDWR)
		s.close()
	    #killWVdial process by name
		#subprocess.Popen("xterm -hold -e KillWVDIAL.sh" , shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)		
		proc.terminate()
		modemState = 1
		modemTimer = time.time()
	elif modemState == 1 and time.time() - modemTimer > 2: 
		print 'Cutting power to modem'
		GPIO.output(modemPwrPin, GPIO.LOW)  #cut power to modem
		modemTimer = time.time()
		modemState = 2
	elif modemState == 2 and time.time() - modemTimer > 20:     #will be 90 seconds.  Modem has been off for awhile...plug back in, wait 12 seconds in state 3, then fire up 
		print 'Waited 90 seconds, turning modem on'
		GPIO.output(modemPwrPin, GPIO.HIGH)
		modemTimer = time.time()
		modemState = 3
	elif modemState == 3 and time.time() - modemTimer > 12:      #modem should be booted by now, run wvdial, wait another ten seconds in state 3, then put process in operating mode 4
		print 'Waited for 10s for modem to boot, now starting wvdial'
		#start WVDial process.  Run output into log file
		#subprocess.Popen("xterm -hold -e StartWVDIAL.sh" , shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
		proc = subprocess.Popen(['nohup', '/usr/bin/wvdial', '&']) 
		modemTimer = time.time()
		modemState = 4
		
	elif modemState == 4 and time.time() - modemTimer > 9.9:      #wvdial should be running by now - put process in operating mode 0
		modemTimer = time.time()
		print 'Waited 10s for wvdial, now entering main message loop'
		modemState = 0	
		s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		try:
			s.connect((HOST, PORT))
		except Exception as  inst:
			print inst
			
		s.setblocking(0)   #make non blocking after a connection is actually made	
		s.sendall('<EOF>')   #makes me a real client at dah soyvah
	
	
	time.sleep(0.0005)
	
		
	
