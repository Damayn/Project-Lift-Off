﻿using GXPEngine;
using System;
using System.Drawing;

public class Customers : AnimationSprite
{
    //the time it takes to change frames variables
    int frame;

    float lastChangeOfFace;
    float timer = 1000;

    Random random = new Random(); 

    GameSettings settings;

    public Customers(GameSettings settings) : base(settings.people[new Random().Next(1, 5)], 5, 2)
    {
        lastChangeOfFace = Time.time;

        this.settings = settings;

        random = new Random();

        //SetRandomImageFilename();
        
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
}