using GXPEngine;
using System;

// Start button
public class StartButton : Button
{
    GameSettings settings;

    public StartButton(GameSettings settings) : base("start.png", 2, 1)
    {

        this.settings = settings;

    }
}