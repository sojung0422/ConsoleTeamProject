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
        Options.Add(Managers.Scene.GetOption("Main"));
        if (Game.Player.Hp >= 20)
            Options.Add(Managers.Scene.GetOption("DungeonEnter"));
        DrawScene();
    }

    public override void NextScene()
    {
        do
        {
            Renderer.PrintOptions(14, Options, true, selectionIdx);
        }
        while (ManageInput());

        //while (true)
        //{
        //    DrawScene();
        //    if (!int.TryParse(Console.ReadLine(), out int index)) continue;
        //    if (index < 0 || Options.Count < index) continue;
        //    Options[index].Execute();
        //    break;
        //}
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
            Renderer.Print(12, "체력이 부족하여 던전에 입장할 수 없습니다(체력 20이상 필요)");

        Renderer.PrintOptions(14, Options, true);
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

        return commands != Command.Interact;
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
                Options[selectionIdx].Execute();
                break;
        }
    }
}





