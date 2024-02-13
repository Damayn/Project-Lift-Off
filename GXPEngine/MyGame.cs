using System;                   
using GXPEngine;                

public class MyGame : Game {
	GameSettings settings;
	MenuManager menuManager;
	MainMenu mainMenu;

	public MyGame() : base(1366, 768, false)
	{
		SetUp();
	}

	private void SetUp () 
	{
        settings = new GameSettings();

        menuManager = new MenuManager(settings);
		AddChild (menuManager);

		mainMenu = new MainMenu(menuManager, settings);
		AddChild(mainMenu);
	}

	void Update() 
	{
		// If the play button has been pressed
		if (settings.hasGameStarted) 
		{
			// Go through every child that the game has
			foreach (GameObject child in this.GetChildren ()) 
			{
				// If that cild is the main menu
				if (child is MainMenu)
				{
					// Delete it
					child.LateDestroy();
				}
			}	
		}
	}

	static void Main() {
		new MyGame().Start();
	}
}