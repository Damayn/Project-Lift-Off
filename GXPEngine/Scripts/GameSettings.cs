﻿using GXPEngine;
using System;
using System.Collections.Generic;
using System.Runtime;


public class GameSettings
{
    
    public bool hasGameStarted = false;
    public bool isGameOver = false;

    public bool hasEnteredName = false;
    public bool hasAName = false;

    public bool inSelectionMode = false;

    public bool inSeedBagSelection = false;
    public bool inPotSelection = false;

    public bool isTimePaused = false;

    public String sceneName;

    public string[] people = { "Faces.png", "Rolling.png", "Viking.png", "cat.png", "Bird.png" };

    public float currentProductionValue { get; set; }
    public int points = 50;

    public int currentLevel = 1;

    public string playerName;

    public bool play = false;
    public bool playAgain = false;

    public bool scream = false;

    public List<string> collectedFlowers = new List<string>();
    public List<Customers> customers = new List<Customers>();
    public GameSettings()
    {
    
    }

    public void ResetSettings () 
    {
        hasGameStarted = false;
        hasAName = false;
        hasEnteredName = false;
        isGameOver = false;
        isTimePaused = false;
        currentLevel = 1;
        playerName = "";
        currentProductionValue = 0;

        customers.Clear();
    }

}
