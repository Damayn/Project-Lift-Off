using GXPEngine;

public class OptionsMenu : GameObject
{
    // Reference to the menu manager
    GameSettings settings;
    MenuManager menuManager;
    Sprite background;
    public OptionsMenu(MenuManager menuManager, GameSettings settings) : base ()
    {
        this.settings = settings;
        this.menuManager = menuManager;
        background = new Sprite("background_menu.png");

        background.width = game.width;
        background.height = game.height;

        AddChild(background);

        SetUp();
    }

    void SetUp()
    {
        // creating a back button
        BackButton backButton = new BackButton(menuManager, settings);
        backButton.SetXY(game.width / 2, game.height / 2 + backButton.width);
        AddChild(backButton);

        Slider slider = new Slider("circle.png", "slider.png", 500, 300, 0, 100, 100, "trackFront.png", "progressBarOutline.png", "right.png", "right.png");
        this.AddChild(slider);
    }
}
