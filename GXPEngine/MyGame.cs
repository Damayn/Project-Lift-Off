using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using GXPEngine;
using GXPEngine.Core;

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
    ModeMananger modeManager;

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
    Sound planting;
    Sound selection;

    Sound warning;
    SoundChannel ping;

    EasyDraw canvas;

    public MyGame() : base(1366, 768, false, false, -1, -1, false)
    {
        settings = new GameSettings();
        scoreManager = new ScoreManager();
        this.AddChild(scoreManager);

        menuManager = new MenuManager(settings, this, scoreManager);
        //kills the buttons?
        menuManager.SetMainMenu();
        AddChild(menuManager);

        serialPort = new SerialPortManager("COM3", 9600);
        readButton = new ReadButton(serialPort,settings);
        this.AddChild(readButton);

        backgroundMusic = new Sound("backgroundMusic.wav", true, true);
        potChange = new Sound("Select_Pot.mp3",false,false);
        bagChange = new Sound("Select_Seed_Packet.mp3",false,false);
        level = new Sound("Level_Up.mp3",false,false);
        planting = new Sound("Sowing_Planting Seeds.mp3",false,false);
        selection = new Sound("Select_Something.mp3",false,false);
        warning = new Sound("ProgressBar_Warning.mp3",true,false);

        play = backgroundMusic.Play();

        ping = warning.Play();
        ping.Mute = true;


        modeManager = new ModeMananger(serialPort, settings);
        this.AddChild (modeManager);

        ReadButton.currentColor = " 200, 33, 63, 0";
    }

    public void SetUp () 
	{
        background = new Sprite("gameplayBackground.png");
        AddChild(background);

        //hardcoding of background image testing (change it if you want)
        customerBackground = new Sprite("bg_temp1.png");

        customerBackground.x = 1040;
        customerBackground.y = -100;
        customerBackground.width = 390;
        customerBackground.height = 720;

        AddChild (customerBackground);

        slider = new Slider("progressBarOutline.png", "slider.png", 20, 20, 0, 100, 50);
        float speed = 0.5f;
        slider.currentValue = Mathf.Lerp(slider.currentValue, slider.maximumValue / 2, speed);
        AddChild(slider);

        Sprite vine = new Sprite("vine_progress_bar.png");
        vine.SetOrigin(vine.width / 2, 0);
        vine.SetXY (slider.x + slider.track.width /2 - 22, slider.y + slider.track.height /2 - 12);
        this.AddChild(vine);

        CreatePots();

        settings.isTimePaused = false;

        canvas = new EasyDraw(300, 300, false);
        canvas.SetXY( slider.x, slider.y + 60);
        AddChild(canvas);
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

                    LevelText();

                    UpdateProductionSlider();

                    SelectionMechanic();

                    DecreaseProductionSlider();
                }
            }
        }
        else if (settings.isGameOver)
        {
            play.Mute = true;
            ping.Mute = true;
            settings.playAgain = false;
            menuManager.SetGameOverMenu();
            if (!scoreSaved)
            {
                scoreManager.SaveScore(settings.playerName, settings.points);
                scoreSaved = true;
            }
        }
    }

    void LevelText ()
    {
        canvas.ClearTransparent ();
        canvas.Fill(Color.White); // Set text color
        canvas.TextFont("Helvetika", 28); // Set font and size
        canvas.Text("Level: " + GameSettings.currentLevel, 100,100); // Adjust position as needed
    }

    void Pause ()
    {
        if ((Input.GetKeyDown(Key.Q) || ReadButton.button3Pressed) && !settings.isTimePaused)
        {
            pause = new Pause(game.width, game.height, "white.png", menuManager, settings);
            AddChild(pause);

            TogglePauseTime();
        } else if ((Input.GetKeyDown(Key.Q) || ReadButton.button3Pressed)  && settings.isTimePaused)
        {
            pause.Destroy();
            TogglePauseTime();
        }

    }

    void AddNewCustomer ()
    {
        if (settings.customers.Count == 0 && !settings.isGameOver)
        {
            Console.WriteLine("asd");

            Customers customer = new Customers(settings, slider, customerBackground);
            settings.customers.Add(customer);
            AddChild(customer);
        }
    }

    void GameOver () 
    {

        if (slider.currentValue <= 5) 
        {
            settings.isGameOver = true;

        }
        else if (slider.currentValue <= 10 )
        {

            if (settings.barWarning == false )
            {

                ping.Mute = false;

                settings.barWarning = true;

            }

        }
        else if (slider.currentValue > 10)
        {

            ping.Mute = true;

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

            level.Play();

            slider.maximumValue *= 2;
            slider.currentValue = slider.maximumValue / 2;
            GameSettings.currentLevel++;
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
        int numPotsPerRow = 5; // Total number of pots per row
        float potSpacingX = 285f; // Spacing between pots along the x-axis
        float scaleIncrement = 0.08f; // Amount to increment scale for each pot

        // Calculate the starting x position for the leftmost pot
        float startX = width / 2 - ((numPotsPerRow / 2) * potSpacingX);

        // Calculate the y position for the pots
        float startY = height;

        // Create pots starting from the leftmost one
        for (int i = 0; i < numPotsPerRow; i++)
        {
            // Calculate the x position for the current pot
            float currentX = startX + i * potSpacingX;

            // Calculate the scale for the current pot
            float scale = 0.3f + (i < 2 ? i * scaleIncrement : (4 - i) * scaleIncrement);

            // Create and add the pot
            Pot pot = new Pot(currentX, startY, i, 2, 1);
            pot.SetScaleXY(scale);
            pots.Add(pot);
            AddChild(pot);
        }
    }

    void CreateSeedBags()
    {
        int seedBagIndex = 0;

        // Calculate the number of seed bags per row and spacing
        int numSeedBagsPerRow = 5;
        float seedBagSpacing = (seedBagSelectionMenu.width / 5) - 10;

        // Calculate the total width of the seed bags
        float totalSeedBagsWidth = (numSeedBagsPerRow - 1) * seedBagSpacing;

        // Calculate the starting x position for the first seed bag
        float startX = seedBagSelectionMenu.x - seedBagSelectionMenu.width / 2 + 25;

        // Calculate the starting y position for the first seed bag
        float startY = seedBagSelectionMenu.y - 95;

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

        if (Input.GetKeyDown(Key.SPACE) || ReadButton.button1Pressed == true)
        {

            changing = selection.Play();

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
            currentPotIndex = 2;
            currentSeedBagIndex = 0;
        }
    }

    void HandleSelectionInput()
    {
        if (settings.inPotSelection) // If in pot selection mode
        {
            if (Input.GetKeyDown(Key.LEFT) || ReadButton.IsJoystickLeft)
            {
                MoveToPreviousPot();
            }
            else if (Input.GetKeyDown(Key.RIGHT) || ReadButton.IsJoystickRight)
            {
                MoveToNextPot();
            }
        }
        else if (settings.inSeedBagSelection) // If in seed bag selection mode
        {
            if (Input.GetKeyDown(Key.LEFT) || ReadButton.IsJoystickLeft)
            {
                MoveToPreviousSeedBag();
            }
            else if (Input.GetKeyDown(Key.RIGHT) || ReadButton.IsJoystickRight)
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
        if (Input.GetMouseButtonDown(0) || ReadButton.button4Pressed)
        {
            if (settings.inPotSelection) // If in pot selection mode
            {
                // Check if the current pot is not already selected
                if (!pots[currentPotIndex].isSelected)
                {

                    // Display the seed bag selection menu above the current pot
                    seedBagSelectionMenu = new Sprite("seedBagMenu.png");
                    //seedBagSelectionMenu.SetScaleXY(2f, 2f);
                    seedBagSelectionMenu.SetOrigin(seedBagSelectionMenu.width / 2, seedBagSelectionMenu.height / 2 + 30);
                    seedBagSelectionMenu.SetXY(game.width /2, game.height /2);
                    this.AddChild(seedBagSelectionMenu);

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
                settings.inPotSelection = true;
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

        planting.Play();

        // Calculate the vertical offset based on the pot's size
        //float yOffsetFrog = pot.height + 65 + (currentPotIndex * 5.5f);
        //float yOffset = pot.height / 2 + 100 - (currentPotIndex * 10f);


        float x = 0;
        float y = 0;

        // Define a Vector2 variable to hold the position
        Vector2 plantPosition = new Vector2(0, 0);


        if (seedNumber == 1)
        {
            Plant plant = new Plant("flower" + seedNumber + ".png", GetPlantPosition(pot, seedNumber).x, GetPlantPosition(pot, seedNumber).y, 8, 3, pot, settings, true);
            AddChild(plant);
            plants.Add(plant);

            Console.WriteLine(plantPosition.x);
        }
        else
        {
            Plant plant = new Plant("flower" + seedNumber + ".png", GetPlantPosition(pot, seedNumber).x, GetPlantPosition(pot, seedNumber).y, 4, 1, pot, settings, false);
            AddChild(plant);
            plants.Add(plant);
        }

        //if (currentPotIndex == 0)
        //{
        //    switch (seedNumber)
        //    {
        //        case 1:
        //            plantPosition = new Vector2(pot.x, pot.y);
        //            return;
        //        case 2:
        //            return;
        //        case 3:
        //            return;
        //        case 4:
        //            return;
        //        default:
        //            return;
        //    }
        //}
        //else if (currentPotIndex == 1)
        //{
        //    switch (seedNumber)
        //    {
        //        case 0:
        //            return;
        //        case 1:
        //            return;
        //        case 2:
        //            return;
        //        case 3:
        //            return;
        //        case 4:
        //            return;
        //        default:
        //            return;
        //    }
        //}
        //else if (currentPotIndex == 2)
        //{
        //    switch (seedNumber)
        //    {
        //        case 0:
        //            return;
        //        case 1:
        //            return;
        //        case 2:
        //            return;
        //        case 3:
        //            return;
        //        case 4:
        //            return;
        //        default:
        //            return;
        //    }
        //}
        //else if (currentPotIndex == 3)
        //{
        //    switch (seedNumber)
        //    {
        //        case 0:
        //            return;
        //        case 1:
        //            return;
        //        case 2:
        //            return;
        //        case 3:
        //            return;
        //        case 4:
        //            return;
        //        default:
        //            return;
        //    }
        //}
        //else if (currentPotIndex == 4)
        //{
        //    switch (seedNumber)
        //    {
        //        case 0:
        //            return;
        //        case 1:
        //            return;
        //        case 2:
        //            return;
        //        case 3:
        //            return;
        //        case 4:
        //            return;
        //        default:
        //            return;
        //    }
        //}



    }

    private Vector2 GetPlantPosition(Pot pot, int seedNumber)
    {
        Vector2 plantPosition = new Vector2(0, 0); // Default position

        // Determine position based on pot index and seed number
        if (pot != null)
        {
            switch (currentPotIndex)
            {
                case 0:
                    switch (seedNumber)
                    {
                        case 1:
                            plantPosition = new Vector2(pot.x - 75, pot.y - 300);
                            break;
                        case 2:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 160);
                            break;
                        case 3:
                            plantPosition = new Vector2(pot.x + 25, pot.y - 160);
                            break;
                        case 4:
                            plantPosition = new Vector2(pot.x + 40, pot.y - 160);
                            break;
                        case 5:
                            plantPosition = new Vector2(pot.x, pot.y - 160);
                            break;
                        default:
                            // Define default position
                            break;
                    }
                    break;
                case 1:
                    switch (seedNumber)
                    {
                        case 1:
                            plantPosition = new Vector2(pot.x - 75, pot.y - 350);
                            break;
                        case 2:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 205);
                            break;
                        case 3:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 205);
                            break;
                        case 4:
                            plantPosition = new Vector2(pot.x + 40, pot.y - 205);
                            break;
                        case 5:
                            plantPosition = new Vector2(pot.x, pot.y - 205);
                            break;
                        default:
                            // Define default position
                            break;
                    }
                    break;

                case 2:
                    switch (seedNumber)
                    {
                        case 1:
                            plantPosition = new Vector2(pot.x - 75, pot.y - 400);
                            break;
                        case 2:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 255);
                            break;
                        case 3:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 255);
                            break;
                        case 4:
                            plantPosition = new Vector2(pot.x + 40, pot.y - 255);
                            break;
                        case 5:
                            plantPosition = new Vector2(pot.x, pot.y - 255);
                            break;
                        default:
                            // Define default position
                            break;
                    }
                    break;

                case 3:
                    switch (seedNumber)
                    {
                        case 1:
                            plantPosition = new Vector2(pot.x - 75, pot.y - 350);
                            break;
                        case 2:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 205);
                            break;
                        case 3:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 205);
                            break;
                        case 4:
                            plantPosition = new Vector2(pot.x + 40, pot.y - 205);
                            break;
                        case 5:
                            plantPosition = new Vector2(pot.x, pot.y - 205);
                            break;
                        default:
                            // Define default position
                            break;
                    }
                    break;

                case 4:
                    switch (seedNumber)
                    {
                        case 1:
                            plantPosition = new Vector2(pot.x - 75, pot.y - 310);
                            break;
                        case 2:
                            plantPosition = new Vector2(pot.x + 20, pot.y - 165);
                            break;
                        case 3:
                            plantPosition = new Vector2(pot.x + 25, pot.y - 160);
                            break;
                        case 4:
                            plantPosition = new Vector2(pot.x + 40, pot.y - 160);
                            break;
                        case 5:
                            plantPosition = new Vector2(pot.x, pot.y - 165);
                            break;
                        default:
                            // Define default position
                            break;
                    }
                    break;

                default:
                    break;
            }
        }

        return plantPosition;
    }


    static void Main() {
		new MyGame().Start();
	}

}