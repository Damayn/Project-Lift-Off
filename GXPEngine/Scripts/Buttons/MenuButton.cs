using GXPEngine;

class MenuButton : Button
{
    GameSettings settings;
    MenuManager menuManager;

    Sound press;

    public MenuButton (GameSettings settings, MenuManager menuManager) : base ("menu.png", 2, 1)
    {
        this.settings = settings;
        this.menuManager = menuManager;

        press = new Sound("button_press.mp3", false, false);
    }

    protected override void Update()
    {
        if (this.hasBeenPressed)
        {

            press.Play();

            menuManager.SetMainMenu();
        }    

        base.Update();
    }
}