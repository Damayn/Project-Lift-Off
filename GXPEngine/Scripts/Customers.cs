using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class Customers : AnimationSprite
{
    string[] flowers = { "flower1", "flower2", "flower3", "flower4", "flower5" };

    float lastChangeOfFace;
    float timer = 15000;
    int frame = 0;

    int flowerCount = 0;
    int currnetFlowerAmmout;
    int x2 = 0;

    Random random = new Random();
    GameSettings settings;
    EasyDraw canvas;
    ScreenShake screenShake;
    Slider productionSlider;
    Sprite customerBackground;

    Sound angry;
    Sound happy;
    Sound spawn;
    SoundChannel states;

    public List<string> flowersCollected = new List<string>(); // Array to store collected flowers

    public Customers(GameSettings settings, Slider productionSlider, Sprite customerBackground) : base(settings.people[0], 5, 1)
    {
        Console.WriteLine (GameSettings.currentLevel);
        this.settings = settings;
        this.customerBackground = customerBackground;

        ScreenShake screenShake = new ScreenShake();
        this.productionSlider = productionSlider;

        SetUp();
    }

    void SetUp ()
    {
        this.SetScaleXY(0.3f, 0.3f);
        this.SetXY(1150, customerBackground.y );
        //this.width = 200;
        //this.height = 150;
        
        canvas = new EasyDraw(300, 300, false);
        canvas.SetXY (customerBackground.x + 60, customerBackground.y + 110);
        game.AddChild(canvas);


        angry = new Sound("Customer_Negative_Feedback.mp3",false,false);
        happy = new Sound("Customer_Positive_Feedback.mp3", false, false);
        spawn = new Sound("New_Order.mp3", false, false);

        lastChangeOfFace = Time.time;
        random = new Random();
        SelectFlowers();

        settings.scream = false;

        foreach (string flower in flowersCollected)
        {
            Console.WriteLine(flower);
        }

        states = spawn.Play();
    }

    void Update()
    {
        if (Time.time - lastChangeOfFace > timer && !settings.isTimePaused) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
        {
            lastChangeOfFace = Time.time;
            frame++;
            SetCycle(frame, 1);

            if (frame > 4)
            {
                LateDestroy();
                canvas.LateDestroy();
                settings.customers.Clear();
                productionSlider.currentValue -= productionSlider.maximumValue / 4 * GameSettings.currentLevel;
            }

        }

        if (frame == 10)
        {

            //screenShake = new ScreenShake();
            //screenShake.ShakeScreen(1000f, 2f);
            //AddChild(screenShake);
            if (settings.scream == false)
            {
                states = angry.Play();

                settings.scream = true;
            }
            
        }
        if (flowersCollected.Count <= 0 && frame != 0)
        { 

            if (settings.scream == false)
            {

                states = happy.Play();

                settings.scream = true;

            }

            productionSlider.currentValue += GetProductionAmount();
            settings.points += GetProductionAmount();
            this.Destroy();
        }

        if (frame == 0 && flowersCollected.Count == 0)
        {
            SelectFlowers();
        }

        DisplayFlowerCounts();
    }

    int GetProductionAmount()
    {
        float productionAmount = 0;

        // Determine the base production amount based on the frame
        switch (frame)
        {
            case 0: // Adjust based on your frame logic
                productionAmount = productionSlider.maximumValue /2 ; // Example value for frame 0
                break;
            case 1: // Adjust based on your frame logic
                productionAmount = (int)productionSlider.maximumValue / 2.4f; // Example value for frame 1
                break;
            case 2:
                productionAmount = (int)productionSlider.maximumValue / 2.8f;
                break;
            case 3:
                productionAmount = (int)productionSlider.maximumValue / 2.8f;
                break;
            case 4:
                productionAmount = (int)productionSlider.maximumValue / 3.2f;
                break;
            // Add more cases as needed for different frames
            default:
                // Default production amount
                productionAmount = 0;
                break;
        }

        //// Iterate over each unique flower in the flowersCollected list
        //foreach (var uniqueFlower in flowersCollected.Distinct())
        //{
        //    // Get the count of the current flower type in the flowersCollected list
        //    int flowerCount = flowersCollected.Count(flower => flower == uniqueFlower);

        //    // Determine the value for the current flower type based on its index and count
        //    int flowerValue = CalculateFlowerValue(uniqueFlower, flowerCount);

        //    // Add the value of the current flower type to the production amount
        //    productionAmount += flowerValue;
        //}

        return (int)productionAmount;
    }

    // Method to calculate the value of each flower type based on its index and count
    int CalculateFlowerValue(string flowerType, int flowerCount)
    {
        int baseValue = 0;

        // Determine the base value for each flower type
        switch (flowerType)
        {
            case "flower1":
                baseValue = 20;
                break;
            case "flower2":
                baseValue = 30;
                break;
            case "flower3":
                baseValue = 35;
                break;
            case "flower4":
                baseValue = 45;
                break;
            case "flower5":
                baseValue = 50;
                break;
            default:
                // Default base value
                baseValue = 0;
                break;
        }

        // Calculate the value for each flower based on its count
        int flowerValue = baseValue * flowerCount - (int)Math.Pow(0.5, flowerCount); // Adjust multiplier as needed

        return flowerValue;
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
        if (GameSettings.currentLevel == 1)
        {
            Console.WriteLine("kill me"); 
            switch (flowerIndex)
            {
                case 0: // Corrected flower index
                    return 100;
                case 1: // Corrected flower index
                    return 100;
                case 2: // Corrected flower index
                    return 100;
                case 3: // Corrected flower index
                    return 100;
                default:
                    return 0;
            }
        } else if (GameSettings.currentLevel >= 2 && GameSettings.currentLevel <= 10)
        {
             Console.WriteLine("kill me");
            switch (flowerIndex)
            {
                
                case 1:
                    return 10 + 3 * GameSettings.currentLevel;
                case 2:
                    return 5 + 3 * GameSettings.currentLevel;
                case 3: 
                    return 3 * GameSettings.currentLevel;
                case 4:
                    return GameSettings.currentLevel;
                default:
                    return 0;
            }
        }
        else if (GameSettings.currentLevel >= 11 && GameSettings.currentLevel <= 20)
        {
            switch (flowerIndex)
            {
                case 0:
                    return 40 + (GameSettings.currentLevel - 10);
                case 1:
                    return 50 + 2 * (GameSettings.currentLevel - 5);
                case 2:
                    return 65 + 2 * (GameSettings.currentLevel - 5);
                case 3:
                    return 70 + 2 * (GameSettings.currentLevel - 5);
                case 4:
                    return 10 + 2 * (GameSettings.currentLevel - 5);
                default:
                    return 0;
            }
        }
        else if (GameSettings.currentLevel >= 21 && GameSettings.currentLevel <= 30)
        {
            switch (flowerIndex)
            {
                case 0:
                    return 10 + 5 * (GameSettings.currentLevel - 15);
                case 1:
                    return 40 + 5 * (GameSettings.currentLevel - 15);
                case 2:
                    return 60 + 5 * (GameSettings.currentLevel - 15);
                case 3:
                    return 55 + 5 * (GameSettings.currentLevel - 15);
                case 4:
                    return 50 + 5 * (GameSettings.currentLevel - 15);
                default:
                    return 0;
            }
        }
        return 0;
    }

    float CalculateAdditionalFlowerChance(int flowerIndex)
    {
        // Implement logic to calculate the chance for additional flowers based on level
        if (GameSettings.currentLevel == 1)
        {
            // Start with a base chance (adjust as needed)
            float baseChance = 60;

            // Decrease the chance by 10 for each additional flower
            float additionalFlowerChance = baseChance - (10 * flowersCollected.Count);

            // Ensure the chance doesn't go below 0
            return Math.Max(additionalFlowerChance, 0);
        }
        else if (GameSettings.currentLevel >= 2 && GameSettings.currentLevel <= 10)
        {
            // Define base chances for each flower
            float[] baseChances = { 50, 40, 30, 20, 10 };

            // Get the base chance for the current flower
            float baseChance = baseChances[flowerIndex];

            // Increase the base chance based on the current level
            baseChance += (GameSettings.currentLevel - 1) * 5;

            // Get the count of the current flower type that has already been collected
            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

            // Decrease the chance by a certain amount for each additional flower of the same type
            float decreaseAmount = 10 * flowerTypeCount;

            // Calculate the additional flower chance
            float additionalFlowerChance = baseChance - decreaseAmount;

            // Ensure the chance doesn't go below 0
            return Math.Max(additionalFlowerChance, 0);
        }
        else if (GameSettings.currentLevel >= 11 && GameSettings.currentLevel <= 20)
        {
            // Define base chances for each flower
            float[] baseChances = { 30, 40, 50, 30, 15 };

            // Get the base chance for the current flower
            float baseChance = baseChances[flowerIndex];

            // Increase the base chance based on the current level
            baseChance += GameSettings.currentLevel;

            // Get the count of the current flower type that has already been collected
            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

            // Decrease the chance by a certain amount for each additional flower of the same type
            float decreaseAmount = 10 * flowerTypeCount;

            // Calculate the additional flower chance
            float additionalFlowerChance = baseChance - decreaseAmount;

            // Ensure the chance doesn't go below 0
            return Math.Max(additionalFlowerChance, 0);
        }
        else if (GameSettings.currentLevel >= 21 && GameSettings.currentLevel <= 30)
        {
            // Define base chances for each flower
            float[] baseChances = { 40, 40, 50, 60, 60 };

            // Get the base chance for the current flower
            float baseChance = baseChances[flowerIndex];

            // Increase the base chance based on the current level
            baseChance += (GameSettings.currentLevel - 21) * 5;

            // Get the count of the current flower type that has already been collected
            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

            // Decrease the chance by a certain amount for each additional flower of the same type
            float decreaseAmount = 10 * flowerTypeCount;

            // Calculate the additional flower chance
            float additionalFlowerChance = baseChance - decreaseAmount;

            // Ensure the chance doesn't go below 0
            return Math.Max(additionalFlowerChance, 0);
        }

        return 0; // No additional flowers for levels beyond 30
    }

    void DisplayFlowerCounts()
    {
        canvas.ClearTransparent();
        // Get unique flowers from the flowersCollected list
        var uniqueFlowers = flowersCollected.Distinct();

        // Define initial y position for drawing text
        int x = 0;
        y = 35;

        // Iterate over each unique flower
        foreach (var flowerName in uniqueFlowers)
        {
            // Count occurrences of the current flower in the flowersCollected list
            flowerCount = flowersCollected.Count(flower => flower == flowerName);

            // Draw flower image
            // Assuming you have a method to load and draw flower images
            // Replace DrawFlowerImage method with your actual implementation
            DrawFlowerImage(flowerName, x + 60, y - 15); // Adjust position as needed

            // Draw flower count next to the image
            canvas.Fill(Color.White); // Set text color
            canvas.TextFont("Helvetika", 18); // Set font and size
            canvas.Text($"{flowerCount} x", x, y); // Adjust position as needed

            // Increment y position for the next flower
            y += 50; // Adjust spacing between flower counts
        }
    }

    void DrawFlowerImage(string flowerName, float x, float y)
    {
        Sprite flowerImage = new Sprite(flowerName + "Harvestable.png");
        flowerImage.SetXY(x, y);
        flowerImage.SetOrigin(flowerImage.width / 2, flowerImage.height / 2);
        flowerImage.SetScaleXY(0.045f);
        canvas.AddChild(flowerImage);
    }
}



