using System;                   // System contains a lot of default C# libraries 
using GXPEngine;                // GXPEngine contains the engine

public class MyGame : Game {
	public MyGame() : base(800, 600, false)
	{
		
	}

	void Update() 
	{
		Console.WriteLine("print");
	}

	// Main is the first method that's called when the program is run
	static void Main() {
		// Create a "MyGame" and start it:
		new MyGame().Start();
	}
}