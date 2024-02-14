using GXPEngine;
using System;
using System.Drawing;

public class Customers : AnimationSprite
{
    //the time it takes to change frames variables
    int frame;

    float lastChangeOfFace;
    float timer = 1000;

    Sprite faces;

    Random random;

    EasyDraw customerCanvas;

    string[] people = {"Faces.png", "Rolling.png", "Rolling.png", "Faces.png", "Faces.png"};
    string filename;

    public Customers(string filename) : base(filename, 5, 2)
    {
        // If you use the method in update I told you about, here you should put
        lastChangeOfFace = Time.time;

        this.filename = filename;

        random = new Random();

        SetRandomImageFilename();
        Console.WriteLine(filename);
    }

    void Update()
    {
        // This wont rly work that good since update isnt always called the same amout of times in a second. It is based on fps. For a timer is better to do smth like:

        if (Time.time - lastChangeOfFace > timer) // time.time being the time since the app started in miliseconds and timer being whatever you want for example 15 seconds (15000ms)
        {
            lastChangeOfFace = Time.time;
            frame++;
            SetCycle(frame, 1); // Or SetFrame (frame)

        }

    }

    void SetRandomImageFilename()
    {
        int stringIndex = random.Next(0, people.Length);
        filename = people[stringIndex];
    }

}
