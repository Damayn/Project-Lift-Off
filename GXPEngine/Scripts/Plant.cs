using GXPEngine;
using System;
using System.CodeDom;
using System.ComponentModel;

class Plant : AnimationSprite
{
    Sprite waterDrop;
    Pot pot;
    GameSettings settings;

    int frame = 0;

    int timerStart;
    int timeToGrow;

    bool isWatered = false;
    bool isGrown = false;
    bool wilting = false;

    Random random = new Random();

    int animationChangeTimer;

    // Wilting when the plant starts growing
    int wiltingTimer;
    int growingStartTimer;
    int wiltingChance;

    // Wilting when you dont water the plant withing a certain time of planting it
    int plantTimer;
    int timeTillWilting = 2000;

    // Wilting when you dont harvest the plant withing a ceraint time since it has grown
    int grownTimer;

    public int productionAmoutGiven;

    public bool hasBeenClicked;

    Sound water;
    Sound harvest;
    Sound scythe;

    SoundChannel mainHarvest;
   
    Sound []tahiti = { new Sound("Frog_Harvest_Noise.mp3", false, false), new Sound("Bunny_Harvest_Noise.mp3", false, false), new Sound("Bat_Harvest_Noise.mp3", false, false), new Sound("Eye_Harvest_Noise.mp3", false, false), new Sound("Mushroom_Harvest_Noise.mp3", false, false) };
    SoundChannel magicalPlace;

    private bool isFrog;
    private bool growingAnimationDone = false;
    public bool grownAnimationDone = false;

    private bool wiltingGrownAnimDone = false;
    private bool wiltingGrowingAnimDone = false;

    private bool reverseWiltingAnim = false;

    private bool hasWilted = false;

    public Plant(string fileName, float x, float y, int cols, int rows, Pot pot, GameSettings settings, bool isFrog) : base(fileName, cols, rows)
    {
        this.isFrog = isFrog;

        Setup(fileName, x, y);

        this.SetScaleXY(0.2f);
        this.SetOrigin(this.width / 2, this.height);
        this.pot = pot;

        this.settings = settings;
    }

    void Setup(string fileName, float x, float y)
    {
        this.SetOrigin(width / 2, height);

        waterDrop = new Sprite("waterDrop.png");
        waterDrop.SetScaleXY(0.5f, 0.5f);
        waterDrop.SetXY(80, -1150);

        AddChild(waterDrop);

        timerStart = Time.time;
        timeToGrow = GetTimeToGrow(fileName);
        wiltingTimer = GetWiltingCheckTime(fileName);
        wiltingChance = GetWiltingChance(fileName);

        water = new Sound("Watering.mp3",false,false);
        harvest = new Sound("Growing_Harvestable.mp3",false,false);
        scythe = new Sound("Harvesting.mp3",false,false);

        this.SetXY(x, y);
        this.scale = 3;

        plantTimer = Time.time;
    }

