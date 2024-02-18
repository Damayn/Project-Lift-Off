using GXPEngine;

public class OptionsMenu : GameObject
{
    // Reference to the menu manager
    MenuManager menuManager;
    public OptionsMenu(MenuManager menuManager) : base ()
    {
        this.menuManager = menuManager;

        SetUp();
    }

    void SetUp () 
    {
        // creating a back button
        BackButton backButton = new BackButton(menuManager);
        backButton.SetXY(game.width / 2, game.height / 2 + backButton.width);
        AddChild(backButton);

        Slider slider = new Slider("track.png", "slider.png", 500, 300, 0, 100, 100, "trackFront.png", "trackBack.png", "thumb.png", "HalfCircle.png");
        this.AddChild(slider);
    }
}
