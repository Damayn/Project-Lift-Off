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

		mainMenu = new MainMenu (menuManager, settings);
		AddChild (mainMenu);

    }

	void Update() 
	{
		
	}

	static void Main() {
		new MyGame().Start();
	}
}