    int GetTimeToGrow(string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(5000, 8000);
            case 2:
                return random.Next(3000, 5000);
            case 3:
                return random.Next(2500, 5000);
            case 4:
                return random.Next(5000, 9000);
            case 5:
                return random.Next(3000, 5000);
            default:
                return 0;
        }
    }

    int GetWiltingCheckTime (string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(5000, 8000);
            case 2:
                return random.Next(3000, 5000);
            case 3:
                return random.Next(2500, 5000);
            case 4:
                return random.Next(5000, 9000);
            case 5:
                return random.Next(3000, 5000);
            default:
                return 0;
        }
    }
    int GetWiltingChance (string fileName)
    {
        // Extract the number from the file name
        int flowerNumber = int.Parse(fileName.Substring(fileName.Length - 5, 1));

        // Assign time to grow based on the flower number
        switch (flowerNumber)
        {
            case 1:
                return random.Next(25, 30);
            case 2:
                return random.Next(25, 40);
            case 3:
                return random.Next(32, 40);
            case 4:
                return random.Next(40, 45);
            case 5:
                return random.Next(50, 55);
            default:
                return 0;
        }
    }

    void Update()
    {
        Growing();

        HandleMouseInput ();
    }

    void Growing()
    {
        WiltingWhileGrowing();
        WiltingIfNotHarvested();

        if (isWatered)
        {
            if (Time.time - timerStart > timeToGrow && !wilting && !isGrown)
            {

                if (isGrown == false)
                {
                    harvest.Play();
                }

                isGrown = true;
                growingStartTimer = Time.time;
                grownTimer = Time.time;
            }
        }

        if (isFrog && !isGrown && isWatered)
        {
            if (!wilting && !growingAnimationDone)
            {
                this.Animate(0.1f);
                this.SetCycle(0, 8);
                if (this.currentFrame > 6.5)
                {
                    growingAnimationDone = true;
                }
            }
            else if (growingAnimationDone)
            {
                this.SetCycle(8, 1);
            }
        }
        else if (isFrog && isGrown && !wilting)
        {
            if (!grownAnimationDone)
            {
                this.Animate(0.1f);
            }
            this.SetCycle(9, 6);

            if (this.currentFrame > 13)
            {
                grownAnimationDone = true;
            }
        }
        else if (isFrog && isGrown && wilting && hasWilted)
        {
            if (!wiltingGrownAnimDone)
            {
                this.Animate(0.1f);
            }
            this.SetCycle(15, 5);

            if (this.currentFrame > 18)
            {
                wiltingGrownAnimDone = true;
                this.AddChild(waterDrop);
                hasWilted = true;
                isGrown = false;
            }
        } else if (isFrog && !isGrown && !hasWilted) 
        {
        
        }
        //else if (wiltingGrowingAnimDone)
        //{
        //    this.SetFrame(20); 
        //}    
    }

    void WiltingWhileGrowing ()
    {
        // Can wilting while growing
        if (!isGrown && !wilting && Time.time - growingStartTimer > wiltingTimer)
        {
            int randomWiltingChance = random.Next(1, 101); // Random number between 1 and 100      

            Console.WriteLine(randomWiltingChance);
            Console.WriteLine(wiltingChance);

            if (randomWiltingChance <= wiltingChance)
            {
                wilting = true;
                Console.WriteLine(wilting);
            }

            growingStartTimer = Time.time;
        }
    }

    //void WiltingIfNotWatered ()
    //{
    //    if (!isWatered && Time.time - plantTimer > timeTillWilting && !wilting)
    //    {
    //        wilting = true;
    //    }
    //}

    void WiltingIfNotHarvested ()
    {
        if (isGrown && !wilting && Time.time - grownTimer > timeTillWilting)
        {
            wilting = true;
        }
    }

    void HandleMouseInput ()
    {
        if (pot.isHovered)
        {
            if (ReadButton.isMovingVertically)
            {
                if (!isWatered)
                {
                    water.Play();

                    this.RemoveChild(waterDrop);
                    isWatered = true;
                }
            }

            if (isGrown && pot.isHovered && ReadButton.isMovingHorizontally)
            {
                for (int i = 0; i < settings.customers.Count; i++)
                {
                    foreach (String flower in settings.customers[i].flowersCollected)
                    {
                        string plantNameWithoutExtension = this.name.Split('.')[0];

                        if (plantNameWithoutExtension == flower)
                        {
                            settings.customers[i].flowersCollected.Remove(flower);

                            Console.WriteLine("Removed flower: " + flower);

                                break;
                            }
                            
                        }
                    }

                mainHarvest = scythe.Play();

                switch (this.name)
                {

                    case "flower1.png":

                        magicalPlace = tahiti[0].Play();

                        break;

                    case "flower2.png":

                        magicalPlace = tahiti[1].Play();

                        break;

                    case "flower3.png":

                        magicalPlace = tahiti[2].Play();

                        break;

                    case "flower4.png":

                        magicalPlace = tahiti[3].Play();

                        break;

                    case "flower5.png":

                        magicalPlace = tahiti[4].Play();

                        break;

                }

                this.LateDestroy();

                pot.isSelected = false;

                this.hasBeenClicked = true;
            } 
            //else if (!isGrown && hasWilted && ReadButton.button4Pressed)
            //{
            //    this.RemoveChild(waterDrop);
            //    if (!reverseWiltingAnim)
            //    {
            //        this.Animate(0.1f);
            //    }
            //    this.SetCycle(20, -5);

            //    if (this.currentFrame < 16)
            //    {
            //        reverseWiltingAnim = true;
            //        this.AddChild(waterDrop);
            //        hasWilted = false;
            //        wilting = false;
            //        isGrown = true;
            //    }
            //}
        }
    }
}