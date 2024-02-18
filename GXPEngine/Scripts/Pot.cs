using GXPEngine;
using System;

public class Pot : AnimationSprite
{
    // Boolean variable to track if a plant has been planted in the pot
    private bool plantPlanted;

    public int potIndex;

    public bool isHovered;
    public bool isSelected;

    // Constructor with parameters for position
    public Pot(float x, float y, int potIndex, int cols, int rows) : base("pot.png", cols, rows)
    {
        // Set the anchor of the pot
        SetOrigin (width /2, height /2);
        // Set the position of the pot
        SetXY(x, y);
        // Set the scale of the pot
        SetScaleXY(0.1f, 0.1f);

        // By default, when the pot is initialized, no plant is planted
        plantPlanted = false;

        this.potIndex = potIndex;
    }

    private void Update ()
    {
        IsPotHovered();
    }

    public void IsPotHovered ()
    {
        if (isHovered)
        {
            SetCycle(1, 1);
        }
        else
        {
            SetCycle(0, 1);
        }
    }

    // Method to set whether a plant has been planted in the pot
    public void SetPlantPlanted(bool planted)
    {
        plantPlanted = planted;
    }

    // Method to check if a plant has been planted in the pot
    public bool IsPlantPlanted()
    {
        return plantPlanted;
    }
}
