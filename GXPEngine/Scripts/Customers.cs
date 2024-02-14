using GXPEngine;
using System;

public class Customers : AnimationSprite
{
    //the time it takes to change frames variables
    int counter;
    int frame;

    public Customers() : base("Faces.png", 5, 2)
    {
        // If you use the method in update I told you about, here you should put
        //lastChangeOfFace = Time.time
    }

    void Update()
    {
        // This wont rly work that good since update isnt always called the same amout of times in a second. It is based on fps. For a timer is better to do smth like:
        
        //if (Time.time - lastChangeOfFace > timer) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
        //{
               // lastCahngeOfFace = Time.time
        //    // frame ++
        //    // SetCycle (frame, 1) // Or SetFrame (frame)
        //}

        //counter increase speed
        counter++;
        //change of sprite
        if (counter > 200)
        {

            counter = 0;

            frame++;
            //restart
            if (frame == frameCount)
            {

                frame = 0;
            }

            SetFrame(frame);

        }

    }

}
