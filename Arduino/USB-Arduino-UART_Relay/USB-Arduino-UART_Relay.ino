#include <SoftwareSerial.h>
#include <Adafruit_Fingerprint.h>
SoftwareSerial scannerSerial(5, 6);

Adafruit_Fingerprint finger = Adafruit_Fingerprint(&scannerSerial);

uint8_t id;

void setup() {
  pinMode(12, INPUT_PULLUP);
  Serial.begin(57600);
  while(!Serial);
  delay(100);
  Serial.println("HANDSHAKE");
  finger.begin(57600);
  if (finger.verifyPassword()) {
    Serial.println("SCANNER_FOUND");
  } else {
    Serial.println("SCANNER_NOTFOUND");
    while (1) { delay(1); }
  }

  finger.getTemplateCount();
  Serial.print("SCANNER_TEMPLATES_"); Serial.println(finger.templateCount);
}

void loop() { 
  if (Serial.available() > 0) {
    int incomingString = Serial.read();
      clearBuffer();
    switch (incomingString){
      case 0x01:
      Serial.print("SCANNER_TEMPLATES_"); Serial.println(finger.templateCount);
      break;
      case 0x02:
      clearBuffer();
      Serial.println("OK");
      fingerWizard();
      break;
      case 0x03:
      clearBuffer();
      getFingerprintIDez();
      Serial.println("END");
      break;
      case 0x04:
      clearBuffer();
      finger.emptyDatabase();
      Serial.println("OK");
      break;
      default:
      clearBuffer();
      break;
      case 0x05:
      clearBuffer();
      Serial.println("OK");
      deleteWizard();
      break;
      case 0x06:
  if (digitalRead(12) < 1){
    Serial.println("SCANNER_START_READ");
    clearBuffer();
    getFingerprintIDez();
    Serial.println("END");
    } else{
      Serial.println("NO");
      }
      break;
      }
  }
delay(50);
}

void deleteWizard(){
  while (!(Serial.available() > 0)){ delay(10); }
  byte numma = Serial.read();
  id = numma;
  Serial.println(id);
  //clearBuffer();

  uint8_t p = finger.deleteModel(id);

  switch (p){
    case FINGERPRINT_OK:
    Serial.println("OK");
    return;
    case FINGERPRINT_PACKETRECIEVEERR:
    Serial.println("PACKREC");
    return;
    case FINGERPRINT_BADLOCATION:
    Serial.println("BADLOC");
    return;
    case FINGERPRINT_FLASHERR:
    Serial.println("FLASHERR");
    return;
    default:
    Serial.println("ERROR");
    return;
    }
}

void clearBuffer()
{
  while (Serial.available() > 0){
    Serial.read();
    }
    return;
  }

// returns -1 if failed, otherwise returns ID #
int getFingerprintIDez() {
  uint8_t p = finger.getImage();
  if (p != FINGERPRINT_OK) return -1;

  p = finger.image2Tz();
  if (p != FINGERPRINT_OK)  return -1;

  p = finger.fingerFastSearch();
  if (p != FINGERPRINT_OK)  return -1;
  
  // found a match!
  Serial.print("ID_"); Serial.println(finger.fingerID);
  return finger.fingerID; 
}

