using GXPEngine;
using System;
using System.IO.Ports;

class ModeMananger : GameObject
{
    SerialPortManager serialPortManager;
    GameSettings settings;

    public static bool harvest;
    public static bool water;



    public ModeMananger(SerialPortManager serialPortManager, GameSettings settings) : base()
    {
        this.serialPortManager = serialPortManager;
        this.settings = settings;
    }

    void Update()
    {
        SetMenuMode();


        if (settings.hasGameStarted && settings.hasAName && settings.hasEnteredName)
        {
            ToggleWateringAndHarvestingState();
        }

        if (settings.wateringState)
        {
            ReadButton.currentColor = " 70 , 130, 180, 0";
        }
        // Check if in harvesting state
        else if (settings.harvestingState)
        {
            ReadButton.currentColor = " 255 , 0, 0, 0";
        }
        // Check if in menu state
        else if (settings.menuState)
        {
            ReadButton.currentColor = " 200, 33, 63, 0";
        }



        if (!settings.menuState && settings.hasGameStarted && settings.hasEnteredName)
        {
            
        }
    }

    void ToggleWateringAndHarvestingState ()
    {
        if (Input.GetKeyDown(Key.Y) || ReadButton.button2Pressed) // Change Key.Y to your desired button
        {
            // Toggle between watering and harvesting states
            if (settings.wateringState)
            {
                settings.wateringState = false;
                settings.harvestingState = true;
            }
            else
            {
                settings.wateringState = true;
                settings.harvestingState = false;
            }

            Console.WriteLine(settings.wateringState);
            Console.WriteLine(settings.harvestingState);
        } 
        //else
        //{
        //    settings.wateringState = false;
        //    settings.harvestingState = false;
        //}
    }

    void SetMenuMode()
    {
        foreach (GameObject child in game.GetChildren())
        {
            if (child is GameOver || child is MainMenu || child is NameMenu || child is OptionsMenu || child is Pause)
            {
                settings.menuState = true;
            }
            else
            {
                settings.menuState = false;
            }
        }
    }
}