# Echo client program
import socket
import RPi.GPIO as GPIO
import time


GPIO.setmode(GPIO.BOARD)
GPIO.setup(26, GPIO.OUT, initial=GPIO.HIGH)


HOST = '54.148.253.160'    # The remote host
PORT = 24235              # The same port as used by the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))
s.setblocking(0)   #make non blocking after a connection is actually made
s.sendall('<EOF>')   #makes me a real client at dah soyvah

lastTime = time.clock()
carState = 'OFF'
data = ''
pinState = 0
GPIO.output(26, pinState)

startMsgToConsume = False
latchStartTime = time.clock()

print 'Everything initialized....'

while 1:
    # assign to carState according to input pin  (using output pin for now
	# if GPIO.input(some pin):
		
	
	
	if  (time.clock() - lastTime) > 0.08:
		lastTime = time.clock()
		s.sendall('ACK_Status CarState:' + carState + ' <EOF>')
		print 'Sent the ping' + '     ' + 'ACK_Status CarState:' + carState + ' <EOF>'
	
	if (startMsgToConsume == True and (time.clock() - latchStartTime) > 0.04):
		GPIO.output(26, GPIO.LOW)
		startMsgToConsume = False

	
	try:
		data = s.recv(1024)
	except:
		PORT = 24235
		
	if data == 'StartCar <EOF>':
		#carState = 'ON' if carState == 'OFF' else 'OFF'
		#pinState = 1 if carState == 'ON' else 0
		#s.sendall('ACK_Status CarState:' + carState + ' <EOF>')
		GPIO.output(26, GPIO.HIGH)
		latchStartTime = time.clock()
		startMsgToConsume = True
		print 'CarStart message received' 

		
	data = ''	
	time.sleep(0.05)
	
		
	
