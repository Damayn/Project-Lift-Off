using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing;

class NameMenu : GameObject
{
    GameSettings settings;
    MenuManager menuManager;
    PlayButton playButton;
    BackButton backButton;
    MyGame gameRef;

    const int numRows = 9;
    const int lettersPerRow = 3;
    const int spacingX = 100;
    const int spacingY = 50;

    private List<Letter> letters = new List<Letter>();
    private int currentLetterIndex = 0;
    private string enteredText = ""; // Track the entered text
    private EasyDraw easyDraw;

    Sprite background;

    Sprite pad;

    public NameMenu(GameSettings settings, MenuManager menuManager, MyGame gameRef) : base()
    {
        this.settings = settings;
        this.menuManager = menuManager;
        this.gameRef = gameRef;

        background = new Sprite("background_menu.png");
        pad = new Sprite("bg_temp1.png");

        background.width = game.width;
        background.height = game.height;

        AddChild(background);

        pad.x = 0;
        pad.y = 100;

        pad.width = 400;
        pad.height = 650;

        AddChild(pad);

        // Define starting positions
        int startX = 100;
        int startY = 200;

        for (int row = 0; row < numRows; row++)
        {
            // Loop through each letter in the row
            for (int col = 0; col < lettersPerRow; col++)
            {
                // Calculate the index of the letter (0 to 25 for A to Z)
                int letterIndex = row * lettersPerRow + col;

                // Check if the letter index is within the range of available letters (0 to 25 for A to Z)
                if (letterIndex < 26)
                {
                    // Create the letter instance with the corresponding letter filename
                    Letter letter = new Letter(((char)('A' + letterIndex)).ToString() + ".png");

                    // Calculate the position of the letter based on its index and row/col spacing
                    int posX = startX + col * spacingX;
                    int posY = startY + row * spacingY;

                    // Set the position of the letter
                    letter.SetXY(posX, posY);

                    // Add the letter to the NameMenu
                    AddChild(letter);

                    letters.Add(letter);
                }
            }
        }

        // Add the "Backspace" button in the last row next to the letter Z
        Letter backspaceButton = new Letter("Backspace.png");
        int backspaceCol = lettersPerRow - 1; // Position it in the last column
        int backspaceRow = numRows - 1; // Position it in the last row
        int backspacePosX = startX + backspaceCol * spacingX;
        int backspacePosY = startY + backspaceRow * spacingY;
        backspaceButton.SetXY(backspacePosX, backspacePosY);
        AddChild(backspaceButton);
        letters.Add(backspaceButton);


        Sprite textBox = new Sprite("name_box.png");
        textBox.SetOrigin(textBox.width / 2, textBox.height / 2);
        textBox.SetXY(game.width / 2, game.height / 2 - 30);
        textBox.scaleX = 0.5f;
        textBox.scaleY = 0.3f;
        this.AddChild(textBox);

        // Create an EasyDraw object for drawing the text box
        easyDraw = new EasyDraw(384, 64);
        easyDraw.SetXY(textBox.x - textBox.width / 2, textBox.y - 20); // Position it below the letters
        this.AddChild(easyDraw);

        playButton = new PlayButton(settings, gameRef);
        playButton.SetXY(textBox.x, textBox.y + 140);
        this.AddChild(playButton);

        backButton = new BackButton(menuManager, settings);
        backButton.SetXY(playButton.x, playButton.y + 100);
        this.AddChild(backButton);

        UpdateSelection(); // Highlight the initial selected letter
    }

    private void UpdateSelection()
    {
        foreach (var letter in letters)
        {
            letter.isHovered = false;
        }

        // Highlight the currently selected letter
        letters[currentLetterIndex].isHovered = true;
    }

    private void CycleThroughLetters(int deltaRow, int deltaCol)
    {
        // Calculate the new row and column indices based on the current selection
        int newRow = currentLetterIndex / lettersPerRow + deltaRow;
        int newCol = currentLetterIndex % lettersPerRow + deltaCol;

        // Handle wrapping around when moving between the first and last rows
        if (newRow < 0) // Moving up from the first row
        {
            newRow = numRows - 1; // Move to the last row
        }

        // Handle looping from A to Z and vice versa when moving left and right
        if (newCol < 0)
        {
            newCol = lettersPerRow - 1; // Move to the last column
            newRow--; // Move up one row
        }
        else if (newCol >= lettersPerRow)
        {
            newCol = 0; // Move to the first column
            newRow++; // Move down one row
        }

        // Ensure row index stays within bounds
        newRow = (newRow + numRows) % numRows;

        // Update the current letter index based on the new indices
        currentLetterIndex = newRow * lettersPerRow + newCol;

        // Update the selection visualization
        UpdateSelection();
    }


