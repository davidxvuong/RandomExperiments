#define topLight 13
#define middleLight 12
#define bottomLight 11
String input = "";

void setup() {
  // put your setup code here, to run once:
  pinMode(topLight, OUTPUT);
  pinMode(middleLight, OUTPUT);
  pinMode(bottomLight, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  if (Serial.available() > 0) {
    input = "";
    while(Serial.available() > 0){
      input += char(Serial.read());
      delay(2);
    }
    
    Serial.println(input);
    if (input == "1") {
      digitalWrite(topLight, HIGH);
    }
    else if (input == "2") {
      digitalWrite(middleLight, HIGH);
    }
    else if (input == "3") {
      digitalWrite(bottomLight, HIGH);
    }
    else {
      return;
    }
    delay(1000);
    digitalWrite(bottomLight, LOW);
    digitalWrite(middleLight, LOW);
    digitalWrite(topLight, LOW);
    delay(1000);
  }
}
