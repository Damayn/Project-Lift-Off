using GXPEngine;
using System;

public class Player : AnimationSprite
{

    int counter;
    int frame;

    private float countdownTimer;
    private int countdownValue;


    public Player() : base("Faces.png", 5, 2)
    {

        countdownTimer = 60f; // Initial countdown time in seconds
        countdownValue = (int)countdownTimer;

    }

    void Update()
    {
        counter++;

        /* countdownTimer -= Time.deltaTime;

         // Update the countdown value (rounded down)
         countdownValue = (int)Math.Floor(countdownTimer);
        */
        if (counter > 200)
        {

            counter = 0;

            frame++;

            if (frame == frameCount)
            {

                frame = 0;
            }

            SetFrame(frame);

        }

    }

}
