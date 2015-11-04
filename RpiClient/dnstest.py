import socket
import RPi.GPIO as GPIO
import time
import os
import subprocess, signal

dnstime = time.time()
print socket.gethostbyname('aws.mjcpe.com')
print repr(time.time() - dnstime)