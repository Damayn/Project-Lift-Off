using GXPEngine;
using System;

// Start button
public class StartButton : Button
{
    GameSettings settings;

    Sound press;

    public StartButton(GameSettings settings) : base("start.png", 2, 1)
    {

        this.settings = settings;

        press = new Sound("button_press.mp3", false, false);

    }
<<<<<<< Updated upstream
=======

    protected override void Update()
    {
        if (hasBeenPressed)
        {

            press.Play();

            settings.hasGameStarted = true;
            settings.isGameOver = false;
        }

        base.Update();
    }
>>>>>>> Stashed changes
}