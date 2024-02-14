using GXPEngine;
using System;

class Plant : AnimationSprite
{
    Sprite waterDrop;

    int timerStart;
    int timeToGrow;

    bool isWatered = false;
    bool isGrown = false;
    bool isWithering = false;

    Random random = new Random();

    public Plant(string fileName) : base(fileName, 2, 5)
    {
        waterDrop = new Sprite("waterDrop.png");
        waterDrop.SetScaleXY(0.02f, 0.02f);
        waterDrop.SetXY(0, -20);

        AddChild(waterDrop);

        timerStart = Time.time;
        timeToGrow = GetTimeToGrow(fileName);

        this.SetScaleXY(50, 50);
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

    void Update()
    {
        if (Time.time - timerStart > timeToGrow)
        {
            if (isWatered)
            {
                isGrown = true;
            }
            else
            {
                isWithering = true;
            }
        }

        if (isGrown)
        {
            SetCycle(3, 1);
        }
        else
        {
            SetCycle(0, 1);
        }

        if (isWithering) 
        {
            
        }

        if (HitTestPoint(Input.mouseX, Input.mouseY))
        {
            if (Input.GetMouseButtonDown(0))
            {
                waterDrop.LateDestroy();
                isWatered = true;
            }
        }
    }

}