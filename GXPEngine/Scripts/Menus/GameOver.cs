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

    public GameOver(MenuManager menuManager, GameSettings settings): base()
    {

        setting = settings;
        this.menuManager = menuManager;

        BackButton backButton = new BackButton(this.menuManager);
        backButton.SetXY(game.width / 2, game.height / 2);
        AddChild(backButton);

        setting.isGameOver = false;

    }



}
