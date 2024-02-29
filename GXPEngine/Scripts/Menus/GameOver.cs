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

    Sound finish;
    Sound press;

    int currentButtonIndex = 0;
    int distance = 300;

    Sprite scoreBackground;

    public GameOver (MenuManager menuManager, GameSettings settings, ScoreManager scoreManager) : base()
    {
        setting = settings;
        this.menuManager = menuManager;
        this.scoreManager = scoreManager;

        canvas = new EasyDraw(1366, 768);
        canvas.SetXY(100, 100);

        background = new Sprite("background_menu.png");
        scoreBackground = new Sprite("bg_temp1.png");

        finish = new Sound("Game_Over.mp3",false,false);
        press = new Sound("button_press.mp3", false, false);

        background.width = game.width;
        background.height = game.height;
        AddChild(background);

        scoreBackground.x = 500;
        scoreBackground.y = 150;

        scoreBackground.width = 350;
        scoreBackground.height = 400;

        AddChild(scoreBackground);

        playAgainButton = new PlayAgainButton();
        playAgainButton.SetXY(game.width / 2, game.height / 2 + (distance - 100));
        this.AddChild(playAgainButton);

        ExitButton exitButton = new ExitButton();
        exitButton.SetXY(game.width /2, game.height / 2 + distance);
        this.AddChild(exitButton);

        this.AddChild(canvas);

        buttons = new Button[] { playAgainButton, exitButton};
        buttons[currentButtonIndex].isHovered = true;

        DrawTopScores();


        if (setting.play == false )
        {

            finish.Play();

            setting.play = true;

        }


    }

    void DrawTopScores()
    {
        List<string> topScores = scoreManager.GetTopScores();

        // Clear canvas before drawing
        canvas.Clear(Color.Transparent);

        // Set text font and size
        canvas.TextFont("Helvetica", 30);

        canvas.Text("LeaderBoard:",450, 50);

        // Draw top scores
        float y = 170;
        foreach (string score in topScores)
        {
            canvas.Text(score, 500, y);
            y += 50;
        }

    }

    void Update()
    {
        if (playAgainButton.hasBeenPressed)
        {

            if (setting.playAgain == false)
            {

                press.Play();

                setting.playAgain = true;

            }
           
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
