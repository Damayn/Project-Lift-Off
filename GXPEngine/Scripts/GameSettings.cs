using GXPEngine;
using System;

public class GameSettings
{
    public bool hasGameStarted = false;
    public bool isGameOver = false;
    public bool inSelectionMode = false;

    public bool inSeedBagSelection = false;
    public bool inPotSelection = false;

    public string[] people = { "Faces.png", "Rolling.png", "Rolling.png", "Faces.png", "Faces.png" };
    public GameSettings()
    {
    
    }

    void Update () 
    {
        
    }
}
