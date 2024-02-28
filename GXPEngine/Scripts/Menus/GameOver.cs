using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameOver : GameObject
{

    MenuManager menuManager;
    GameSettings setting;

    public GameOver(MenuManager menuManager, GameSettings settings, Sprite background): base()
    {

        setting = settings;
        this.menuManager = menuManager;

        BackButton backButton = new BackButton(this.menuManager, "menu.png");
        backButton.SetXY(game.width / 2, game.height / 2);
        AddChild(backButton);

        background.width = game.width;  
        background.height = game.height;
        AddChild(background);

        setting.isGameOver = false;

    }



}
