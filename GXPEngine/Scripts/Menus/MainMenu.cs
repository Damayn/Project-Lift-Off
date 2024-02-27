using GXPEngine;

class MainMenu : GameObject
{
    GameSettings settings;
    MenuManager menuManager;

    StartButton startButton;
    OptionsButton optionsButton;
    BackButton backButton;

    Button[] buttons;
    int currentButtonIndex = 0;

    public MainMenu(GameSettings settings, MenuManager menuManager) : base()
    {
        this.settings = settings;
        this.menuManager = menuManager;

        startButton = new StartButton(settings);
        startButton.SetXY(game.width / 2, game.height / 2 - 100);

        optionsButton = new OptionsButton(menuManager);
        optionsButton.SetXY(game.width / 2, game.height / 2);

        backButton = new BackButton(menuManager);
        backButton.SetXY(game.width / 2, game.height / 2 + 100);

        buttons = new Button[] { startButton, optionsButton, backButton };
        buttons[currentButtonIndex].isHovered = true;

        foreach (Button button in buttons)
        {
            AddChild(button);
        }
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
