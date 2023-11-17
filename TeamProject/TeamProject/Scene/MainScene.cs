using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject; 
public class MainScene : Scene {
    public override string Title { get; protected set; } = "스파르타 마을";

    public override void EnterScene() {
        // #1. 선택지 설정.
        Options.Clear();
        Options.Add(Managers.Scene.GetOption("ShowInfo"));
        Options.Add(Managers.Scene.GetOption("Inventory"));
        Options.Add(Managers.Scene.GetOption("Equipment"));
        Options.Add(Managers.Scene.GetOption("Shop"));
        Options.Add(Managers.Scene.GetOption("Dungeon"));
        Options.Add(Managers.Scene.GetOption("Rest"));
        DrawScene();
    }

    public override void NextScene() 
    {
        do
        {
            Renderer.PrintOptions(6, Options, true, selectionIdx);
        }
        while (ManageInput());

        // 세진님의 조작 처리 기능
        //while (true) {
        //    DrawScene();
        //    if (!int.TryParse(Console.ReadLine(), out int index)) continue;
        //    if (index < 0 || Options.Count < index) continue;
        //    Options[index].Execute();
        //    break;
        //}
    }

    protected override void DrawScene() {
        Renderer.DrawBorder(Title);
        Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");
        Renderer.Print(4, "이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        Renderer.PrintOptions(6, Options, true, selectionIdx);
        Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
    }

    // ================ 키 조작 관련 추가  ================ //
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
    // =================================================== //
}
