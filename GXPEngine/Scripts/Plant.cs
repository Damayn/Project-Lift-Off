﻿using GXPEngine;
using System;

class Plant : AnimationSprite
{
    Sprite waterDrop;
    Pot pot;
    GameSettings settings;

    int frame = 0;

    int timerStart;
    int timeToGrow;

    bool isWatered = false;
    bool isGrown = false;
    bool wilting = false;

    Random random = new Random();

    int animationChangeTimer;

    // Wilting when the plant starts growing
    int wiltingTimer;
    int growingStartTimer;
    int wiltingChance;

    // Wilting when you dont water the plant withing a certain time of planting it
    int plantTimer;
    int timeTillWilting = 5000; // 5 seconds

    // Wilting when you dont harvest the plant withing a ceraint time since it has grown
    int grownTimer;

    public int productionAmoutGiven;

    public bool hasBeenClicked;
    public Plant(string fileName, float x, float y, Pot pot, GameSettings settings) : base(fileName, 2, 4)
    {
        Setup(fileName, x, y);

        this.pot = pot;

        this.settings = settings;
    }

    void Setup(string fileName, float x, float y)
    {
        this.SetOrigin(width / 2, height);

        waterDrop = new Sprite("waterDrop.png");
        waterDrop.SetScaleXY(0.02f, 0.02f);
        waterDrop.SetXY(0, -20);

        AddChild(waterDrop);

        timerStart = Time.time;
        timeToGrow = GetTimeToGrow(fileName);
        wiltingTimer = GetWiltingCheckTime(fileName);
        wiltingChance = GetWiltingChance(fileName);

        this.SetXY(x, y);
        this.scale = 3;

        plantTimer = Time.time;
    }

    int GetTimeToGrow(string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(5000, 8000);
            case 2:
                return random.Next(3000, 5000);
            case 3:
                return random.Next(2500, 5000);
            case 4:
                return random.Next(5000, 9000);
            case 5:
                return random.Next(3000, 5000);
            default:
                return 0;
        }
    }

    int GetWiltingCheckTime (string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(5000, 8000);
            case 2:
                return random.Next(3000, 5000);
            case 3:
                return random.Next(2500, 5000);
            case 4:
                return random.Next(5000, 9000);
            case 5:
                return random.Next(3000, 5000);
            default:
                return 0;
        }
    }
    int GetWiltingChance (string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(25, 30);
            case 2:
                return random.Next(25, 40);
            case 3:
                return random.Next(32, 40);
            case 4:
                return random.Next(40, 45);
            case 5:
                return random.Next(50, 55);
            default:
                return 0;
        }
    }

    void Update()
    {
        Growing();

        HandleMouseInput ();
    }

    void Growing () 
    {
        if (isWatered)
        {
            if (Time.time - timerStart > timeToGrow && !wilting)
            {
                isGrown = true;
                growingStartTimer = Time.time;
                grownTimer = Time.time;
            }

            if (Time.time - animationChangeTimer > timeToGrow / 7 && !wilting)
            {
                frame++;
                animationChangeTimer = Time.time;

                if (frame < 7)
                {
                    SetCycle(frame, 1);
                }
                else
                {
                    SetCycle(6, 1);
                }
            }
        }
        else
        {
            SetCycle(0, 1);
        }
    }

    void WiltingWhileGrowing ()
    {
        // Can wilting while growing
        if (!isGrown && !wilting && Time.time - growingStartTimer > wiltingTimer)
        {

            int randomWiltingChance = random.Next(1, 101); // Random number between 1 and 100      


            Console.WriteLine(randomWiltingChance);
            Console.WriteLine(wiltingChance);

            if (randomWiltingChance <= wiltingChance)
            {
                wilting = true;
                Console.WriteLine(wilting);
            }

            growingStartTimer = Time.time;
        }
    }

    void WiltingIfNotWatered ()
    {
        if (!isWatered && Time.time - plantTimer > timeTillWilting && !wilting)
        {
            wilting = true;
        }
    }

    void WiltingIfNotHarvested ()
    {
        if (isGrown && !wilting && Time.time - grownTimer > timeTillWilting)
        {
            wilting = true;
        }
    }

    void HandleMouseInput ()
    {
        if (HitTestPoint(Input.mouseX, Input.mouseY))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!isWatered)
                {
                    waterDrop.LateDestroy();
                    isWatered = true;
                }

                if (isGrown)
                {
                    for (int i = 0; i < settings.customers.Count; i++)
                    {
                        foreach (String flower in settings.customers[i].flowersCollected)
                        {
                            string plantNameWithoutExtension = this.name.Split('.')[0];

                            if (plantNameWithoutExtension == flower)
                            {
                                settings.customers[i].flowersCollected.Remove (flower);

                                Console.WriteLine("Removed flower: " + flower);

                                break;
                            }
                            
                        }
                    }

                    this.LateDestroy();
                    pot.isSelected = false;

                    this.hasBeenClicked = true;
                }    
            }
        }
    }
}