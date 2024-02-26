using GXPEngine;
using System;
using System.Runtime;

public class MenuManager : GameObject
{
    // Settings reference
    GameSettings settings;

    public MenuManager(GameSettings settings) : base()
    {

        this.settings = settings;

    }

    public void SetCurrentMenu(GameObject menu)
    {
        // Adds the current menu
        game.AddChild(menu);

    }

    public void SetMainMenu()
    {
        // Sets the current menu to the main menu
        SetCurrentMenu(new MainMenu(this, settings));

        // Deletes the options menu
        foreach (GameObject child in game.GetChildren())
        {

            if (child is OptionsMenu)
            {

                child.LateDestroy();

            }

        }

    }

    public void SetOptionsMenu()
    {
        // Sets the current menu to options menu
        SetCurrentMenu(new OptionsMenu(this));

        // Deletes the main menu
        foreach (GameObject child in game.GetChildren())
        {

            if (child is MainMenu)
            {

                child.LateDestroy();

            }

        }

    }

    public void SetGameOverMenu()
    {
        // If the game is over
        if (settings.isGameOver)
        {

            SetCurrentMenu(new GameOver(this, settings));

        }
    }
}
