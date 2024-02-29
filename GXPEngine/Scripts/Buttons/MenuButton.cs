class MenuButton : Button
{
    GameSettings settings;
    MenuManager menuManager;
    public MenuButton (GameSettings settings, MenuManager menuManager) : base ("menu.png", 2, 1)
    {
        this.settings = settings;
        this.menuManager = menuManager;
    }

    protected override void Update()
    {
        if (this.hasBeenPressed)
        {
            menuManager.SetMainMenu();
        }    

        base.Update();
    }
}