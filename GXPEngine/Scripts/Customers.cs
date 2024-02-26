using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class Customers : AnimationSprite
{
    string[] flowers = { "flower1", "flower2", "flower3", "flower4", "flower5" };

    float lastChangeOfFace;
    float timer = 1000;
    int frame = 0;

    Random random = new Random();
    GameSettings settings;
    EasyDraw canvas;

    ScreenShake screenShake;

    public List<string> flowersCollected = new List<string>(); // Array to store collected flowers

    public Customers(GameSettings settings) : base(settings.people[new Random().Next(1, 5)], 5, 2)
    {
        
        this.settings = settings;

        SetUp();
    }

    void SetUp ()
    {
        this.SetXY(1150, 150);
        this.width = 200;
        this.height = 150;

        lastChangeOfFace = Time.time;
        random = new Random();
        SelectFlowers();

        canvas = new EasyDraw(300, 300, false);
        canvas.SetXY (this.x, this.y - 150);
        game.AddChild(canvas);
        DisplayFlowerCounts();

        foreach (string flower in flowersCollected)
        {
            Console.WriteLine(flower);
        }
    }

    void Update()
    {
        if (Time.time - lastChangeOfFace > timer) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
        {
            lastChangeOfFace = Time.time;
            frame++;
            SetCycle(frame, 1);
        }
        if (frame == 10)
        {

            screenShake = new ScreenShake();
            screenShake.ShakeScreen(1000f, 2f);
            AddChild(screenShake);

        }


    }

    void SelectFlowers()
    {
        for (int i = 0; i < flowers.Length; i++)
        {
            float flowerChance = CalculateFlowerChance(i);
            float randomNumber = random.Next(1, 101);

            if (randomNumber < flowerChance)
            {
                // Add the selected flower to the collected flowers array
                AddFlower(i);

                // Calculate the chance for additional flowers
                float additionalFlowerChance = CalculateAdditionalFlowerChance(i);

                // Check if there's a chance for additional flowers
                while (additionalFlowerChance > 0)
                {
                    randomNumber = random.Next(1, 101);

                    // If the random number is less than or equal to the additional flower chance, add more flowers
                    if (randomNumber <= additionalFlowerChance)
                    {
                        AddFlower(i);
                    }

                    // Decrease the additional flower chance by 10 (adjust as needed)
                    additionalFlowerChance -= 10;
                }
            }
        }
    }

    void AddFlower(int flowerIndex)
    {
        flowersCollected.Add(flowers[flowerIndex]);
    }

    // Still has to be worked on. Right now for level 1 it is a guarantee that you will get the first flower and for level 2 trough level 10 there should be an increase in the chances but i have to add that
    float CalculateFlowerChance(int flowerIndex)
    {
        if (settings.currentLevel == 1)
        {
            switch (flowerIndex)
            {
                case 0: // Corrected flower index
                    return 100;
                default:
                    return 0;
            }
        } else if (settings.currentLevel >= 2 && settings.currentLevel <= 10)
        {
            switch (flowerIndex)
            {
                case 0:
                    return 20;
                case 1:
                    return 80;
                case 2:
                    return 80;
                default:
                    return 0;
            }
        }

        return 0;
    }

    float CalculateAdditionalFlowerChance(int flowerIndex)
    {
        // Implement logic to calculate the chance for additional flowers based on level
        if (settings.currentLevel == 1)
        {
            // Start with a base chance (adjust as needed)
            float baseChance = 100;

            // Decrease the chance by 10 for each additional flower
            float additionalFlowerChance = baseChance - (10 * flowersCollected.Count);

            // Ensure the chance doesn't go below 0
            return Math.Max(additionalFlowerChance, 0);
        }
        else if (settings.currentLevel >= 2 && settings.currentLevel <= 10)
        {
            // Define base chances for each flower
            float[] baseChances = { 50, 40, 30, 20, 10 }; // Adjust these values as needed

            // Get the base chance for the current flower
            float baseChance = baseChances[flowerIndex];

            // Increase the base chance based on the current level
            baseChance += (settings.currentLevel - 1) * 5; // Adjust the increment value as needed

            // Get the count of the current flower type that has already been collected
            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

            // Decrease the chance by a certain amount for each additional flower of the same type
            float decreaseAmount = 10 * flowerTypeCount; // Adjust the decrease amount based on the count of the same type

            // Calculate the additional flower chance
            float additionalFlowerChance = baseChance - decreaseAmount;

            // Ensure the chance doesn't go below 0
            return Math.Max(additionalFlowerChance, 0);
        }


        return 0; // No additional flowers for level 1
    }

    void DisplayFlowerCounts()
    {
        // Get unique flowers from the flowersCollected list
        var uniqueFlowers = flowersCollected.Distinct();

        // Define initial y position for drawing text
        int x = 0;

        // Iterate over each unique flower
        foreach (var flowerName in uniqueFlowers)
        {
            // Count occurrences of the current flower in the flowersCollected list
            int flowerCount = flowersCollected.Count(flower => flower == flowerName);

            // Draw flower image
            // Assuming you have a method to load and draw flower images
            // Replace DrawFlowerImage method with your actual implementation
            DrawFlowerImage(flowerName, x, 20); // Adjust position as needed

            // Draw flower count next to the image
            canvas.Fill(Color.White); // Set text color
            canvas.TextFont("Arial", 14); // Set font and size
            canvas.Text($"{flowerCount}", x, 20); // Adjust position as needed

            // Increment y position for the next flower
            x += 40; // Adjust spacing between flower counts
        }
    }

    void DrawFlowerImage(string flowerName, float x, float y)
    {
        Sprite flowerImage = new Sprite(flowerName + ".png");
        flowerImage.SetXY (x, y); 
        canvas.AddChild (flowerImage);
    }
}
