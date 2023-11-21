using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject; 
public class MainScene : Scene {
    public override string Title { get; protected set; } = "스파르타 마을";

    #region Scene

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
            Renderer.PrintOptions(6, Options, true, selectedOptionIndex);
            GetInput();
        }
        while (lastCommand != Command.Interact);
    }

    protected override void DrawScene() {
        Renderer.DrawBorder(Title);
        Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");
        Renderer.Print(4, "이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
        Renderer.PrintOptions(6, Options, true, selectedOptionIndex);
        Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
    }

    #endregion

    #region Input

    protected override void OnCommandExit() {
        
    }

    #endregion
}
