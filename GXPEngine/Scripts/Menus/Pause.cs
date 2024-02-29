using GXPEngine;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections.Generic;

public class Pause : EasyDraw
{
    GameSettings settings;
    String imagePath;
    MenuManager menuManager;

    Button[] buttons;

    int currentButtonIndex = 0;

    public Pause(int width, int height, string imagePath, MenuManager menuManager, GameSettings settings) : base(width, height)
    {
        this.alpha = alpha;
        this.imagePath = imagePath;
        this.menuManager = menuManager;

        Bitmap image = new Bitmap(imagePath);
        //position, image size, padding?, 
        graphics.DrawImage(image, new Rectangle(0, 0, 1366, 768), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

        MenuButton menuButton = new MenuButton(settings, menuManager);
        menuButton.SetOrigin(menuButton.width / 2, menuButton.height / 2);
        menuButton.SetXY(game.width / 2, game.height / 2 - menuButton.height /2);
        this.AddChild(menuButton);

        BackButton backButton = new BackButton(menuManager, settings);
        backButton.SetOrigin(backButton.width / 2, backButton.height / 2);
        backButton.SetXY(game.width / 2, game.height / 2 + backButton.width /2);
        AddChild(backButton);

        buttons = new Button[] { menuButton , backButton };
        buttons[currentButtonIndex].isHovered = true;
    }

    void Update ()
    {
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