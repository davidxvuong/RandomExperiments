bool flip = false;
int tmr1_counter;
int brightness = 255;

void setup() {
  //Set up pins as inputs/outputs
  for (int i = 2; i < 10; i++){
    pinMode(i, OUTPUT);
  }
  pinMode(10, INPUT);
  
  noInterrupts();
  
  //set up comparison register and timer
  //timer will run until the timer counter is equal tothe comparison register, in which an interrupt is generated
  OCR0A = 0xAF;
  TIMSK0 |= _BV(OCIE0A);
  
  interrupts();
}

void loop() {
  // wait until the push button has been pressed
  while (digitalRead(10) != HIGH){
  }
  
  //turn on each LED from left to right (and vice versa). Intensity of the LED based on the measured value of the potentiometer
  do {
    for (int j = 2; j < 10; j++) {
      if (flip == false)
        analogWrite(j, brightness);
      else
        analogWrite(11-j, brightness);
        
      delay(150);
    }
    
    //turn off each   from left to right (and vice versa)
    for (int k = 9; k > 1; k--) {
      if (flip == false)
        digitalWrite(k,LOW);
      else
        digitalWrite(11-k,LOW);
        
      delay(150);
    }
    
    //negate the boolean variable 'flip' to allow the code to at least run twice - once to turn on the LEDs from left to right
    //and then the second time from right to left
    flip = !flip;
  }while (flip != false);
  
  //flash all the LEDs five times. Intensity is based on the measured value of the potentiometer
  for (int i = 0; i < 5; i++) {
    for (int j = 2; j < 10; j++) {
      analogWrite(j, brightness);
    } 
    
    delay(200);
    for (int j = 2; j < 10; j++) {
      digitalWrite(j, LOW);
    } 
    delay(200);
  }
}

//Interrupt Service ROutine from the timer interrupt.
SIGNAL(TIMER0_COMPA_vect) {
  //Read the value from the analog input pin A0, then check if it is different than the one stored in the brightness variable
  
  int in = analogRead(A0);
  in = map(in, 0, 1023, 0, 255);
  if (in != brightness) {
    brightness = in;
  }
}
