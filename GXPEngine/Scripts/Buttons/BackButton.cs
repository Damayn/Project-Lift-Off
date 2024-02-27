// Back button
using GXPEngine;

public class BackButton : Button
{
    MenuManager menuManager;

    public bool changeScene;

    public BackButton(MenuManager menuManager) : base("BackButton.png", 2, 1)
    {
        this.menuManager = menuManager;
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
