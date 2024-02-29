using GXPEngine;
using System;

class ReadButton : GameObject
{
    SerialPortManager serialPort;

    float prevButton1State;
    float prevButton2State;
    float prevButton3State;

    float prevAccelerationX;
    float prevAccelerationY;
    float prevAccelerationZ;

    public bool button1Pressed = false;
    public bool button2Pressed = false;
    public bool button3Pressed = false;

    public ReadButton(SerialPortManager serialPort) : base()
    {
        this.serialPort = serialPort;
        serialPort.Open();
    }

    void Update()
    {
        // Check button presses
        CheckButtonPress(8, ref prevButton1State, ref button1Pressed);
        CheckButtonPress(9, ref prevButton2State, ref button2Pressed);
        CheckButtonPress(10, ref prevButton3State, ref button3Pressed);

        // Check for horizontal movement
        CheckHorizontalMovement();
    }

    void CheckButtonPress(int buttonIndex, ref float prevButtonState, ref bool buttonPressed)
    {
        float currentButtonState = serialPort.ButtonState(buttonIndex);

        // Check if the button was not pressed previously and is pressed now
        if (prevButtonState != 0 && currentButtonState == 0)
        {
            buttonPressed = true;
            Console.WriteLine("Button pressed");

            // Set the previous state to 0 to prevent further detections until button is released
            prevButtonState = 0;
        }
        else
        {
            buttonPressed = false;

            // Update the previous button state only if the button is not pressed
            if (currentButtonState != 0)
            {
                prevButtonState = currentButtonState;
            }
        }
    }

    void CheckHorizontalMovement()
    {
        // Get accelerometer data
        float accelerationX = serialPort.ButtonState (7);
        //float accelerationY = serialPort.AccelerometerY;
        //float accelerationZ = serialPort.AccelerometerZ;

        // Calculate the change in acceleration along the X-axis
        float deltaX = Math.Abs(accelerationX - prevAccelerationX);

        // Set a threshold for detecting horizontal movement
        float threshold = 1.0f;

        // Check if the change in acceleration along the X-axis exceeds the threshold
        if (deltaX > threshold)
        {
            // Horizontal movement detected
            Console.WriteLine("Horizontal movement detected!");
        }

        // Update previous acceleration values for next frame
        prevAccelerationX = accelerationX;
        //prevAccelerationY = accelerationY;
        //prevAccelerationZ = accelerationZ;
    }
}
