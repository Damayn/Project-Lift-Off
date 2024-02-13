using GXPEngine;
using System;

// Start button
public class StartButton : Button
{
    GameSettings settings;
    public StartButton(GameSettings settings) : base("PlayButton.png", 2, 1)
    {
        this.settings = settings;
    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {
            settings.hasGameStarted = true;
            settings.isGameOver = false;
        }

        base.Update();
    }
}