//=======
//﻿using GXPEngine;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;

//public class Customers : AnimationSprite
//{
//    string[] flowers = { "flower1", "flower2", "flower3", "flower4", "flower5" };

//    float lastChangeOfFace;
//    float timer = 15000;
//    int frame = 0;

//    int flowerCount = 0;
//    int currnetFlowerAmmout;
//    int x2 = 0;

//    Random random = new Random();
//    GameSettings settings;
//    EasyDraw canvas;
//    ScreenShake screenShake;
//    Slider productionSlider;

//    public List<string> flowersCollected = new List<string>(); // Array to store collected flowers

//    public Customers(GameSettings settings, Slider productionSlider) : base(settings.people[new Random().Next(1, 5)], 5, 2)
//    {
        
//        this.settings = settings;

//        ScreenShake screenShake = new ScreenShake();
//        this.productionSlider = productionSlider;

//        SetUp();
//    }

//    void SetUp ()
//    {
//        this.SetXY(1150, 150);
//        this.width = 200;
//        this.height = 150;

//        lastChangeOfFace = Time.time;
//        random = new Random();
//        SelectFlowers();

//        canvas = new EasyDraw(300, 300, false);
//        canvas.SetXY (this.x, this.y - 150);
//        game.AddChild(canvas);
        

//        foreach (string flower in flowersCollected)
//        {
//            Console.WriteLine(flower);
//        }
//    }