    void UpdateEnteredText(string letter)
    {
        if (enteredText != letter)
        {
            // Append the entered letter to the existing text
            enteredText += letter;
        }

        // Clear and redraw the text box with the updated entered text
        easyDraw.TextFont("NimbusSanL-BolIta", 36); // Set font
        easyDraw.ClearTransparent(); // Clear the text box
        easyDraw.Fill(Color.Black); // Set text color to black
        easyDraw.Text(enteredText, 5, easyDraw.height / 2 + 22); // Draw the entered text
    }

    void Update()
    {
        // Handle arrow key input to cycle through letters
        if (Input.GetKeyDown(Key.LEFT) || ReadButton.IsJoystickLeft)
        {
            if (currentLetterIndex == 21 && !playButton.isHovered)
            {
                playButton.isHovered = true;
                letters[currentLetterIndex].isHovered = false;

            }
            else if (playButton.isHovered)
            {
                playButton.isHovered = false;
                currentLetterIndex = 20;
                letters[currentLetterIndex].isHovered = true;
            }
            else if (backButton.isHovered)
            {
                currentLetterIndex = 26;
                letters[currentLetterIndex].isHovered = true;
                backButton.isHovered = false;
            } else if (!backButton.isHovered)
            {
                CycleThroughLetters(0, -1); // Move left
            }
        }
        else if (Input.GetKeyDown(Key.RIGHT) || ReadButton.IsJoystickRight)
        {
            if (currentLetterIndex == 20)
            {
                playButton.isHovered = true;
                currentLetterIndex++;

                foreach (var letter in letters)
                {
                    letter.isHovered = false;
                }
                return; // Exit the method to avoid further processing
            }
            else if (playButton.isHovered)
            {
                letters[currentLetterIndex].isHovered = true;
                playButton.isHovered = false;
            }
            else if (currentLetterIndex == 26)
            {
                backButton.isHovered = true;

                foreach (var letter in letters)
                {
                    letter.isHovered = false;
                }
            }
            else
            {
                CycleThroughLetters(0, 1); // Move right
            }

        }
        else if (Input.GetKeyDown(Key.UP) || ReadButton.IsJoystickUp)
        {
            if ((!playButton.isHovered && !backButton.isHovered))
            {
                CycleThroughLetters(-1, 0); // Move up
            }
            else if (playButton.isHovered)
            {
                playButton.isHovered = false;
                backButton.isHovered = true;
            }
            else if (backButton.isHovered)
            {
                backButton.isHovered = false;
                playButton.isHovered = true;
            }
        }
        else if (Input.GetKeyDown(Key.DOWN) || ReadButton.IsJoystickDown)
        {
            if ((!playButton.isHovered && !backButton.isHovered))
            {
                CycleThroughLetters(1, 0); // Move down
            }
            else if (playButton.isHovered)
            {
                playButton.isHovered = false;
                backButton.isHovered = true;
            }
            else if (backButton.isHovered)
            {
                backButton.isHovered = false;
                playButton.isHovered = true;
            }

        }

        if (enteredText.Length > 0)
        {
            settings.hasAName = true;
        }

        foreach (Letter letter in letters)
        {
            if (letter.isClicked)
            {
                if (letter.name == "Backspace.png")
                {
                    if (enteredText.Length > 0)
                    {
                        // Remove the last character from the entered text
                        enteredText = enteredText.Substring(0, enteredText.Length - 1);

                        UpdateEnteredText(enteredText);
                    }
                }
                else
                {
                    string letterName = letter.GetLetterFileNameWithoutExtension(); // Get the name of the clicked letter sprite
                    if (enteredText.Length < 10)
                    {
                        UpdateEnteredText(letterName);
                    }
                }

                letter.isClicked = false; // Reset click status
            }
        }

        settings.playerName = this.enteredText;
    }
}
