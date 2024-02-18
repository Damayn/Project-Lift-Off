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

    Pause pause;

    private List<Pot> pots = new List<Pot>();
    private List<Seed> seedBags = new List<Seed>();

    int currentPotIndex = 0;
    int currentSeedBagIndex = 0;

    private bool potHasPotBeenCreated = false;

    private Sprite seedBagSelectionMenu;

    Slider slider;

	public MyGame() : base(1366, 768, false, false, -1, -1, false)
	{
		SetUp();
	}

	void SetUp () 
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
        //
        settings.isTimePaused = false;

        pause = new Pause(game.width, game.height, "black.png");
        AddChild(pause);
    }


    void Update() 
	{
        this.targetFps = 60;

		if (settings.hasGameStarted)
        {
            if (potHasPotBeenCreated == false)
            {
                CreatePots();

                slider = new Slider("productionBarTrack.png", "productionBarSlider.png", 20, 20, 0, 20, 50);
                AddChild(slider);
            }
            /// still work in progress
            /// 

            if (settings.isTimePaused && Input.GetKeyDown(Key.Q))
            {

                pause.Destroy();

                TogglePauseTime();

            }
            else if (!settings.isTimePaused && Input.GetKeyDown(Key.Q))
            {

                pause = new Pause(game.width, game.height, "white.png");
                AddChild(pause);

                TogglePauseTime();

            }


            if (!settings.isTimePaused) 
            {
                UpdateProductionSlider();
                IncreaseLevel();
                SelectionMechanic();
            }

            Console.WriteLine(settings.currentLevel);
        }

    }

    void UpdateProductionSlider () 
    {
        slider.currentValue = settings.currentProductionValue;
    }

    void IncreaseLevel ()
    {
        if (slider.currentValue == slider.maximumValue)
        {
            slider.maximumValue *= 2;
            slider.currentValue = slider.maximumValue / 2;
            settings.currentLevel++;
        }
    }

    void TogglePauseTime()
    {
        settings.isTimePaused = !settings.isTimePaused;
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
        int numSeedBagsPerRow = 5;
        float seedBagSpacing = (seedBagSelectionMenu.width / 5);

        // Calculate the total width of the seed bags
        float totalSeedBagsWidth = (numSeedBagsPerRow - 1) * seedBagSpacing;

        // Calculate the starting x position for the first seed bag
        float startX = seedBagSelectionMenu.x - seedBagSelectionMenu.width / 2 + 5;

        // Calculate the starting y position for the first seed bag
        float startY = seedBagSelectionMenu.y - seedBagSelectionMenu.height / 2 + seedBagSelectionMenu.height / 4;

        // Create seed bags using a for loop
        for (int i = 0; i < 5; i++)
        {
            // Calculate the x position for the current seed bag
            float x = startX + (i % numSeedBagsPerRow) * seedBagSpacing;

            // Calculate the y position for the current seed bag
            float y = startY + (i / numSeedBagsPerRow) * seedBagSpacing;

            seedBagIndex++;

            // Create and add the seed bag to the game
            seedBag = new Seed("SeedBag " + (seedBagIndex) + ".png", x, y, seedBagIndex); // Adjust the file name pattern
            seedBags.Add(seedBag);
            AddChild(seedBag);
        }
    }

    void DeleteSeedBags () 
    {
        foreach (Seed seed in seedBags)
        {
            seed.LateDestroy();
        }

        seedBags.Clear();
    }

    void SelectionMechanic()
    {
        ToggleSelectionMode();

        HandleSelectionInput();

        UpdateSelectionHoverState();

        HandleMouseInput();
    }

    void ToggleSelectionMode()
    {
        if (Input.GetKeyDown(Key.SPACE))
        {
            if (settings.inPotSelection == false && !settings.inSeedBagSelection) // If the pot selection is off
            {
                // Turn it on
                settings.inPotSelection = true;
            }
            else if (settings.inPotSelection == true) // If pot selection is on
            {
                // Turn it off
                settings.inPotSelection = false;
                // Clear all selected pots
                ClearPotSelection();
            }
            else if (!settings.inPotSelection && settings.inSeedBagSelection) // If the seed bag selection is on and space is pressed
            {
                // Turn the selection off
                settings.inSeedBagSelection = false;
                // Clear all selected seed bags
                ClearSeedBagSelection();
                DeleteSeedBags();
                seedBagSelectionMenu.LateDestroy();
            }

            // Reset pot and seed bag selection indices
            currentPotIndex = 0;
            currentSeedBagIndex = 0;
        }
    }

    void HandleSelectionInput()
    {
        if (settings.inPotSelection) // If in pot selection mode
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
        else if (settings.inSeedBagSelection) // If in seed bag selection mode
        {
            if (Input.GetKeyDown(Key.LEFT))
            {
                MoveToPreviousSeedBag();
            }
            else if (Input.GetKeyDown(Key.RIGHT))
            {
                MoveToNextSeedBag();
            }
        }
    }
    void UpdateSelectionHoverState()
    {
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
    }

    void HandleMouseInput()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            if (settings.inPotSelection) // If in pot selection mode
            {
                // Check if the current pot is not already selected
                if (!pots[currentPotIndex].isSelected)
                {
                    // Display the seed bag selection menu above the current pot
                    seedBagSelectionMenu = new Sprite("seedBagMenu.png");
                    seedBagSelectionMenu.SetOrigin(seedBagSelectionMenu.width / 2, seedBagSelectionMenu.height / 2);
                    this.AddChild(seedBagSelectionMenu);
                    seedBagSelectionMenu.SetXY(pots[currentPotIndex].x, pots[currentPotIndex].y - pots[currentPotIndex].height / 2 - 50);

                    // Create seed bags for selection
                    CreateSeedBags();

                    // Switch to seed bag selection mode
                    settings.inPotSelection = false;
                    settings.inSeedBagSelection = true;
                }
                else
                {
                    // Deselect the pot if already selected
                    settings.inPotSelection = false;
                }

                // Clear all selected pots
                ClearPotSelection();
            }
            else if (settings.inSeedBagSelection) // If in seed bag selection mode
            {
                // Destroy the seed bag selection menu
                seedBagSelectionMenu.LateDestroy();

                // Clear all seed bag selections
                ClearSeedBagSelection();

                if (!pots[currentPotIndex].isSelected)
                {
                    // Plant the selected seed bag in the current pot
                    PlantSeedInPot(seedBags[currentSeedBagIndex], pots[currentPotIndex]);
                    // Delete the seed bags after planting
                    DeleteSeedBags();
                    // Set the current pot as selected
                    pots[currentPotIndex].isSelected = true;
                }
                else
                {
                    // Deselect the pot if already selected
                    pots[currentPotIndex].isSelected = false;
                }

                // Switch back to pot selection mode
                settings.inPotSelection = false;
                // Turn off seed bag selection mode
                settings.inSeedBagSelection = false;
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

        // Create a new plant with the same number as the seed bag
        Plant plant = new Plant("flower" + seedNumber + ".png", pot.x, pot.y - pot.height /2 - 20, pot, settings);
        AddChild(plant);
    }

    static void Main() {
		new MyGame().Start();
	}
}