// Back button
using GXPEngine;

public class BackButton : Button
{
    MenuManager menuManager;

    public bool changeScene;

    string button;

    public BackButton(MenuManager menuManager, string button) : base(button, 2, 1)
    {

        this.menuManager = menuManager;
        this.button = button;

    }

    protected override void Update()
    {

        if (hasBeenPressed)
        {

            menuManager.SetMainMenu();
            LateDestroy();

            foreach (GameObject child in game.GetChildren())
            {
                if (child is Pause)
                {
                    menuManager.SetMainMenu();
                }
            }

        }

        base.Update();

    }
}
