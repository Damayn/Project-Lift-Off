using GXPEngine;
using System;

public enum Gamestate
{

    //hasGameStarted,
    //isGameOver,
    inSelectionMode,
    inSeedBagSelection,
    inPotSelection,
    //isTimePaused,
    //isTimeUnPaused

}

public class GameSettings
{

    public bool isTimePaused = false;

    public bool hasGameStarted = false;

    public bool isGameOver = false;

    public Gamestate hey;

    public string[] people = { "Faces.png", "Rolling.png", "Viking.png", "cat.png", "Bird.png" };

    public float currentProductionValue { get; set; }

    public int currentLevel = 1;
    public GameSettings()
    {
    
    }

    void Update () 
    {
        
    }
}
