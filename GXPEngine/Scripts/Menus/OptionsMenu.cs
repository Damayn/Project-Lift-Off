using GXPEngine;

public class OptionsMenu : GameObject
{
    // Reference to the menu manager
    MenuManager menuManager;

    Slider slider;
    public OptionsMenu(MenuManager menuManager) : base ()
    {
        this.menuManager = menuManager;

        SetUp();
    }

    void SetUp()
    {
        // creating a back button
        BackButton backButton = new BackButton(menuManager);
        backButton.SetXY(game.width / 2, game.height / 2 + backButton.width);
        AddChild(backButton);

        slider = new Slider("track.png", "trackFront.png", "trackBack.png", "slider.png", "thumb.png", "HalfCircle.png",500, 300, 0, 100, 100);
        this.AddChild(slider);
    }
}
