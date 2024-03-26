# ZenScape Flask API

## Description

A Python Script that interfaces the pulse sensor with the ZenScape Unity Project through the use of Firebase.
Before running the script, make sure that the Putty terminal is set up and the script is uploaded to the Arduino Uno.

## Usage

1. Create a virtual environment called env:
    ```
    py -3 -m venv env 
    ```

2. Activate the environment:
    ```
    .\\env\Scripts\activate      
    ```

3. Install the dependencies:
    ```
    pip install -r requirements.txt
    ```

4. Run the pulse listener script:
    ```
    python pulseListener.py
    ```

5. Access the data through Firebase:
    As the pulse is printed from the pulse sensor to the COM port to the log file, the live pulse, the average pulse, and the pulse history are updated in Firebase. This data will be read and used in the Zenscape Unity app.