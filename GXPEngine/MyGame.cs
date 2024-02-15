using System;
using System.Collections.Generic;
using GXPEngine;                

public class MyGame : Game {
	GameSettings settings;
	MenuManager menuManager;
    Pot pot;
    Seed seedBag;

    Sprite background;

    ScreenShake screenShake;

    private List<Pot> pots = new List<Pot>();
    private List<Seed> seedBags = new List<Seed>();

    int currentPotIndex = 0;
    int currentSeedBagIndex = 0;

    private bool potHasPotBeenCreated = false;

    bool isTimePaused;

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
        //hardcoding of background image testing (change it if you want)
        background = new Sprite("white.png");

        background.x = 1150;
        background.y = 0;
        background.width = 200;
        background.height = 300;

        AddChild (background);

        Customers customers = new Customers(settings);
        AddChild(customers);

        screenShake = new ScreenShake();
        screenShake.ShakeScreen(1000f, 2f);
        AddChild (screenShake);

        isTimePaused = false;
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
            /// still work in progress
            if (!isTimePaused)
            {



               TogglePauseTime();

          }

        }

        SelectionMechanic();
    }

    public void TogglePauseTime()
    {
        isTimePaused = !isTimePaused;
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

            //settings.inSelectionMode = !settings.inSelectionMode;
            if (settings.inSeedBagSelection == false && settings.inPotSelection == false)
            {
                settings.inSeedBagSelection = true;
            } else if (settings.inSeedBagSelection == true && settings.inPotSelection == false)
            {
                settings.inSeedBagSelection = false;
                ClearSeedBagSelection ();
            }

            if (settings.inPotSelection && settings.inSeedBagSelection == false)
            {
                settings.inPotSelection = false;
                ClearPotSelection ();
            }

            // Reset pot and seed bag selection indices
            currentPotIndex = 0;
            currentSeedBagIndex = 0;
        }

        if (settings.inSeedBagSelection)
        {
            if (Input.GetKeyDown(Key.LEFT))
            {
                MoveToPreviousSeedBag();
            } else if (Input.GetKeyDown(Key.RIGHT)) 
            {
                MoveToNextSeedBag();
            }
        }  else if (settings.inPotSelection)
        {
            if (Input.GetKeyDown(Key.LEFT))
            {
                MoveToPreviousPot();
            }
            else if (Input.GetKeyDown(Key.RIGHT))
            {
                MoveToNextPot();
            }
        }

        if (settings.inSeedBagSelection)
        {
            // Set all seed bags to not hovered
            ClearSeedBagSelection();

            // Set isHovered to true for the currently selected seed bag
            seedBags[currentSeedBagIndex].isHovered = true;
        }

        if (settings.inPotSelection)
        {
            // Set all pots to not hovered
            ClearPotSelection();

            // Set isPotHovered to true for the currently selected pot
            pots[currentPotIndex].isHovered = true;
        }

        // Check for right mouse button click to select a seed bag
        if (Input.GetMouseButtonDown(0))
        {
            if (settings.inSeedBagSelection)
            {
                ClearSeedBagSelection();

                seedBags[currentSeedBagIndex].isSelected = true;

                settings.inSeedBagSelection = false;
                settings.inPotSelection = true;
            }else if (settings.inPotSelection)
            {
                // Plant the selected seed bag in the current pot
                PlantSeedInPot(seedBags[currentSeedBagIndex], pots[currentPotIndex]);

                ClearPotSelection();

                pots[currentPotIndex].isChosen = true;

                settings.inPotSelection = false;
            }
               
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

    void ClearSeedBagSelection()
    {
        foreach (Seed seed in seedBags)
        {
            seed.isHovered = false;
        }
    }
    
    void ClearPotSelection ()
    {
        foreach (Pot pot in pots)
        {
            pot.isHovered = false;
        }
    }

    void PlantSeedInPot(Seed seed, Pot pot)
    {
        // Extract the number from the seed bag's name
        string seedBagName = seedBags[currentSeedBagIndex].name;
        int seedNumber = int.Parse(seedBagName.Substring(seedBagName.IndexOf("SeedBag") + 8, 1));

        // Create a new plant with the extracted seed number
        Plant plant = new Plant("flower" + seedNumber + ".png");
        plant.SetXY(pot.x, 0);
        pot.AddChild(plant);
    }

    void CheckIfPotHasPlant()
    {
        foreach (Pot pot in pots)
        {
            foreach (Plant plant in pot.GetChildren())
            {
                if (plant == null)
                {
                    pot.isChosen = false;
                }
            }
        }
    }

    static void Main() {
		new MyGame().Start();
	}
}