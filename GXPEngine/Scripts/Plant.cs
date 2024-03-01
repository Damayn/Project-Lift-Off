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
    int timeTillWilting = 10000;

    private float timeSinceWilting;
    private float timeTillDestroy;

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

    private bool waterDropAdded = false;

    private bool hasWilted = false;

    public Plant(string fileName, float x, float y, int cols, int rows, Pot pot, GameSettings settings, bool isFrog) : base(fileName, cols, rows)
    {
        this.isFrog = isFrog;

        Setup(fileName, x, y);

        if (isFrog)
        {
            this.SetScaleXY(0.2f);
        } else
        {
            this.SetScaleXY(1f);
        }
        
        this.SetOrigin(this.width / 2, this.height);
        this.pot = pot;

        this.settings = settings;

        
    }

    void Setup(string fileName, float x, float y)
    {
        this.SetOrigin(width / 2, height);

        waterDrop = new Sprite("waterDrop.png");
        if (isFrog)
        {
            waterDrop.SetScaleXY(1f);
            waterDrop.SetXY(this.x + 450, this.y - 550);
        } else
        {
            waterDrop.SetScaleXY(0.2f, 0.2f);
            waterDrop.SetXY(this.x + 50, this.y - 350);
        }
        this.AddChild(waterDrop);
        


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
                return random.Next(15, 25);
            case 2:
                return random.Next(15, 25);
            case 3:
                return random.Next(20, 30);
            case 4:
                return random.Next(20, 25);
            case 5:
                return random.Next(25, 30);
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

        if (isFrog && !isGrown && isWatered && !wilting && !hasWilted)
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
        else if (isFrog && isGrown && !wilting && !hasWilted)
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
        else if (isFrog && isGrown && wilting && !hasWilted)
        {
            if (!wiltingGrownAnimDone)
            {
                this.Animate(0.1f);
            } else
            {
                timeSinceWilting = Time.time;
                Console.WriteLine(timeSinceWilting);
            }
            
            //if (wiltingGrownAnimDone)
            //{
            //    timeSinceWilting = Time.time;
            //}

            this.SetCycle(15, 5);

            if (this.currentFrame > 18)
            {
                wiltingGrownAnimDone = true;
                this.AddChild(waterDrop);
                hasWilted = true;
                isGrown = false;
                Console.WriteLine(hasWilted);
            }
        }
        else if (isFrog && !isGrown && !hasWilted && wilting)
        {
            if (!wiltingGrowingAnimDone)
            {
                this.Animate(0.1f);
            } else
            {
                timeSinceWilting = Time.time;
                Console.WriteLine(timeSinceWilting);
            }
            
            //if (wiltingGrowingAnimDone)
            //{
            //    timeSinceWilting = Time.time;
            //}

            this.SetCycle(16, 4);

            if (this.currentFrame > 18)
            {
                wiltingGrowingAnimDone = true;
                
                hasWilted = true;
                isGrown = false;
            }
        } else if (!isFrog && !isGrown && isWatered && !wilting && !hasWilted)
        {
            this.SetFrame(1);
            
        }
        else if (!isFrog && isGrown && !wilting && !hasWilted) 
        {
            this.SetFrame(2);
        } else if ((!isFrog && !isGrown && !hasWilted && wilting) || (!isFrog && isGrown && !hasWilted && wilting))
        {
            this.SetFrame(3);
            Console.WriteLine(hasWilted);
            hasWilted = true;
        }

        //if (hasWilted && (Time.time - timeSinceWilting > timeTillDestroy))
        //{
        //    this.LateDestroy();
        //    pot.isSelected = false;
        //}

        if (wilting && !waterDropAdded)
        {
            waterDrop = new Sprite("waterDrop.png");
            if (isFrog)
            {
                waterDrop.SetScaleXY(1f);
                waterDrop.SetXY(this.x + 450, this.y - 550);
            }
            else
            {
                waterDrop.SetScaleXY(0.2f, 0.2f);
                waterDrop.SetXY(this.x + 50, this.y - 350);
            }
            this.AddChild(waterDrop);
            waterDropAdded = true;
        }

        //if (hasWilted)
        //{
        //    timeSinceWilting = Time.time;
        //}
    }

    void WiltingWhileGrowing ()
    {
        // Can wilting while growing
        if (isWatered && !isGrown && !wilting && Time.time - growingStartTimer > wiltingTimer)
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
            if ((Input.GetMouseButtonDown(1) || ReadButton.isMovingVertically) && settings.wateringState)
            {
                if (!isWatered)
                {
                    water.Play();

                    this.RemoveChild(waterDrop);
                    isWatered = true;
                }
            }

            if (isGrown && pot.isHovered && settings.harvestingState && (Input.GetMouseButtonDown(1) || (ReadButton.isMovingHorizontally)))
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
            else if (!isGrown && hasWilted && ReadButton.isMovingHorizontally && settings.wateringState) //(ReadButton.isMovingVertically || Input.GetMouseButtonDown(0)))
            {
                water.Play();

                this.RemoveChild(waterDrop);

                hasWilted = false;
                wilting = false;

                if (isFrog)
                {
                    this.SetFrame(12);
                } else
                {
                    this.SetFrame(3);
                }

                if (!isGrown)
                {
                    harvest.Play();
                }

                isGrown = true;
            }
        }
    }
}