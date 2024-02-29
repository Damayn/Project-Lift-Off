using GXPEngine;

class PlayAgainButton : Button
{

    Sound press;

    public PlayAgainButton () : base ("playAgain.png", 2, 1)
    {

        press = new Sound("button_press.mp3",false,false);

    }

}