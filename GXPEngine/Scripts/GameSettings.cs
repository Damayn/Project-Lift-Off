using GXPEngine;
using System;


public class GameSettings
{
    
    public bool hasGameStarted = false;
    public bool isGameOver = false;
    public bool inSelectionMode = false;

    public bool inSeedBagSelection = false;
    public bool inPotSelection = false;

    public bool isTimePaused = false;

    public string[] people = { "Faces.png", "Rolling.png", "Viking.png", "cat.png", "Bird.png" };
    public GameSettings()
    {
    
    }

    void Update () 
    {
        
    }
}
