using GXPEngine;
using System;
using System.Drawing.Imaging;
using System.Drawing;

public class Pause : EasyDraw
{

    String imagePath;
    MenuManager menuManager;

    public Pause(int width, int height, string imagePath, MenuManager menuManager) : base(width, height)
    {

        this.alpha = alpha;
        this.imagePath = imagePath;
        this.menuManager = menuManager;

        Bitmap image = new Bitmap(imagePath);
        //position, image size, padding?, 
        graphics.DrawImage(image, new Rectangle(0, 0, 1366, 768), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

        BackButton backButton = new BackButton(menuManager, "back.png");
        backButton.isHovered = true;
        backButton.SetXY(game.width / 2, game.height / 2 + backButton.width);
        AddChild(backButton);

    }

}