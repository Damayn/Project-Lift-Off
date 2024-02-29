using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameOver : GameObject
{

    MenuManager menuManager;
    GameSettings setting;
    Sprite background;
    PlayAgainButton playAgainButton;
    EasyDraw canvas;
    ScoreManager scoreManager;
    Button[] buttons;

    int currentButtonIndex = 0;

    public GameOver (MenuManager menuManager, GameSettings settings, ScoreManager scoreManager) : base()
    {
        setting = settings;
        this.menuManager = menuManager;
        this.scoreManager = scoreManager;

        canvas = new EasyDraw(300, 300);
        canvas.SetXY(100, 100);
        

        background = new Sprite("background_menu.png");

        background.width = game.width;
        background.height = game.height;
        AddChild(background);

        playAgainButton = new PlayAgainButton();
        playAgainButton.SetXY(game.width / 2, game.height / 2 - playAgainButton.height / 2);
        this.AddChild(playAgainButton);

        ExitButton exitButton = new ExitButton();
        exitButton.SetXY(game.width / 2, game.height / 2 + exitButton.height / 2);
        this.AddChild(exitButton);

        this.AddChild(canvas);

        buttons = new Button[] { playAgainButton, exitButton};
        buttons[currentButtonIndex].isHovered = true;

        DrawTopScores();
    }

    void DrawTopScores()
    {
        List<string> topScores = scoreManager.GetTopScores();

        // Clear canvas before drawing
        canvas.Clear(Color.Transparent);

        // Set text font and size
        canvas.TextFont("Helvetika", 30);

        // Draw top scores
        float y = 50;
        foreach (string score in topScores)
        {
            canvas.Text(score, 50, y);
            y += 50;
        }

    }

    void Update()
    {
        if (playAgainButton.hasBeenPressed)
        {
            menuManager.SetMainMenu();
        }

        if (Input.GetKeyDown(Key.UP))
        {
            ChangeSelection(-1);
            Console.WriteLine("asd");
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
