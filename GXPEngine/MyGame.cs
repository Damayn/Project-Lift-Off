using System;
using System.Collections.Generic;
using GXPEngine;                

public class MyGame : Game {
	GameSettings settings;
	MenuManager menuManager;
    Pot pot;

    private List<Pot> pots = new List<Pot>();

    private bool potHasPotBeenCreated = false;

    int currentPotIndex = 0;

	public MyGame() : base(1366, 768, false)
	{
		SetUp();
	}

	private void SetUp () 
	{
        settings = new GameSettings();

        menuManager = new MenuManager(settings);
		menuManager.SetMainMenu();
		AddChild (menuManager);
	}

	void Update() 
	{
		if (settings.hasGameStarted)
        {
            if (potHasPotBeenCreated == false)
            {
                CreatePots();
            }
        }

        SelectionMechanic();
    }

    void CreatePots()
    {
        int potIndex = 0;

        // Calculate the number of pots per row and spacing
        int numPotsPerRow = 3;
        float potSpacingX = 200f;
        float potSpacingY = 200f;

        // Calculate the total width of the pots
        float totalPotsWidth = (numPotsPerRow - 1) * potSpacingX;

        // Calculate the starting x position for the first pot in the center of the window
        float startX = (width - totalPotsWidth) / 2;

        // Calculate the y position for the pots
        float startY = height / 2;

        // Create pots using a for loop
        for (int i = 0; i < 5; i++)
        {
            // Calculate the x position for the current pot
            float x = startX + (i % numPotsPerRow) * potSpacingX;

            // Calculate the y position for the current pot
            float y = startY + (i / numPotsPerRow) * potSpacingY;

            potIndex++;

            // Create and add the pot to the game
            pot = new Pot(x, y, potIndex, 2, 1);
            pots.Add(pot);
            AddChild(pot);
        }

        potHasPotBeenCreated = true;
    }

    void SelectionMechanic()
    {
        if (Input.GetKeyDown(Key.SPACE))
        {
            settings.inSelectionMode = !settings.inSelectionMode;

            foreach (Pot pot in pots)
            {
                pot.isPotHovered = false;
            }

            currentPotIndex = 0;
        }

        if (settings.inSelectionMode)
        {
            // Check for keyboard input to cycle through pots
            if (Input.GetKeyDown(Key.LEFT))
            {
                // Move to the previous pot
                MoveToPreviousPot();
            }
            else if (Input.GetKeyDown(Key.RIGHT))
            {
                // Move to the next pot
                MoveToNextPot();
            }

            foreach (Pot pot in pots)
            {
                pot.isPotHovered = false;
            }

            // Set isPotHovered to true for the currently selected pot
            pots[currentPotIndex].isPotHovered = true;
        }
    }

    void MoveToPreviousPot()
    {
        currentPotIndex--;
        if (currentPotIndex < 0) currentPotIndex = pots.Count - 1;
    }

    void MoveToNextPot()
    {
        currentPotIndex++;
        if (currentPotIndex >= pots.Count) currentPotIndex = 0;
    }


    static void Main() {
		new MyGame().Start();
	}
}