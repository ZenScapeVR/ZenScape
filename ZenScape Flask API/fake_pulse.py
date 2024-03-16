import serial
import random
import time

def fake_pulse():
    try:
        ser = serial.Serial('COMx', 9600)  # Replace 'COMx' with the appropriate serial port
        print("Serial port opened successfully")
        while True:
            # Generate random pulse data (simulating the Arduino)
            pulse_data = random.randint(60, 100)
            # Write the pulse data to the serial port
            ser.write(f"{pulse_data}\n".encode())
            print(f"Sent pulse data: {pulse_data}")
            # Wait for a short interval (simulating real-time data)
            time.sleep(1)
    except Exception as e:
        print(f"An error occurred: {e}")
    finally:
        # Close the serial connection when exiting the loop
        if 'ser' in locals():
            ser.close()
            print("Serial port closed")

if __name__ == "__main__":
    fake_pulse()
