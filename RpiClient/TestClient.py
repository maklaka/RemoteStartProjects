# Echo client program
import socket
import RPi.GPIO as GPIO
import time
import os



carSensePin = 11
radioPin = 15
modemPwrPin = 26

GPIO.setmode(GPIO.BOARD)
GPIO.setup(carSensePin, GPIO.IN)
GPIO.setup(radioPin, GPIO.OUT, initial=GPIO.LOW)
GPIO.setup(modemPwrPin, GPIO.OUT, initial=GPIO.HIGH)

HOST = '54.148.253.160'    # The remote host
PORT = 24235              # The same port as used by the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
try:
	s.connect((HOST, PORT))
except:
	os.system("sudo shutdown -r now")
	
s.setblocking(0)   #make non blocking after a connection is actually made
s.sendall('<EOF>')   #makes me a real client at dah soyvah

lastPingTime = time.clock()
lastModemActivation = time.clock()
latchStartTime = time.clock()

carState = 'OFF'
pinState = 0
modemState = 1

data = ''

GPIO.output(radioPin, pinState)

startMsgToConsume = False


print 'Everything initialized....'

while 1:
    # assign to carState according to input pin  
	
	if GPIO.input(carSensePin) == True and carState == 'OFF':
		carState = 'ON'
		s.sendall('ACK_Status CarState:' + carState + ' <EOF>')  #just turned on,  send status off!
	elif GPIO.input(carSensePin) == False and carState == 'ON':
		carState = 'OFF'	
	
	if  (time.clock() - lastPingTime) > 0.08:                        # this  will double as connection tester
		lastPingTime = time.clock()
		try:
			s.sendall('ACK_Status CarState:' + carState + ' <EOF>')
		except:
			os.system("sudo shutdown -r now")   #dummy command but will restart modem or restart RPI because a transmission error'd out?  connection must be down?
			
		print 'Sent the ping' + '     ' + 'ACK_Status CarState:' + carState + ' <EOF>'
	
	if (startMsgToConsume == True and (time.clock() - latchStartTime) > 0.04):    #this amounts to about 6 seconds of on time for the remote start button
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
		GPIO.output(radioPin, GPIO.HIGH)
		latchStartTime = time.clock()
		startMsgToConsume = True
		print 'CarStart message received' 

	
	
	GPIO.output(modemPwrPin, GPIO.LOW)
	modemState == 0
	
			
	data = ''	
	time.sleep(0.05)
	
		
	
