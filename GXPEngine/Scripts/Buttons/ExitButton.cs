// Exit button
public class ExitButton : Button
{
    public ExitButton() : base("quit.png", 2, 1)
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