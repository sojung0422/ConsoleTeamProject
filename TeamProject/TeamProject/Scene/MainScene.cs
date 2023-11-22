using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject; 
public class MainScene : Scene {
    public override string Title { get; protected set; } = "스 노 우 딘";

    #region Scene

    public override void EnterScene() {
        if (!MusicPlayer.Instance.IsPlaying)
        {
            MusicPlayer.Instance.music = "Snowy.mp3";
            MusicPlayer.Instance.PlayAsync(0.05f); // 음악파일명, 볼륨
        }
        else if (MusicPlayer.Instance.IsPlaying && MusicPlayer.Instance.music != "Snowy.mp3")
        {
            MusicPlayer.Instance.music = "Snowy.mp3";
            MusicPlayer.Instance.PlayAsync(0.05f); // 음악파일명, 볼륨
        }
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
            Renderer.PrintOptions(19, Options, true, selectedOptionIndex);
            GetInput();
        }
        while (lastCommand != Command.Interact);
    }

    protected override void DrawScene() {
        Renderer.DrawBorder(Title);
        Renderer.Print(4, "너, 인간이지? 정말 웃기는구만", margin: Console.WindowWidth / 2);
        Renderer.Print(5, "난 샌즈야, 뼈다귀 샌즈", margin: Console.WindowWidth / 2);
        Renderer.Print(7, "원래는 인간들을 감시하는 일을 해야하는데", margin: Console.WindowWidth / 2);
        Renderer.Print(8, "근데.. 그냥.. 누구 잡는 일은 딱히 신경 안써서", margin: Console.WindowWidth / 2);
        Renderer.Print(10, "하지만 던전 안쪽에는 몬스터들이 있어서 조심해", margin: Console.WindowWidth / 2);
        Renderer.Print(11, "여기 스노우딘 마을에서 준비해 두라고", margin: Console.WindowWidth / 2);


        Renderer.Print(13, "          ▄▄▀▀▀▀▀▀▀▀▀▄▄", margin: Console.WindowWidth / 2);
        Renderer.Print(14, "         █░░░░░░░░░░░░░█", margin: Console.WindowWidth / 2);
        Renderer.Print(15, "        █░░░░░░░░░░▄▄▄░░█", margin: Console.WindowWidth / 2);
        Renderer.Print(16, "        █░░▄▄▄░░▄░░███░░█", margin: Console.WindowWidth / 2);
        Renderer.Print(17, "        ▄█░▄░░░▀▀▀░░░▄░█▄", margin: Console.WindowWidth / 2);
        Renderer.Print(18, "        █░░▀█▀█▀█▀█▀█▀░░█", margin: Console.WindowWidth / 2);
        Renderer.Print(19, "        ▄██▄▄▀▀▀▀▀▀▀▄▄██▄", margin: Console.WindowWidth / 2);
        Renderer.Print(20, "      ▄█░█▀▀█▀▀▀█▀▀▀█▀▀█░█▄", margin: Console.WindowWidth / 2);
        Renderer.Print(21, "     ▄▀░▄▄▀▄▄▀▀▀▄▀▀▀▄▄▀▄▄░▀▄", margin: Console.WindowWidth / 2);
        Renderer.Print(22, "     █░░░░▀▄░█▄░░░▄█░▄▀░░░░█", margin: Console.WindowWidth / 2);
        Renderer.Print(23, "     ▀▄▄░█░░█▄▄▄▄▄█░░█░▄▄▀", margin: Console.WindowWidth / 2);
        Renderer.Print(24, "        ▀██▄▄███████▄▄██▀", margin: Console.WindowWidth / 2);
        Renderer.Print(25, "         ███████▀████████", margin: Console.WindowWidth / 2);
        Renderer.Print(26, "       ▄▄█▀▀▀▀█   █▀▀▀▀█▄▄", margin: Console.WindowWidth / 2);
        Renderer.Print(27, "       ▀▄▄▄▄▄▀▀   ▀▀▄▄▄▄▄▀ ", margin: Console.WindowWidth / 2);
        Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
    }

    #endregion

    #region Input

    protected override void OnCommandExit() {
        
    }

    #endregion
}
