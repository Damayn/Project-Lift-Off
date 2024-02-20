using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Customers : AnimationSprite
{
    string[] flowers = { "flower1", "flower2", "flower3", "flower4", "flower5" };

    float lastChangeOfFace;
    float timer = 1000;
    int frame = 0;

    Random random = new Random();

    GameSettings settings;

    List<string> flowersCollected = new List<string>(); // Array to store collected flowers

    public Customers(GameSettings settings) : base(settings.people[new Random().Next(1, 5)], 5, 2)
    {
        lastChangeOfFace = Time.time;
        this.settings = settings;
        random = new Random();
        SelectFlowers();
    }

    void Update()
    {
        if (Time.time - lastChangeOfFace > timer) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
        {
            lastChangeOfFace = Time.time;
            frame++;
            SetCycle(frame, 1);
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
        while (flowerIndex >= flowersCollected.Count)
        {
            flowersCollected.Add(null); // Extend the list to accommodate the new flower
        }
        flowersCollected[flowerIndex] = flowers[flowerIndex]; // Add the selected flower to the collected flowers array
        Console.WriteLine(flowersCollected[flowerIndex]);
    }

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
}
