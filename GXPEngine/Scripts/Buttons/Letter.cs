﻿using GXPEngine;
using System;

class Letter : Sprite
{
    public bool isHovered = false;
    public bool isClicked = false;

    private uint defColor;

    Sound press;

    public Letter(string fileName) : base(fileName)
    {
        SetOrigin(width / 2, height / 2); // Set the origin to the center of the letter sprite

        defColor = this.color;

        press = new Sound("button_press.mp3", false, false);

        if (this.name != "Backspace.png")
        {
            this.SetScaleXY(0.2f, 0.2f);
        } else
        {
            this.SetScaleXY(1.6f);
        }
    }

    void Update()
    {
        if (isHovered) 
        {
            this.color = 187;
        }
        else
        {
            this.color = defColor;
        }

        // Check if the left mouse button is clicked while the cursor is over the letter
        if (isHovered && (Input.GetMouseButtonDown(0) || ReadButton.button4Pressed))
        {

            press.Play();

            isClicked = true;
        }
    }

    public string GetLetterFileNameWithoutExtension()
    {
        if (isClicked && this.name != "Backspace.png")
        {
            // Reset the click state
            isClicked = false;

            // Return the filename without the ".png" extension
            return name.Substring(0, name.Length - 4);
        }

        return null; // Return null if the letter hasn't been clicked
    }
}
