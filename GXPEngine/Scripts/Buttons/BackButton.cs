// Back button
using GXPEngine;

public class BackButton : Button
{
    MenuManager menuManager;
    GameSettings settings;

    public bool changeScene;

    public BackButton(MenuManager menuManager, GameSettings settings) : base("back.png", 2, 1)
    {
        this.settings = settings;
        this.menuManager = menuManager;
    }

    protected override void Update()
    {

        if (hasBeenPressed)
        {
            //menuManager.SetMainMenu();
            //LateDestroy();

            //foreach (GameObject child in game.GetChildren())
            //{
            //    if (child is Pause || child is NameMenu)
            //    {
            //        child.LateDestroy();
            //    }
            //}


            foreach (GameObject child in game.GetChildren())
            {
                if (child is Pause)
                {
                    child.LateDestroy();
                    settings.isTimePaused = false;
                } else if (child is NameMenu) 
                {
                    child.LateDestroy();

                    menuManager.SetMainMenu();
                }
            }


        }

        base.Update();

    }
}
