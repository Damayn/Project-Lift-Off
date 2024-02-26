// Back button
public class BackButton : Button
{
    MenuManager menuManager;

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

        }

        base.Update();

    }
}
