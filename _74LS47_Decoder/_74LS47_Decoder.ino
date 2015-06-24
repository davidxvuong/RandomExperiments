String input;
byte num;
int counter = 2;

void setup() {
  // put your setup code here, to run once:
  for (int i = 2; i < 6; i++) {
    pinMode(i, OUTPUT);
    digitalWrite(i, HIGH);
  }
  
  Serial.begin(9600);
  Serial.print("Enter a number between 0 to 15: ");
}

void loop() {
  // put your main code here, to run repeatedly:
  if (Serial.available()> 0) {
    input = "";
    counter = 2;
    while (Serial.available() > 0) {
      input += char(Serial.read());
      delay(2);
    }

    num = (byte)(input.toInt());
    Serial.println(num);
    for (int i = 1; i <= 8; i = i << 1) {
      Serial.print(counter);
      if (num & i) {
        digitalWrite(counter, HIGH);
        Serial.println(" ON");
      }
      else {
        digitalWrite(counter, LOW);
        Serial.println(" OFF");
      }
      counter++;
    }
    
    delay(5000);
    
    for (int i = 2; i < 6; i++) {
      digitalWrite(i, HIGH);
    }
    
    Serial.println();
    Serial.print("Enter a number between 0 to 15: ");
  } 
}
