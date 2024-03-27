#include <PulseSensorPlayground.h>

const int PulseWire = 0;
const int LED = LED_BUILTIN;
int Threshold = 500;

PulseSensorPlayground pulseSensor;

unsigned long previousMillis = 0;    // Variable to store the time when BPM was last sent
const long interval = 100;          // Interval between sending BPM data (milliseconds)

void setup() {
  Serial.begin(9600);
  pulseSensor.analogInput(PulseWire);
  pulseSensor.blinkOnPulse(LED);
  pulseSensor.setThreshold(Threshold);
  pulseSensor.begin();
}

void loop() {
  unsigned long currentMillis = millis();

  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;

    if (pulseSensor.sawStartOfBeat()) {
      int myBPM = pulseSensor.getBeatsPerMinute();
      Serial.println(myBPM);  // Write BPM data to serial port
    }
  }
  // Keep the loop running without delay
}