//    void Update()
//    {
//        if (Time.time - lastChangeOfFace > timer && !settings.isTimePaused) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
//        {
//            lastChangeOfFace = Time.time;
//            frame++;
//            SetCycle(frame, 1);

//            if (frame > 11)
//            {
//                LateDestroy();
//                productionSlider.currentValue -= 20 * settings.currentLevel;
//            }

//        }

//        if (frame == 10)
//        {

//            //screenShake = new ScreenShake();
//            //screenShake.ShakeScreen(1000f, 2f);
//            //AddChild(screenShake);

//        }
//        if (flowersCollected.Count <= 0 && frame != 0)
//        {
//            productionSlider.currentValue += GetProductionAmount();
//            settings.points += GetProductionAmount();
//            this.Destroy();
//        }

//        if (frame == 0 && flowersCollected.Count == 0)
//        {
//            SelectFlowers();
//        }

//        DisplayFlowerCounts();
//    }

//    int GetProductionAmount()
//    {
//        int productionAmount = 0;

//        // Determine the base production amount based on the frame
//        switch (frame)
//        {
//            case 0: // Adjust based on your frame logic
//                productionAmount = 5; // Example value for frame 0
//                break;
//            case 1: // Adjust based on your frame logic
//                productionAmount = 10; // Example value for frame 1
//                break;
//            // Add more cases as needed for different frames
//            default:
//                // Default production amount
//                productionAmount = 0;
//                break;
//        }

