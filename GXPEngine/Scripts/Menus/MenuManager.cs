using GXPEngine;
using System;
using System.Runtime;

public class MenuManager : GameObject
{
    // Settings reference
    GameSettings settings;
    MyGame gameRef;

    public MenuManager(GameSettings settings,MyGame gamRef) : base()
    {

        this.settings = settings;
        this.gameRef = gamRef;

    }

    public void SetCurrentMenu(GameObject menu)
    {
        // Adds the current menu
        game.AddChild(menu);

    }

    public void SetMainMenu()
    {
        // Sets the current menu to the main menu
        SetCurrentMenu(new MainMenu(this, settings, gameRef));

        // Deletes the options menu
        foreach (GameObject child in game.GetChildren())
        {

            if (child is OptionsMenu)
            {

                child.LateDestroy();

            }

            foreach (GameObject gameObject in game.GetChildren())
            {
                if (gameObject is Pot || gameObject is Slider || gameObject is Pause || gameObject is Customers || gameObject is Seed || gameObject is Sprite)
                {

                    gameObject.LateDestroy();

                    if (gameObject is Customers) 
                    {
                        settings.customers.Remove((Customers)gameObject);
                    }
                }

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

    public void SetNameMenu () 
    {
        SetCurrentMenu(new NameMenu(settings));

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
