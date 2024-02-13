// Exit button
public class ExitButton : Button
{
    public ExitButton() : base("ExitButton.png", 2, 1)
    {

    }

    protected override void Update()
    {
        if (hasBeenPressed)
        {
            game.Destroy();
        }

        base.Update();
    }
}