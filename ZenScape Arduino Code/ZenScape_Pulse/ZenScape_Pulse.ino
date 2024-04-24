#include <PulseSensorPlayground.h>

const int PulseWire = 0;
const int LED = LED_BUILTIN;
int Threshold = 500;

PulseSensorPlayground pulseSensor;

unsigned long previousMillis = 0;    // Variable to store the time when BPM was last sent
const long interval = 100;          // Interval between sending BPM data (milliseconds)

const int MIN_HEART_RATE = 40;
const int MAX_HEART_RATE = 200;

void setup() {
  Serial.begin(9600);
  pulseSensor.analogInput(PulseWire);
  pulseSensor.blinkOnPulse(LED);
  pulseSensor.setThreshold(Threshold);
  pulseSensor.begin();
}

// Function to check if a BPM value is within the reasonable heart rate range
bool isValidHeartRate(int bpm) {
    return (bpm >= MIN_HEART_RATE && bpm <= MAX_HEART_RATE);
}

void loop() {
  unsigned long currentMillis = millis();

  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;

    if (pulseSensor.sawStartOfBeat()) {
      int myBPM = pulseSensor.getBeatsPerMinute();
      
      // Check if the BPM value is within the reasonable heart rate range
      if (isValidHeartRate(myBPM)) {
        Serial.println(myBPM);  // Write BPM data to serial port if valid
      } else {
        // BPM value is not within the reasonable heart rate range, discard
        // Do nothing or add any additional handling here
      }
    }
  }
  // Keep the loop running without delay
}
