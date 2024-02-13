// Options button
public class OptionsButton : Button
{
    MenuManager menuManager;
    public OptionsButton(MenuManager menuManager) : base("OptionsButton.png", 2, 1)
    {
        this.menuManager = menuManager;
    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {
            menuManager.SetOptionsMenu();
        }

        base.Update();
    }
}