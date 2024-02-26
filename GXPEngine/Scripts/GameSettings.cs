using GXPEngine;
using System;
using System.Collections.Generic;


public class GameSettings
{
    
    public bool hasGameStarted = false;
    public bool isGameOver = false;
    public bool inSelectionMode = false;

    public bool inSeedBagSelection = false;
    public bool inPotSelection = false;

    public bool isTimePaused = false;

    public string[] people = { "Faces.png", "Rolling.png", "Viking.png", "cat.png", "Bird.png" };

    public float currentProductionValue { get; set; }

    public int currentLevel = 2;

    public List<string> collectedFlowers = new List<string>();
    public List<Customers> customers = new List<Customers>();
    public GameSettings()
    {
    
    }

    void Update () 
    {
        
    }
}
