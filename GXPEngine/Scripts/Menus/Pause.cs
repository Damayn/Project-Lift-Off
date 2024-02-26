using GXPEngine;
using System;
using System.Drawing.Imaging;
using System.Drawing;

public class Pause : EasyDraw
{

    String imagePath;

    public Pause(int width, int height, string imagePath) : base(width, height)
    {

        this.alpha = alpha;
        this.imagePath = imagePath;

        Bitmap image = new Bitmap(imagePath);
        //position, image size, padding?, 
        graphics.DrawImage(image, new Rectangle(0, 0, 1366, 768), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

    }

}