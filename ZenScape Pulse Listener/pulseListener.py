import firebase_admin
from firebase_admin import credentials, db
import time

# Initialize Firebase
cred = credentials.Certificate("./key.json")
firebase_admin.initialize_app(cred, {
    'databaseURL': 'https://zenscape-b6d91-default-rtdb.firebaseio.com/'
})

# Path to your log file
log_file_path = './pulseLog.log'

# Function to read the most recent pulse from the log file and delete the line
def get_most_recent_pulse():
    pulse = None
    with open(log_file_path, 'r') as file:
        lines = file.readlines()
        if lines:
            try:
                pulse = int(lines[-1].strip())
            except Exception as e:
                exception = e
    return pulse

# Function to read the active user ID from Firebase
def read_active_user_id():
    ref = db.reference('active_user/userId')
    return ref.get()

# Function to read pulse history from Firebase for the active user
def read_pulse_history():
    active_user_id = read_active_user_id()
    if active_user_id:
        ref = db.reference('zenscape_users/' + active_user_id + '/pulse_history')
        return ref.get() or []
    return []

# Function to update pulse history for the active user in Firebase
def update_pulse_history(pulse):
    active_user_id = read_active_user_id()
    if active_user_id:
        pulse_history = read_pulse_history()
        pulse_history.append(pulse)
        ref = db.reference('zenscape_users/' + active_user_id + '/pulse_history')
        ref.set(pulse_history)

# Function to calculate average pulse from pulse history for the active user
def calculate_average_pulse():
    pulse_history = read_pulse_history()
    if pulse_history:
        total_pulse = sum(pulse_history)
        average_pulse = round(total_pulse / len(pulse_history))
        return average_pulse
    return None

# Function to push pulse to the active user in Realtime Database
def push_pulse_to_active_user(pulse):
    active_user_id = read_active_user_id()
    if active_user_id:
        ref = db.reference('zenscape_users/' + active_user_id + '/live_pulse')
        ref.set(pulse)

# Function to push average pulse to the active user in Realtime Database
def push_avg_pulse_to_active_user(pulse):
    active_user_id = read_active_user_id()
    if active_user_id:
        ref = db.reference('zenscape_users/' + active_user_id + '/avg_pulse')
        ref.set(pulse)

# Function to watch the log file and push data to Firebase
def watch_and_push_data():
    while True:
        most_recent_pulse = get_most_recent_pulse()
        avg_pulse = calculate_average_pulse()
        if most_recent_pulse is not None:
            push_pulse_to_active_user(most_recent_pulse)
            push_avg_pulse_to_active_user(avg_pulse)
            update_pulse_history(most_recent_pulse)
        time.sleep(.100)

# Start watching the log file for changes and pushing data to Firebase
watch_and_push_data()
