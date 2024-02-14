using GXPEngine;
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

    public Customers(GameSettings settings) : base(settings.people[new Random().Next (1, 5)], 5, 2)
    {
        lastChangeOfFace = Time.time;

        this.settings = settings;

        random = new Random();

        x = 1150;
        y = 150;

        width = 200;
        height = 150;

        //SetRandomImageFilename();

    }

    void Update()
    {
        // This wont rly work that good since update isnt always called the same amout of times in a second. It is based on fps. For a timer is better to do smth like:

        if (Time.time - lastChangeOfFace > timer) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
        {
            lastChangeOfFace = Time.time;
            frame++;
            SetCycle(frame, 1); // Or SetFrame (frame)

        }

    }

    //void SetRandomImageFilename()
    //{
    //    int stringIndex = random.Next(0, settings.people.Length);
    //    filename = people[stringIndex];
    //}

}
