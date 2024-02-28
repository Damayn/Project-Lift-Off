using GXPEngine;

class MainMenu : GameObject
{
    GameSettings settings;
    MenuManager menuManager;

    StartButton startButton;
    OptionsButton optionsButton;
    ExitButton backButton;

    Button[] buttons;

    Sprite background;

    int currentButtonIndex = 0;

    int distance = 100;

    public MainMenu(GameSettings settings, MenuManager menuManager) : base()
    {
        this.settings = settings;
        this.menuManager = menuManager;

        background = new Sprite("background_menu.png");

        startButton = new StartButton(settings);
        startButton.SetXY(game.width / 2, game.height / 2 - distance);

        optionsButton = new OptionsButton(menuManager);
        optionsButton.SetXY(game.width / 2, game.height / 2);

        backButton = new ExitButton();
        backButton.SetXY(game.width / 2, game.height / 2 + distance);

        buttons = new Button[] { startButton, optionsButton, backButton };
        buttons[currentButtonIndex].isHovered = true;

        AddChild(background);

        foreach (Button button in buttons)
        {
            AddChild(button);
        }

        background.height = game.height;
        background.width = game.width;
    }

    void Update()
    {
        if (startButton.hasBeenPressed) 
        {
            menuManager.SetNameMenu();
        }

        if (Input.GetKeyDown(Key.UP))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(Key.DOWN))
        {
            ChangeSelection(1);
        }
    }

    private void ChangeSelection(int delta)
    {
        buttons[currentButtonIndex].isHovered = false;
        currentButtonIndex = (currentButtonIndex + delta + buttons.Length) % buttons.Length;
        buttons[currentButtonIndex].isHovered = true;
    }
}
