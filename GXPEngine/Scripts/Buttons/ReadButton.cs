using GXPEngine;
using System;
using System.Collections;
using System.Threading;

class ReadButton : GameObject
{
    SerialPortManager serialPort;
    GameSettings settings;

    float prevButton1State;
    float prevButton2State;
    float prevButton3State;
    float prevButton4State;

    float prevAccelerationX;
    float prevAccelerationY;
    float prevAccelerationZ;

    public static bool isMovingVertically = false;
    public static bool isMovingHorizontally = false;

    public static bool button1Pressed = false;
    public static bool button2Pressed = false;
    public static bool button3Pressed = false;
    public static bool button4Pressed = false;

    public static float joystickX = 0f;
    public static float joystickY = 0f;

    public static float joystickHled;
    public static float joystickReturnTime = 250;

    public static bool isJoystickReturning = false;
    private float joystickTimer = 0f;

    public static bool IsJoystickLeft;
    public static bool IsJoystickRight;
    public static bool IsJoystickUp;
    public static bool IsJoystickDown;

    public bool isHeld;

    public static string currentColor = "";
    public string prevColor = "";

    public ReadButton(SerialPortManager serialPort, GameSettings settings) : base()
    {
        this.serialPort = serialPort;
        this.settings = settings;
        serialPort.Open();
    }

    void Update()
    {
        // Check button presses
        CheckButtonPress(8, ref prevButton1State, ref button1Pressed);
        CheckButtonPress(9, ref prevButton2State, ref button2Pressed);
        CheckButtonPress(10, ref prevButton3State, ref button3Pressed);
        CheckButtonPress(11, ref prevButton4State, ref button4Pressed);


        // Check for downward movement
        CheckDownwardMovement();

        // Check for horizontal movement
        CheckHorizontalMovement();

        // Read joystick values
        GetJoystickValues();

        HandleJoystickReturning();


        if (Time.time - joystickHled > joystickReturnTime)
        {
            isHeld = false;

            IsJoystickLeft = joystickX > 800;
            IsJoystickRight = joystickX < 150;
            IsJoystickUp = joystickY < 150;
            IsJoystickDown = joystickY > 800;

            // Update joystickHled only when any joystick state changes
            if (IsJoystickDown && !isHeld)
            {
                joystickHled = Time.time;
                IsJoystickDown = true;
                Console.WriteLine(IsJoystickDown);
            }
            else if (IsJoystickUp && !isHeld)
            {
                joystickHled = Time.time;
                IsJoystickUp = true;
                Console.WriteLine(IsJoystickUp);
            }
            else if (IsJoystickRight && !isHeld)
            {
                joystickHled = Time.time;
                IsJoystickRight = true;
                Console.WriteLine(IsJoystickRight);
            }
            else if (IsJoystickLeft && !isHeld)
            {
                joystickHled = Time.time;
                IsJoystickLeft = true;
                Console.WriteLine(IsJoystickLeft);
            }
        }
        else
        {
            IsJoystickDown = false;
            IsJoystickLeft = false;
            IsJoystickRight = false;
            IsJoystickUp = false;
        }

        ChangeMode ();
    }

    void ChangeMode ()
    {
        if (prevColor != currentColor) 
        {
            prevColor = currentColor;
            serialPort.Send("ColorIdentifier", currentColor);
        }

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

    void CheckDownwardMovement()
    {
        // Get accelerometer data
        float accelerationZ = serialPort.ButtonState(7);

        // Calculate the change in acceleration along the Z-axis
        float deltaZ = accelerationZ - prevAccelerationZ;

        // Set a threshold for detecting downward movement
        float threshold = -0.4f;

        // Check if the change in acceleration along the Z-axis exceeds the threshold
        if (deltaZ < threshold)
        {
            // Movement detected downward
            isMovingVertically = true;
            Console.WriteLine("Vertical movement!");
        }
        else
        {
            // No downward movement detected
            isMovingVertically = false;
        }

        // Update previous acceleration value for next frame
        prevAccelerationZ = accelerationZ;
    }

    void CheckHorizontalMovement()
    {
        // Get accelerometer data
        float accelerationX = serialPort.ButtonState(5);
        float accelerationY = serialPort.ButtonState(4);
        float accelerationZ = serialPort.ButtonState(7);

        // Calculate the change in acceleration along the X-axis
        float deltaX = Math.Abs(accelerationX - prevAccelerationX);

        // Set a threshold for detecting horizontal movement
        float threshold = 0.8f;

        // Check if the change in acceleration along the X-axis exceeds the threshold
        if (deltaX > threshold)
        {
            // Horizontal movement detected
            isMovingHorizontally = true;
            Console.WriteLine("horizontal movement detected");
        }
        else
        {
            isMovingHorizontally = false;
        }

        // Update previous acceleration value for next frame
        prevAccelerationX = accelerationX;
    }

    void GetJoystickValues()
    {
        joystickX = serialPort.ButtonState(0);
        joystickY = serialPort.ButtonState(1);
    }


    void HandleJoystickReturning()
    {
        if (isJoystickReturning)
        {
            if (joystickTimer >= joystickReturnTime)
            {
                isJoystickReturning = false;
                joystickTimer = 0f;

                // Reset joystickHled to indicate that joystick values can be returned again
                joystickHled = 0f;
            }
        }
    }
}
