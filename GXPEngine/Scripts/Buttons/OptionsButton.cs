// Options button
using GXPEngine;

public class OptionsButton : Button
{
    MenuManager menuManager;

    Sound press;

    public OptionsButton(MenuManager menuManager) : base("setting.png", 2, 1)
    {
        this.menuManager = menuManager;

        press = new Sound("button_press.mp3",false,false);

    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {

            press.Play();

            menuManager.SetOptionsMenu();
        }

        base.Update();
    }
}