//        // Iterate over each unique flower in the flowersCollected list
//        foreach (var uniqueFlower in flowersCollected.Distinct())
//        {
//            // Get the count of the current flower type in the flowersCollected list
//            int flowerCount = flowersCollected.Count(flower => flower == uniqueFlower);

//            // Determine the value for the current flower type based on its index and count
//            int flowerValue = CalculateFlowerValue(uniqueFlower, flowerCount);

//            // Add the value of the current flower type to the production amount
//            productionAmount += flowerValue;
//        }

//        return productionAmount;
//    }

//    // Method to calculate the value of each flower type based on its index and count
//    int CalculateFlowerValue(string flowerType, int flowerCount)
//    {
//        int baseValue = 0;

//        // Determine the base value for each flower type
//        switch (flowerType)
//        {
//            case "flower1":
//                baseValue = 20;
//                break;
//            case "flower2":
//                baseValue = 30;
//                break;
//            case "flower3":
//                baseValue = 35;
//                break;
//            case "flower4":
//                baseValue = 45;
//                break;
//            case "flower5":
//                baseValue = 50;
//                break;
//            default:
//                // Default base value
//                baseValue = 0;
//                break;
//        }

//        // Calculate the value for each flower based on its count
//        int flowerValue = baseValue * flowerCount - (int)Math.Pow(0.5, flowerCount); // Adjust multiplier as needed