void fingerWizard() {
  //Get finger
  //id = (int(Serial.read()));
  while (!(Serial.available() > 0)){ delay(10); }
  byte numma = Serial.read();
  //Serial.println(numma);
  id = numma;
  Serial.println(id);
  int p = -1;
  while (p != FINGERPRINT_OK)
  {
    p = finger.getImage();
    switch (p)
    {
      case FINGERPRINT_OK:
      Serial.println("SCANNER_OK");
      break;
    case FINGERPRINT_NOFINGER:
      Serial.println("SCANNER_NOFINGER");
      break;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("SCANNER_ERROR");
      break;
    case FINGERPRINT_IMAGEFAIL:
      Serial.println("SCANNER_IMAGEFAIL");
      break;
    default:
      Serial.println("SCANNER_ERROR");
      break;
      }
    }

    //Got finger  
    p = finger.image2Tz(1);
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("SCANNER_OK");
      break;
    case FINGERPRINT_IMAGEMESS:
      Serial.println("SCANNER_IMAGEMESS");
      return p;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("SCANNER_ERROR");
      return p;
    case FINGERPRINT_FEATUREFAIL:
      Serial.println("SCANNER_IMAGEFAIL");
      return p;
    case FINGERPRINT_INVALIDIMAGE:
      Serial.println("SCANNER_IMAGEFAIL");
      return p;
    default:
      Serial.println("SCANNER_ERROR");
      return p;
  }
  
  delay(2000);
  p = 0;
  while (p != FINGERPRINT_NOFINGER) {
    p = finger.getImage();
  }
  Serial.print("TEMPLATE_"); Serial.println(id);
  p = -1;
  while (p != FINGERPRINT_OK)
  {
    p = finger.getImage();
    switch (p)
    {
      case FINGERPRINT_OK:
      Serial.println("SCANNER_OK");
      break;
    case FINGERPRINT_NOFINGER:
      Serial.println("SCANNER_NOFINGER");
      break;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("SCANNER_ERROR");
      break;
    case FINGERPRINT_IMAGEFAIL:
      Serial.println("SCANNER_IMAGEFAIL");
      break;
    default:
      Serial.println("SCANNER_ERROR");
      break;
      }
    }

      p = finger.image2Tz(2);
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("SCANNER_OK");
      break;
    case FINGERPRINT_IMAGEMESS:
      Serial.println("SCANNER_IMAGEMESS");
      return p;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("SCANNER_ERROR");
      return p;
    case FINGERPRINT_FEATUREFAIL:
      Serial.println("SCANNER_IMAGEFAIL");
      return p;
    case FINGERPRINT_INVALIDIMAGE:
      Serial.println("SCANNER_IMAGEFAIL");
      return p;
    default:
      Serial.println("SCANNER_ERROR");
      return p;
  } 
  p = finger.createModel();
  if (p == FINGERPRINT_OK) {
    Serial.println("MATCH");
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    Serial.println("ERROR");
    return p;
  } else if (p == FINGERPRINT_ENROLLMISMATCH) {
    Serial.println("NOMATCH");
    return p;
  } else {
    Serial.println("ERROR");
    return p;
  }   
  
  Serial.print("ID "); Serial.println(id);
  p = finger.storeModel(id);
  if (p == FINGERPRINT_OK) {
    Serial.println("OK");
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    Serial.println("ERROR");
    return p;
  } else if (p == FINGERPRINT_BADLOCATION) {
    Serial.println("BADLOC");
    return p;
  } else if (p == FINGERPRINT_FLASHERR) {
    Serial.println("FLASHERR");
    return p;
  } else {
    Serial.println("ERROR");
    return p;
  }   

  
  }

uint8_t getFingerprintID() {
  Serial.println("SCANNER_START_READ");
  uint8_t p = finger.getImage();
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("SCANNER_IMAGE_OK");
      break;
    case FINGERPRINT_NOFINGER:
      Serial.println("SCANNER_NOFINGER");
      return p;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("SCANNER_COMERR");
      return p;
    case FINGERPRINT_IMAGEFAIL:
      Serial.println("SCANNER_IMAGE_FAIL");
      return p;
    default:
      Serial.println("SCANNER_UNKERR");
      return p;
  }

  // OK success!

  p = finger.image2Tz();
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("SCANNER_IMAGE_CONVERT_OK");
      break;
    case FINGERPRINT_IMAGEMESS:
      Serial.println("SCANNER_IMAGEMESS");
      return p;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("SCANNER_COMERR");
      return p;
    case FINGERPRINT_FEATUREFAIL:
      Serial.println("SCANNER_FEATUREFAIL");
      return p;
    case FINGERPRINT_INVALIDIMAGE:
      Serial.println("SCANNER_INVALIDIMAGE");
      return p;
    default:
      Serial.println("SCANNER_UNKERR");
      return p;
  }
  
  // OK converted!
  p = finger.fingerFastSearch();
  if (p == FINGERPRINT_OK) {
    Serial.println("SCANNER_MATCH_OK");
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    Serial.println("SCANNER_COMERR");
    return p;
  } else if (p == FINGERPRINT_NOTFOUND) {
    Serial.println("SCANNER_MATCH_NOT");
    return p;
  } else {
    Serial.println("SCANNER_UNKERR");
    return p;
  }   
  
  // found a match!
  Serial.print("SCANNER_MATCH_ID_"); Serial.println(finger.fingerID); 
  //Serial.print(" with confidence of "); Serial.println(finger.confidence); 

  return finger.fingerID;
}


