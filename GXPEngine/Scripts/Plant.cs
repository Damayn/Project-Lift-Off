using GXPEngine;
using System;

class Plant : AnimationSprite
{
    Sprite waterDrop;
    Pot pot;

    int timerStart;
    int timeToGrow;
    int animationChangeTimer;

    int frame;

    bool isWatered = false;

    bool timerHasBeenUpdate = false;

    bool isGrown = false;
    bool wilting = false;

    Random random = new Random();

    int witherTimer;
    int timer;
    int witherChance;

    bool hasWitherChance;

    public int productionAmoutGiven;

    public Plant(string fileName, float x, float y, Pot pot) : base(fileName, 2, 4)
    {
        this.SetOrigin(width / 2, height);

        waterDrop = new Sprite("waterDrop.png");
        waterDrop.SetScaleXY(0.02f, 0.02f);
        waterDrop.SetXY(0, -20);

        AddChild(waterDrop);


        timeToGrow = GetTimeToGrow(fileName);

        witherTimer = GetWitherCheckTime(fileName);
        witherChance = GetWitherChance(fileName);

        this.SetXY(x, y);
        this.SetScaleXY(3);
        this.pot = pot;
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
                return random.Next(6000, 10000);
            case 3:
                return random.Next(7500, 10000);
            case 4:
                return random.Next(5000, 9000);
            case 5:
                return random.Next(8000, 12000);
            default:
                return 0;
        }
    }

    int GetWitherCheckTime(string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(5000, 8000);
            case 2:
                return random.Next(6000, 10000);
            case 3:
                return random.Next(7500, 10000);
            case 4:
                return random.Next(5000, 9000);
            case 5:
                return random.Next(8000, 12000);
            default:
                return 0;
        }
    }

    int GetWitherChance (string fileName)
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
        if (isWatered)
        {
            if (!timerHasBeenUpdate)
            {
                timerStart = Time.time;
                animationChangeTimer = Time.time;

                timerHasBeenUpdate = true;
            }
            if (Time.time - timerStart > timeToGrow && !wilting)
            {
                isGrown = true;
                timer = Time.time;
            }

            if (Time.time - animationChangeTimer > timeToGrow / 7 && !wilting)
            {
                frame++;
                animationChangeTimer = Time.time;

                if (frame < 7)
                {
                    SetCycle(frame, 1);
                } else
                {
                    SetCycle(6, 1);
                }
            }
        } else
        {
            SetCycle(0, 1);
        }

        if (!isGrown && !wilting && Time.time - timer > witherTimer) 
        {
        
            int randomWitherChance = random.Next(1, 101); // Random number between 1 and 100      
            

            Console.WriteLine(randomWitherChance);
            Console.WriteLine(witherChance);

            if (randomWitherChance <= witherChance)
            {
                wilting = true;
                Console.WriteLine(wilting);
            }

            timer = Time.time;
        }

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
                    this.LateDestroy();
                    pot.isSelected = false;
                }
            }
        }
    }

}