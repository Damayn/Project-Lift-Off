// Exit button
using GXPEngine;

public class ExitButton : Button
{

    Sound press;

    public ExitButton() : base("quit.png", 2, 1)
    {

        press = new Sound("button_press.mp3",false,false);

    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {

            press.Play();

            game.Destroy();
        }

        base.Update();
    }
}