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


        if (settings.inPotSelection || settings.inSeedBagSelection)
        {
            ReadButton.currentColor = " 139, 128, 0, 0";
        }
        else if (settings.menuState)
        {
            ReadButton.currentColor = " 200, 33, 63, 0";
        }

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