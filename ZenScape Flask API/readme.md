# ZenScape Flask API

## Description

An API that interfaces the pulse sensor with the ZenScape Unity Project.

## Usage

1. Create a virtual environment:
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

4. Run the Flask Server on the local network:
    ```
    flask run --host=0.0.0.0
    ```

5. Access the API endpoints:

    - Pulse data endpoint (local): `http://localhost:5000/pulse`
    - Pulse data endpoint from other devices use the ip address of the pc hosting: for example http://192.168.0.17:5000/'
