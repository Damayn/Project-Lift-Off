using GXPEngine;
using System;

class Seed : AnimationSprite
{
    float xPos;
    float yPos;

    public bool isHovered;
    public bool isSelected;

    public int seedBagIndex;

    public Seed (string seedName, float xPos, float yPos, int seedBagIndex) : base (seedName ,2, 1)
    {
        this.xPos = xPos;
        this.yPos = yPos;

        this.seedBagIndex = seedBagIndex;

        SetXY (xPos, yPos);
        this.scale = 0.2f;
    }

    void Update () 
    {
        IsHovering();
    }

    void IsHovering () 
    {
        if (isHovered) 
        {
            this.SetCycle(1, 1);
        } else
        {
            this .SetCycle(0, 1);
        }
    }
}