from flask_cors import CORS
import serial
from flask import Flask, jsonify
import threading
import random
import time

app = Flask(__name__)
CORS(app)

pulse = 0  # Global variable to store pulse data

@app.route('/pulse', methods=['GET'])
def get_pulse_data():
    global pulse
    try:
        ser = serial.Serial('COMx', 9600)  # Replace 'COMx' with the actual serial port your Arduino is connected to
        pulse = int(ser.readline().decode().strip())
        return jsonify({'pulse_data': pulse})
    except Exception as e:
        return jsonify({'An error occured': e})
  

@app.route('/', methods=['GET'])
def home():
    return "The backend is running for ZenScape!"

if __name__ == '__main__':
    # Run the Flask application
    app.run(debug=True)