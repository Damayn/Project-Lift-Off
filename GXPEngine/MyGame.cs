using System;
using System.Collections.Generic;
using GXPEngine;                

public class MyGame : Game {
	GameSettings settings;
	MenuManager menuManager;
    Pot pot;
    Seed seedBag;

    private List<Pot> pots = new List<Pot>();
    private List<Seed> seedBags = new List<Seed>();

    int currentPotIndex = 0;
    int currentSeedBagIndex = 0;

    private bool potHasPotBeenCreated = false;


	public MyGame() : base(1366, 768, false, false, -1, -1, false)
	{
		SetUp();

	}

	private void SetUp () 
	{
        settings = new GameSettings();

        menuManager = new MenuManager(settings);
		menuManager.SetMainMenu();
		AddChild (menuManager);
        
        Customers customers = new Customers("Faces.png");
        AddChild(customers);
        
    }

	void Update() 
	{
        this.targetFps = 60;

		if (settings.hasGameStarted)
        {
            if (potHasPotBeenCreated == false)
            {
                CreatePots();
                CreateSeedBags();
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

    void CreateSeedBags()
    {
        int seedBagIndex = 0;

        // Calculate the number of seed bags per row and spacing
        int numSeedBagsPerRow = 2;
        float seedBagSpacingX = 200f;
        float seedBagSpacingY = 200f;

        // Calculate the total width of the seed bags
        float totalSeedBagsWidth = (numSeedBagsPerRow - 1) * seedBagSpacingX;

        // Calculate the starting x position for the first seed bag
        float startX = 100; // Adjust this as needed

        // Calculate the starting y position for the first seed bag
        float startY = (height - (2 * seedBagSpacingY)) / 2; // Adjust this as needed

        // Create seed bags using a for loop
        for (int i = 0; i < 5; i++)
        {
            // Calculate the x position for the current seed bag
            float x = startX + (i % numSeedBagsPerRow) * seedBagSpacingX;

            // Calculate the y position for the current seed bag
            float y = startY + (i / numSeedBagsPerRow) * seedBagSpacingY;

            seedBagIndex++;

            // Create and add the seed bag to the game
            seedBag = new Seed("SeedBag " + (seedBagIndex) + ".png", x, y, seedBagIndex); // Adjust the file name pattern
            seedBags.Add(seedBag);
            AddChild(seedBag);
        }
    }

    void SelectionMechanic()
    {
        if (Input.GetKeyDown(Key.SPACE))
        {
            settings.inSelectionMode = !settings.inSelectionMode;

            if (!settings.inSelectionMode)
            {
                // Clear any existing selection
                ClearSelection();
            }

            // Reset pot and seed bag selection indices
            currentPotIndex = 0;
            currentSeedBagIndex = 0;
        }

        if (settings.inSelectionMode)
        {
            // Check for keyboard input to cycle through seed bags
            if (Input.GetKeyDown(Key.LEFT))
            {
                if (settings.inSeedBagSelection)
                {
                    MoveToPreviousSeedBag();
                } else if (settings.inPotSelection) 
                {
                    MoveToNextPot();
                }
            }
            else if (Input.GetKeyDown(Key.RIGHT))
            {
                if (settings.inSeedBagSelection)
                {
                    MoveToNextSeedBag();
                } else if (settings.inPotSelection)
                {
                    MoveToNextSeedBag();
                }
            }

            // Check for right mouse button click to select a seed bag
            if (Input.GetMouseButtonDown(0))
            {
                // Select the current seed bag
                //seedBags[currentSeedBagIndex].isSelected = true;
                if (settings.inSeedBagSelection)
                {
                    Console.WriteLine(seedBags[currentPotIndex].seedBagIndex);
                    settings.inSeedBagSelection = false;
                    settings.inPotSelection = true;
                }else if (settings.inPotSelection)
                {
                    // Plant the selected seed bag in the current pot
                    PlantSeedInPot(seedBags[currentSeedBagIndex], pots[currentPotIndex]);
                }
               
            }


            // Set all seed bags to not hovered
            foreach (Seed seed in seedBags)
            {
                seed.isHovered = false;
            }

            // Set isHovered to true for the currently selected seed bag
            seedBags[currentSeedBagIndex].isHovered = true;

            // Set all pots to not hovered
            foreach (Pot pot in pots)
            {
                pot.isPotHovered = false;
            }

            // Set isPotHovered to true for the currently selected pot
            pots[currentPotIndex].isPotHovered = true;
        }
    }

    void MoveToPreviousSeedBag()
    {
        currentSeedBagIndex--;
        if (currentSeedBagIndex < 0) currentSeedBagIndex = seedBags.Count - 1;
    }

    void MoveToNextSeedBag()
    {
        currentSeedBagIndex++;
        if (currentSeedBagIndex >= seedBags.Count) currentSeedBagIndex = 0;
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

    void ClearSelection()
    {
        foreach (Seed seed in seedBags)
        {
            seed.isSelected = false;
        }
    }

    void PlantSeedInPot(Seed seed, Pot pot)
    {
        Console.WriteLine("Seed bag " + seed.name + " has been planted in pot " + pot.potIndex);
    }


    static void Main() {
		new MyGame().Start();
	}
}