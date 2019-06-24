#include <Arduino.h>

const int nbSlider = 6;
int slider[nbSlider] = {A0,A1,A2,A3,A4,A5};

const int numReadings = 10;
int readIndex = 0;
int tabPos[nbSlider][numReadings];
float pos[nbSlider];

int motorSwitch[nbSlider] = {22,50,46,26,30,42};
int motorPinPlus[nbSlider] = {25,53,49,29,33,45};
int motorPinMinus[nbSlider] = {24,52,48,28,32,44};

const int nbButton = 3;
int ROT_B[nbButton] = {3,19,21};
int ROT_A[nbButton] = {2,18,20};
int buttonSwitch[nbButton] = {4,5,6};

volatile int lastEncoded[nbButton] = {0,0,0};
volatile long encoderValue[3] = {0,0,0};

int digit1;
int digit2;
int digit3;
int digit4;
int val[nbSlider] = {0,0,0,0,0,0};
int id;

String message;

bool moveSlider[nbSlider] = {false,false,false,false,false,false};

void setup(){
  Serial.begin(250000);

  for(int i=0;i<nbSlider;i++){
    pinMode(slider[i], INPUT);
    pinMode(motorSwitch[i], OUTPUT);
    pinMode(motorPinPlus[i], OUTPUT);
    pinMode(motorPinMinus[i], OUTPUT);
  }

  for(int i=0;i<nbButton;i++){
    pinMode(ROT_B[i], INPUT);
    pinMode(ROT_A[i], INPUT);
    pinMode(buttonSwitch[i], INPUT);
    digitalWrite(ROT_B[i], HIGH);
    digitalWrite(ROT_A[i], HIGH);
    digitalWrite(buttonSwitch[i], HIGH);
  }
  attachInterrupt(0, updateEncoder0, CHANGE);
  attachInterrupt(1, updateEncoder0, CHANGE);
  attachInterrupt(4, updateEncoder1, CHANGE);
  attachInterrupt(5, updateEncoder1, CHANGE);
  attachInterrupt(2, updateEncoder2, CHANGE);
  attachInterrupt(3, updateEncoder2, CHANGE);
}

void sliderToVal(int id, int val){
  int pos = analogRead(slider[id]);
  if(abs(pos-val)>20){
    if(pos>val){
      digitalWrite(motorPinPlus[id], HIGH);
      digitalWrite(motorPinMinus[id], LOW);
    }
    else{      
      digitalWrite(motorPinPlus[id], LOW);
      digitalWrite(motorPinMinus[id], HIGH);                  
    }
    analogWrite(motorSwitch[id], 255);
  }
  else{
    digitalWrite(motorPinPlus[id], LOW);
    digitalWrite(motorPinMinus[id], LOW);
    analogWrite(motorSwitch[id], 0);
    moveSlider[id] = false;
  }
}

void loop(){
  for(int i=0;i<nbSlider;i++){
    tabPos[i][readIndex] = analogRead(slider[i]);
    pos[i] = 0;
  }
  readIndex = (readIndex+1)%numReadings;
  for(int i=0;i<nbSlider;i++){
    for(int j=0;j<numReadings;j++){
      pos[i] += tabPos[i][j];
    }
    pos[i] /= numReadings;
  }  
  for(int i=0;i<nbSlider;i++){
    Serial.print(round(pos[i]));
    Serial.print(" ");
  }
  for(int i = 0;i<3; i++){
    Serial.print(encoderValue[i]);
    Serial.print(" ");
    if(digitalRead(buttonSwitch[i])){
    Serial.print("0");
    }
    else{
      Serial.print("1");
    }
    Serial.print(" ");
  }
  Serial.print("\n");
  
  if(Serial.available()>0){
    message = Serial.readStringUntil('\n');
    id = message[0]-'0';
    digit1 = message[2]-'0';    
    digit2 = message[3]-'0';    
    digit3 = message[4]-'0';
    digit4 = message[5]-'0';
    if(id>=0 && id <=5){
      val[id] = digit1 * 1000 + digit2 * 100 + digit3 * 10 + digit4;
      val[id] = (abs(val[id]))%1024;
      moveSlider[id] = true;
    }
    else if(id ==6){
      for(int i=0;i<6;i++){
        analogWrite(motorSwitch[i],0);
        digitalWrite(motorPinPlus[i], LOW);
        digitalWrite(motorPinMinus[i], LOW);
        moveSlider[i] = false;
      }
    }
  }
  for(int i=0;i<6;i++){
    if(moveSlider[i]){
      sliderToVal(i,val[i]);
    }
  }
}

void updateEncoder0(){
  int MSB = digitalRead(ROT_A[0]);
  int LSB = digitalRead(ROT_B[0]);
  int encoded = (MSB << 1) |LSB;  
  int sum  = (lastEncoded[0] << 2) | encoded;
  if(sum == 0b1101 || sum == 0b0100 || sum == 0b0010 || sum == 0b1011) encoderValue[0] ++;
  if(sum == 0b1110 || sum == 0b0111 || sum == 0b0001 || sum == 0b1000) encoderValue[0] --;
  lastEncoded[0] = encoded;
}

void updateEncoder1(){
  int MSB = digitalRead(ROT_A[1]);
  int LSB = digitalRead(ROT_B[1]);
  int encoded = (MSB << 1) |LSB;
  int sum  = (lastEncoded[1] << 2) | encoded;
  if(sum == 0b1101 || sum == 0b0100 || sum == 0b0010 || sum == 0b1011) encoderValue[1] ++; 
  if(sum == 0b1110 || sum == 0b0111 || sum == 0b0001 || sum == 0b1000) encoderValue[1] --;
  lastEncoded[1] = encoded;
}

void updateEncoder2(){  
  int MSB = digitalRead(ROT_A[2]);
  int LSB = digitalRead(ROT_B[2]);
  int encoded = (MSB << 1) |LSB;
  int sum  = (lastEncoded[2] << 2) | encoded;
  if(sum == 0b1101 || sum == 0b0100 || sum == 0b0010 || sum == 0b1011) encoderValue[2] ++;
  if(sum == 0b1110 || sum == 0b0111 || sum == 0b0001 || sum == 0b1000) encoderValue[2] --;
  lastEncoded[2] = encoded;
}
