using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject;

public class DungeonGateScene : Scene
{
    public override string Title { get; protected set; } = "던 전 입 구";
    public override void EnterScene()
    {
        Options.Clear();
        if (Game.Player.Hp >= 20)
            Options.Add(Managers.Scene.GetOption("DungeonEnter"));
        DrawScene();
    }

    public override void NextScene()
    {
        Renderer.PrintOptions(15, Options, true, selectionIdx);
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter && Game.Player.Hp >= 20)
                Managers.Scene.GetOption("DungeonEnter").Execute();
            if (key.Key == ConsoleKey.Escape)
                Managers.Scene.GetOption("Main").Execute();
        }
    }
    protected override void DrawScene()
    {
        Renderer.DrawBorder(Title);
        Renderer.Print(5, $"Lv. {Game.Player.Level}");
        Renderer.Print(6, $"{Game.Player.Name} ( {Game.Player.Job} )");
        Renderer.Print(7, $"공격력 : {Game.Player.Damage}");
        Renderer.Print(8, $"방어력 : {Game.Player.Defense}");
        Renderer.Print(9, $"체 력 : {Game.Player.Hp} / {Game.Player.DefaultHpMax}");
        Renderer.Print(10, $"Gold : {Game.Player.Gold} G");
        if (Game.Player.Hp < 20)
        {
            Renderer.Print(12, "체력이 부족하여 던전에 입장할 수 없습니다(체력 20이상 필요)");
            Renderer.PrintKeyGuide("[ESC : 메인화면]");
        }   
        else
            Renderer.PrintKeyGuide("[ESC : 메인화면] [Enter : 던전 입장]");
        Renderer.Print(14, $"다음 단계 : {Game.Stage.StageLevel} 스테이지");
    }

    // 키조작
    private int selectionIdx = 0;

    public bool ManageInput()
    {
        var key = Console.ReadKey(true);

        var commands = key.Key switch
        {
            ConsoleKey.UpArrow => Command.MoveTop,
            ConsoleKey.DownArrow => Command.MoveBottom,
            ConsoleKey.Enter => Command.Interact,
            ConsoleKey.Escape => Command.Exit,
            _ => Command.Nothing
        };

        OnCommand(commands);
        if(Game.Player.Hp >= 20)
            return commands != Command.Interact;
        return true;
    }

    private void OnCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                    selectionIdx--;
                break;
            case Command.MoveBottom:
                if (selectionIdx < Options.Count - 1)
                    selectionIdx++;
                break;
            case Command.Interact:
                if (Game.Player.Hp >= 20)
                    Options[selectionIdx].Execute();
                break;
            case Command.Exit:
                Managers.Scene.GetOption("Dungeon").Execute();
                break;
        }
    }
}





