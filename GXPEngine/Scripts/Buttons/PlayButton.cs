using GXPEngine;

public class PlayButton : Button
{
    GameSettings settings;
    MyGame gameRef;

    public PlayButton(GameSettings settings, MyGame gameRef) : base("Play.png", 2, 1)
    {
        this.settings = settings;
        this.gameRef = gameRef; 
    }

    protected override void Update()
    {
        if (hasBeenPressed && settings.hasAName)
        {
            settings.hasEnteredName = true;

            gameRef.SetUp();   
             
            foreach (GameObject child in game.GetChildren())
            {
                if (child is NameMenu)
                {
                    child.LateDestroy();
                }
            }    
        }

        

        base.Update();
    }
}
