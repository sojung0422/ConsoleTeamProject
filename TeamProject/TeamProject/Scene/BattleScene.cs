using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject;

public class BattleScene : Scene
{
    public override string Title { get; protected set; } = "던 전";
    public override void EnterScene()
    {
        Options.Clear();
        Options.Add(Managers.Scene.GetOption("Back"));
        DrawScene();
    }

    public override void NextScene()
    {
        Options[0].Execute();
    }
    protected override void DrawScene()
    {
        Battle battle = new Battle(Game.Player);
        battle.BattleStart();
    }
}

