using GXPEngine;
using System;
using System.Runtime;

public class MenuManager : GameObject
{
    // Settings reference
    GameSettings settings;
    MyGame gameRef;
<<<<<<< Updated upstream:GXPEngine/Scripts/Managers/MenuManager.cs
    Sprite background;
    ScoreManager scoreManager;

    public MenuManager(GameSettings settings,MyGame gamRef, ScoreManager scoreManager) : base()
=======

    public MenuManager(GameSettings settings,MyGame gamRef) : base()
>>>>>>> Stashed changes:GXPEngine/Scripts/Menus/MenuManager.cs
    {
        this.settings = settings;
        this.gameRef = gamRef;
        this.scoreManager = scoreManager;
    }

    public void SetCurrentMenu(GameObject menu)
    {
        // Adds the current menu
        game.AddChild(menu);

    }

    public void SetMainMenu()
    {

        // Sets the current menu to the main menu
        SetCurrentMenu(new MainMenu(settings, this));

        // Deletes the options menu
        foreach (GameObject child in game.GetChildren())
        {

            if (child is OptionsMenu || child is GameOver)
            {

                child.LateDestroy();

            }

            foreach (GameObject gameObject in game.GetChildren())
            {
                if (gameObject is Pot || gameObject is Slider || gameObject is Pause || gameObject is Customers || gameObject is Seed || gameObject is EasyDraw)
                {

                    gameObject.LateDestroy();

                    settings.customers.Clear();
                }

            }

        }

        settings.ResetSettings();
    }

    public void SetOptionsMenu()
    {
        // Sets the current menu to options menu
        SetCurrentMenu(new OptionsMenu(this, settings));

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
        SetCurrentMenu(new NameMenu(settings, this, gameRef));

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
        SetCurrentMenu(new GameOver(this, settings, scoreManager));

        foreach (GameObject gameObject in game.GetChildren())
        {
            if (gameObject is Pot || gameObject is Slider || gameObject is Pause || gameObject is Customers || gameObject is Seed || gameObject is EasyDraw)
            {
                gameObject.LateDestroy();
                settings.customers.Clear();
            }

        }
    }
}
