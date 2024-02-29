using System;
using System.Collections.Generic;
using System.IO.Ports;
using GXPEngine;                

public class MyGame : Game {
	GameSettings settings;
	MenuManager menuManager;
    Pot pot;
    Seed seedBag;
    Sprite background;
    Pause pause;
    Slider slider;
    SerialPortManager serialPort;
    ReadButton readButton;

    private List<Pot> pots = new List<Pot>();
    private List<Seed> seedBags = new List<Seed>();

    private List<Plant> plants = new List<Plant>();

    int currentPotIndex = 0;
    int currentSeedBagIndex = 0;

    private Sprite seedBagSelectionMenu;
    private Sprite customerBackground;

    ScoreManager scoreManager;
    private bool scoreSaved;

    Sound backgroundMusic;
    SoundChannel play;

    Sound potChange;
    Sound bagChange;
    SoundChannel changing;

    Sound level;

    public MyGame() : base(1366, 768, false, false, -1, -1, false)
    {
        settings = new GameSettings();
        scoreManager = new ScoreManager();
        this.AddChild(scoreManager);
        
        menuManager = new MenuManager(settings, this, scoreManager);
        //kills the buttons?
        menuManager.SetMainMenu();
        AddChild(menuManager);

        serialPort = new SerialPortManager ("COM3", 9600);
        readButton = new ReadButton(serialPort);
        this.AddChild(readButton);

        backgroundMusic = new Sound("backgroundMusic.wav", true, true);
        potChange = new Sound("Select_Pot.mp3",false,false);
        bagChange = new Sound("Select_Seed_Packet.mp3",false,false);
        level = new Sound("Level_Up.mp3",false,false);

        play = backgroundMusic.Play();

    }

    public void SetUp () 
	{
        background = new Sprite("gameplayBackground.png");
        AddChild(background);

        //hardcoding of background image testing (change it if you want)
        customerBackground = new Sprite("white.png");

        customerBackground.x = 1150;
        customerBackground.y = 0;
        customerBackground.width = 200;
        customerBackground.height = 300;

        AddChild (customerBackground);

        slider = new Slider("productionBarTrack.png", "productionBarSlider.png", 20, 20, 0, 100, 50);

        settings.isTimePaused = false;

        CreatePots();

        float speed = 0.05f;

        slider.currentValue = Mathf.Lerp(slider.currentValue, slider.maximumValue / 2, speed);

        AddChild(slider);

        Console.WriteLine(settings.customers.Count);
    }

    void Update() 
	{
        this.targetFps = 60;

        if (!settings.isGameOver)
        {

            play.Mute = false;
            settings.play = false;

            if (settings.hasGameStarted && settings.hasEnteredName)
            {  
                    Pause();
                if (!settings.isTimePaused)
                {
                    GameOver();

                    AddNewCustomer();

                    UpdateProductionSlider();

                    SelectionMechanic();

                    DecreaseProductionSlider();
                }
            }
        }
        else if (settings.isGameOver)
        {
            play.Mute = true;
            settings.playAgain = false;
            menuManager.SetGameOverMenu();
            if (!scoreSaved)
            {
                scoreManager.SaveScore(settings.playerName, settings.points);
                scoreSaved = true;
            }
        }
    }

    void Pause ()
    {
        if ((Input.GetKeyDown(Key.Q) || readButton.button3Pressed) && !settings.isTimePaused)
        {
            pause = new Pause(game.width, game.height, "white.png", menuManager, settings);
            AddChild(pause);

            TogglePauseTime();
        } else if ((Input.GetKeyDown(Key.Q) || readButton.button3Pressed)  && settings.isTimePaused)
        {
            pause.Destroy();
            TogglePauseTime();
        }

    }

    void AddNewCustomer ()
    {
        if (settings.customers.Count == 0 && !settings.isGameOver) 
        {
            Customers customer = new Customers(settings, slider);
            settings.customers.Add(customer);
            AddChild (customer);
        }
    }

    void GameOver () 
    {
        if (slider.currentValue <= 5) 
        {
            settings.isGameOver = true;
        }
    }
    
    void UpdateProductionSlider()
    {
        foreach (Plant plant in plants)
        {
            if (plant.hasBeenClicked)
            {
                slider.currentValue += plant.productionAmoutGiven;
                plant.hasBeenClicked = false;
            }
        }

        if (slider.currentValue >= slider.maximumValue)
        {

            //level.Play();

            slider.maximumValue *= 2;
            slider.currentValue = slider.maximumValue / 2;
            settings.currentLevel++;
        }
    }

    void DecreaseProductionSlider ()
    {
        float speed = 0.0002f;
        slider.currentValue = Mathf.Lerp (slider.currentValue, 0, speed);
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

    void DeleteSeedBags() 
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

        if (Input.GetKeyDown(Key.SPACE) || readButton.button1Pressed == true)
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

        changing = bagChange.Play();

        if (currentSeedBagIndex < 0) currentSeedBagIndex = seedBags.Count - 1;
    }

    void MoveToNextSeedBag()
    {
        currentSeedBagIndex++;

        changing = bagChange.Play();

        if (currentSeedBagIndex >= seedBags.Count) currentSeedBagIndex = 0;
    }

    void MoveToPreviousPot()
    {
        currentPotIndex--;

        changing = potChange.Play();

        if (currentPotIndex < 0) currentPotIndex = pots.Count - 1;
    }

    void MoveToNextPot()
    {
        currentPotIndex++;

        changing = potChange.Play();

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
        plants.Add(plant);
    }

    static void Main() {
		new MyGame().Start();
	}

}