//        return flowerValue;
//    }

//    void SelectFlowers()
//    {
//        for (int i = 0; i < flowers.Length; i++)
//        {
//            float flowerChance = CalculateFlowerChance(i);
//            float randomNumber = random.Next(1, 101);

//            if (randomNumber < flowerChance)
//            {
//                // Add the selected flower to the collected flowers array
//                AddFlower(i);

//                // Calculate the chance for additional flowers
//                float additionalFlowerChance = CalculateAdditionalFlowerChance(i);

//                // Check if there's a chance for additional flowers
//                while (additionalFlowerChance > 0)
//                {
//                    randomNumber = random.Next(1, 101);

//                    // If the random number is less than or equal to the additional flower chance, add more flowers
//                    if (randomNumber <= additionalFlowerChance)
//                    {
//                        AddFlower(i);
//                    }

//                    // Decrease the additional flower chance by 10 (adjust as needed)
//                    additionalFlowerChance -= 10;
//                }
//            }
//        }
//    }

//    void AddFlower(int flowerIndex)
//    {
//        flowersCollected.Add(flowers[flowerIndex]);
//    }

//    // Still has to be worked on. Right now for level 1 it is a guarantee that you will get the first flower and for level 2 trough level 10 there should be an increase in the chances but i have to add that
//    float CalculateFlowerChance(int flowerIndex)
//    {
//        if (settings.currentLevel == 1)
//        {
//            switch (flowerIndex)
//            {
//                case 0: // Corrected flower index
//                    return 100;
//                default:
//                    return 0;
//            }
//        } else if (settings.currentLevel >= 2 && settings.currentLevel <= 10)
//        {
//            switch (flowerIndex)
//            {
//                case 0:
//                    return 80;
//                case 1:
//                    return 40 + 3 * settings.currentLevel;
//                case 2:
//                    return 0 + 3 * settings.currentLevel;
//                default:
//                    return 0;
//            }
//        }
//        else if (settings.currentLevel >= 11 && settings.currentLevel <= 20)
//        {
//            switch (flowerIndex)
//            {
//                case 0:
//                    return 40 + (settings.currentLevel - 10);
//                case 1:
//                    return 80 + 2 * (settings.currentLevel - 5);
//                case 2:
//                    return 80 + 2 * (settings.currentLevel - 5);
//                case 3:
//                    return 50 + 2 * (settings.currentLevel - 5);
//                case 4:
//                    return 50 + 2 * (settings.currentLevel - 5);
//                default:
//                    return 0;
//            }
//        }
//        else if (settings.currentLevel >= 21 && settings.currentLevel <= 30)
//        {
//            switch (flowerIndex)
//            {
//                case 0:
//                    return 10 + 5 * (settings.currentLevel - 15);
//                case 1:
//                    return 45 + 5 * (settings.currentLevel - 15);
//                case 2:
//                    return 60 + 5 * (settings.currentLevel - 15);
//                case 3:
//                    return 70 + 5 * (settings.currentLevel - 15);
//                case 4:
//                    return 70 + 5 * (settings.currentLevel - 15);
//                default:
//                    return 0;
//            }
//        }
//        return 0;
//    }

//    float CalculateAdditionalFlowerChance(int flowerIndex)
//    {
//        // Implement logic to calculate the chance for additional flowers based on level
//        if (settings.currentLevel == 1)
//        {
//            // Start with a base chance (adjust as needed)
//            float baseChance = 100;

//            // Decrease the chance by 10 for each additional flower
//            float additionalFlowerChance = baseChance - (10 * flowersCollected.Count);

//            // Ensure the chance doesn't go below 0
//            return Math.Max(additionalFlowerChance, 0);
//        }
//        else if (settings.currentLevel >= 2 && settings.currentLevel <= 10)
//        {
//            // Define base chances for each flower
//            float[] baseChances = { 50, 40, 30, 20, 10 };

