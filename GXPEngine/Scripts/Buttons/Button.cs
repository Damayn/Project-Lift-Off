using GXPEngine;
using System;
using System.IO;

public class Button : AnimationSprite
{
    // Get and set the bool
    public bool hasBeenPressed { get; set;}
    public bool isHovered { get; set;}

    // Have a string passed to the constructor, the number of column and rows the sprite has
    public Button(string image, int cols, int rows) : base (image, cols, rows) // Pass the image the columns and rows from the constructor to the base wich is the animated srpite object
    {
        this.SetOrigin(width /2, height / 2);
        this.scale = 0.8f;
    }

    protected virtual void Update()
    {
        ButtonUpdate();
    }

    protected virtual void ButtonUpdate ()
    {
        if (isHovered)
        {
            // Set the cycle to the first frame
            this.SetCycle(1);
            // If the button has been clicked
            if (Input.GetMouseButtonDown(0) || ReadButton.button4Pressed)
            {
                hasBeenPressed = true;
            }
        }
        else
        {
            this.SetCycle(0);
        }
    }
}