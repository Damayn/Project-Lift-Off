using GXPEngine;

public class PlayButton : Button
{
    GameSettings settings;
    MyGame gameRef;

    Sound press;

    public PlayButton(GameSettings settings, MyGame gameRef) : base("start.png", 2, 1)
    {
        this.settings = settings;
        this.gameRef = gameRef;
<<<<<<< Updated upstream
=======

        press = new Sound("button_press.mp3", false, false);

>>>>>>> Stashed changes
    }

    protected override void Update()
    {
        if (hasBeenPressed && settings.hasAName)
        {

            press.Play();

            settings.hasEnteredName = true;
            settings.hasGameStarted = true;

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
