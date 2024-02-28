using GXPEngine;

public class OptionsMenu : GameObject
{
    // Reference to the menu manager
    MenuManager menuManager;
    Sprite background;
    public OptionsMenu(MenuManager menuManager) : base ()
    {
        this.menuManager = menuManager;
        background = new Sprite("background_menu.png");

        background.width = game.width;
        background.height = game.height;

        AddChild(background);

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
