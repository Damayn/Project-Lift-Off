using GXPEngine;
using System;

class MainMenu : GameObject
{
    GameSettings settings;
    MenuManager menuManager;

    StartButton startButton;
    OptionsButton optionsButton;
    ExitButton backButton;

    Button[] buttons;

    Sprite background;

    Sound press;

    int currentButtonIndex = 0;

    int distance = 120;

    Sprite nameSprite;

    public MainMenu(GameSettings settings, MenuManager menuManager) : base()
    {
        nameSprite = new Sprite("Name.png");

        nameSprite.SetXY(game.width / 2, game.height / 2 - (int)(distance * 1.5));
        nameSprite.SetOrigin (nameSprite.width /2, nameSprite.height / 2);
        nameSprite.SetScaleXY(0.5f, 0.5f);


        this.settings = settings;
        this.menuManager = menuManager;

        background = new Sprite("background_menu.png");
        press = new Sound("button_press.mp3", false, false);

        startButton = new StartButton(settings);
        startButton.SetXY(game.width / 2, game.height / 2);

        backButton = new ExitButton();
        backButton.SetXY(game.width / 2, game.height / 2 + distance);

        buttons = new Button[] { startButton, backButton };
        buttons[currentButtonIndex].isHovered = true;

        AddChild(background);

        foreach (Button button in buttons)
        {
            AddChild(button);
        }

        background.height = game.height;
        background.width = game.width;

        this.AddChild(nameSprite);
    }

    void Update()
    {
        if (startButton.hasBeenPressed)
        {

            press.Play();
            menuManager.SetNameMenu();
            startButton.hasBeenPressed = false;
        }

        if (Input.GetKeyDown(Key.UP) || ReadButton.IsJoystickUp)
        {
            ChangeSelection(-1);
            
        }
        else if (Input.GetKeyDown(Key.DOWN) || ReadButton.IsJoystickDown)
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
