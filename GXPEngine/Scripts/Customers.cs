using GXPEngine;
using System;

public class Customers : AnimationSprite
{
    //the time it takes to change frames variables
    int counter;
    int frame;

    public Customers() : base("Faces.png", 5, 2)
    {

    }

    void Update()
    {

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
