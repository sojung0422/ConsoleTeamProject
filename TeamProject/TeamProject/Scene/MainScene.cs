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
            Renderer.PrintOptions(9, Options, true, selectedOptionIndex);
            GetInput();
        }
        while (lastCommand != Command.Interact);
    }

    protected override void DrawScene() {
        Renderer.DrawBorder(Title);
        Renderer.PrintCenter(4, "[폐 허]");
        Renderer.Print(6, "폐허에는 퍼즐과 몬스터로 가득하다.");
        Renderer.Print(7, "이 곳에서 나가기 위해 싸움을 준비하자");
        
        Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
    }

    #endregion

    #region Input

    protected override void OnCommandExit() {
        
    }

    #endregion
}