//            // Get the base chance for the current flower
//            float baseChance = baseChances[flowerIndex];

//            // Increase the base chance based on the current level
//            baseChance += (settings.currentLevel - 1) * 5;

//            // Get the count of the current flower type that has already been collected
//            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

//            // Decrease the chance by a certain amount for each additional flower of the same type
//            float decreaseAmount = 10 * flowerTypeCount;

//            // Calculate the additional flower chance
//            float additionalFlowerChance = baseChance - decreaseAmount;

//            // Ensure the chance doesn't go below 0
//            return Math.Max(additionalFlowerChance, 0);
//        }
//        else if (settings.currentLevel >= 11 && settings.currentLevel <= 20)
//        {
//            // Define base chances for each flower
//            float[] baseChances = { 40, 40, 65, 30, 15 };

//            // Get the base chance for the current flower
//            float baseChance = baseChances[flowerIndex];

//            // Increase the base chance based on the current level
//            baseChance += (settings.currentLevel - 11) * 5;

//            // Get the count of the current flower type that has already been collected
//            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

//            // Decrease the chance by a certain amount for each additional flower of the same type
//            float decreaseAmount = 10 * flowerTypeCount;

//            // Calculate the additional flower chance
//            float additionalFlowerChance = baseChance - decreaseAmount;

//            // Ensure the chance doesn't go below 0
//            return Math.Max(additionalFlowerChance, 0);
//        }
//        else if (settings.currentLevel >= 21 && settings.currentLevel <= 30)
//        {
//            // Define base chances for each flower
//            float[] baseChances = { 40, 40, 50, 60, 60 };

//            // Get the base chance for the current flower
//            float baseChance = baseChances[flowerIndex];

//            // Increase the base chance based on the current level
//            baseChance += (settings.currentLevel - 21) * 5;

//            // Get the count of the current flower type that has already been collected
//            int flowerTypeCount = flowersCollected.Count(flower => flower == flowers[flowerIndex]);

//            // Decrease the chance by a certain amount for each additional flower of the same type
//            float decreaseAmount = 10 * flowerTypeCount;

//            // Calculate the additional flower chance
//            float additionalFlowerChance = baseChance - decreaseAmount;

//            // Ensure the chance doesn't go below 0
//            return Math.Max(additionalFlowerChance, 0);
//        }

//        return 0; // No additional flowers for levels beyond 30
//    }

//    void DisplayFlowerCounts()
//    {
//        canvas.ClearTransparent();
//        // Get unique flowers from the flowersCollected list
//        var uniqueFlowers = flowersCollected.Distinct();

//        // Define initial y position for drawing text
//        int x = 10;
//        y = 40;

//        // Iterate over each unique flower
//        foreach (var flowerName in uniqueFlowers)
//        {
//            // Count occurrences of the current flower in the flowersCollected list
//            flowerCount = flowersCollected.Count(flower => flower == flowerName);

//            // Draw flower image
//            // Assuming you have a method to load and draw flower images
//            // Replace DrawFlowerImage method with your actual implementation
//            DrawFlowerImage(flowerName, x + 60, y - 15); // Adjust position as needed

//            // Draw flower count next to the image
//            canvas.Fill(Color.White); // Set text color
//            canvas.TextFont("Helvetika", 18); // Set font and size
//            canvas.Text($"{flowerCount} x", x, y); // Adjust position as needed

//            // Increment y position for the next flower
//            y += 60; // Adjust spacing between flower counts
//        }
//    }

//    void DrawFlowerImage(string flowerName, float x, float y)
//    {
//        Sprite flowerImage = new Sprite(flowerName + "Harvestable.png");
//        flowerImage.SetXY (x, y);
//        flowerImage.SetOrigin (flowerImage.width /2, flowerImage.height /2);
//        flowerImage.SetScaleXY(0.045f);
//        canvas.AddChild (flowerImage);
//    }
//}
//>>>>>>> Stashed changes
