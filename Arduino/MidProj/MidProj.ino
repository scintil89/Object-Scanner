#include <Servo.h>

////////////////////////////////////////////////////////////////////////////////////
// IR Sensor
int ir1 = A0;
int ir2 = A1;
int Stop = A2;

int irValue1 = 0;
int irValue2 = 0;
////////////////////////////////////////////////////////////////////////////////////
// Servo Morter
Servo servoMotor;
Servo linearMotor;
int SERVO = 9;
int LINEAR = 8;
int servoValue = 0;
int linearValue = 0;

int linearMin = 5;
int linearMax = 100;


int rotateValue = 15;

////////////////////////////////////////////////////////////////////////////////////
//
bool isRun = true;
int height = 0;

void SetStrokePerc(float strokePercentage)
{
  if (strokePercentage >= 1.0 && strokePercentage <= 99.0)
  {
    int usec = 1000 + strokePercentage * (2000 - 1000) / 100.0;
    linearMotor.writeMicroseconds(usec);
  }
}

void SetStrokeMM(int strokeReq, int strokeMax)
{
  SetStrokePerc(((float)strokeReq) / strokeMax);
}


////////////////////////////////////////////////////////////////////////////////////
void setup()
{
  servoMotor.attach(SERVO);
  linearMotor.attach(LINEAR);
  Serial.begin(9600);

  //리니어 모터 초기화
  SetStrokePerc(linearMin);

  //서보 모터 초기화
  servoMotor.write(0);

  //버튼 초기화
  //pinMode(BUTTON, INPUT);
}


////////////////////////////////////////////////////////////////////////////////////
void loop()
{
  //Test Code
  //servoMotor.write(servoValue);

  if (isRun)
  {
    delay(500);
    //calculate height
    for (int i = linearMin; i < linearMax; i++)
    {

      SetStrokePerc(i);

      delay(1000);

      int val = analogRead(ir2);

      Serial.println(val);
              
      //측정 값이 한계치로 가면 그 지점 높이로 지정
      if (analogRead(Stop) > 500)  //val > 400 || val < 30
      {
        height = i;
        break;
      }
      else
      {
        height = i;
      }
    }

    Serial.print(height);
    Serial.println("~");

    //리니어 모터 초기화
    SetStrokePerc(linearMin);
    //서보 모터 초기화
    servoMotor.write(0);

    delay(2000);

    //Linear Loop
    for (int h = linearMin; h <= height; h += 5)
    {
      SetStrokePerc(h);
      Serial.println("$$");

      delay(2000);

      servoMotor.write(0);

      //Rotate Loop
      for (int i = 0; i < 180; i += rotateValue)
      {
        //Rotate
        servoValue = i;
        servoMotor.write(servoValue);

        //Serial.print("mortor Value: ");
        //Serial.print(servoValue);

        delay(2000);

        //Sensing 4~30cm
        //오차가 있을 수 있으므로 같은 지점 일정 횟수 스캔 후 평균값 구하자
        int sens1_1 = analogRead(ir1);
        int sens2_1 = analogRead(ir2);
        delay(10);
        int sens1_2 = analogRead(ir1);
        int sens2_2 = analogRead(ir2);
        delay(10);
        int sens1_3 = analogRead(ir1);
        int sens2_3 = analogRead(ir2);
        delay(10);
        int sens1_4 = analogRead(ir1);
        int sens2_4 = analogRead(ir2);
        delay(10);
        int sens1_5 = analogRead(ir1);
        int sens2_5 = analogRead(ir2);
        delay(10);
        int sens1_6 = analogRead(ir1);
        int sens2_6 = analogRead(ir2);
        delay(10);
        int sens1_7 = analogRead(ir1);
        int sens2_7 = analogRead(ir2);
        delay(10);
        int sens1_8 = analogRead(ir1);
        int sens2_8 = analogRead(ir2);
        delay(10);
        int sens1_9 = analogRead(ir1);
        int sens2_9 = analogRead(ir2);
        delay(10);
        int sens1_10 = analogRead(ir1);
        int sens2_10 = analogRead(ir2);
        delay(10);

        irValue1 = (sens1_1 + sens1_2 + sens1_3 + sens1_4 + sens1_5 + sens1_6 + sens1_7 + sens1_8 + sens1_9 + sens1_10) / 10; //sens1;
        irValue2 = (sens2_1 + sens2_2 + sens2_3 + sens2_4 + sens2_5 + sens2_6 + sens2_7 + sens2_8 + sens2_9 + sens2_10) / 10; //sens1;

        //Serial Out String
        String out1;
        String out2;

        ///IR SENSOR 1
        //회전각
        out1 += servoValue;         //Serial.print(servoValue);
        //
        out1 += '&';                //Serial.print('&');
        //거리
        out1 += irValue1;           //Serial.print(irValue1);
        //
        out1 += '&';                //Serial.print('&');
        //높이
        out1 += h;                  //Serial.println(h);
        
        Serial.println(out1);
        
        delay(200);

        ///IR SENSOR 2
        //회전각
        out2 += 180 + servoValue;        //Serial.print(180 + servoValue);
        //
        out2 += '&';                     //Serial.print('&');
        //거리
        out2 += irValue2;               //Serial.print(irValue2);
        //
        out2 += '&';                    //Serial.print('&');
        //y값
        out2 += h;                      //Serial.println(h);

        Serial.println(out2);


      }

      //
      delay(200);
      Serial.println("@@");
    }

    //END
    Serial.println("!!");
    isRun = false;
  }

}

