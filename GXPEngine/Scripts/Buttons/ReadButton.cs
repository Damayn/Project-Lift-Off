using GXPEngine;
using System;
using System.IO.Ports;

class ReadButton : GameObject
{
    SerialPortManager serialPort;

    float button1;
    float button2;
    float button3;

    public bool button1Pressed;
    public bool button2Pressed;
    public bool button3Pressed;

    public ReadButton (SerialPortManager serialPort) : base ()
    {
        this.serialPort = serialPort;

        serialPort.Open (); 
    }

    void Update () 
    {
        GetButtonValue ();
        
        if (button1 == 0)
        {
            button1Pressed = true;
        } else if (button2 == 0)
        {
            button2Pressed = true;
        } else if (button3 == 0)
        {  
            button3Pressed = true;
        }

    }

    void GetButtonValue () 
    {
        button1 = serialPort.ButtonState(1);
        button2 = serialPort.ButtonState(5) ;
        button3 = serialPort.ButtonState(9);
    }
}