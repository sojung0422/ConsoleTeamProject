using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class TitleScene : Scene {
        public override void EnterScene() {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("NewGame"));
            // [우진영] 임시로 예외처리 해두었습니다.
            if (Managers.Game.data.character != null)
                Options.Add(Managers.Scene.GetOption("LoadGame"));

            DrawScene();
        }

        public override void NextScene() {
            do {
                Renderer.PrintOptions(20, Options, true, selectedOptionIndex);
                GetInput();
            }
            while (lastCommand != Command.Interact);
        }

        protected override void DrawScene()
        {
            Renderer.DrawBorder();

            Renderer.PrintCenter(6,   "██    ██ ███    ██ ██████  ███████ ██████      ████████  █████  ██      ███████ ");
            Renderer.PrintCenter(7,   "██    ██ ████   ██ ██   ██ ██      ██   ██        ██    ██   ██ ██      ██      ");
            Renderer.PrintCenter(8,   "██    ██ ██ ██  ██ ██   ██ █████   ██████         ██    ███████ ██      █████   ");
            Renderer.PrintCenter(9,   "██    ██ ██  ██ ██ ██   ██ ██      ██   ██        ██    ██   ██ ██      ██      ");
            Renderer.PrintCenter(10, "  ██████  ██   ████ ██████  ███████ ██   ██        ██    ██   ██ ███████ ███████  ");
                                                                              

            Renderer.Print(17," ▄▀▄▀▀▀▀▄▀▄", margin: Console.WindowWidth - 30);
            Renderer.Print(18," █░░░░░░░░▀▄      ▄ ", margin: Console.WindowWidth - 30);
            Renderer.Print(19,"█░░▀░░▀░░░░░▀▄▄  █░█", margin: Console.WindowWidth - 30);
            Renderer.Print(20,"█░▄░█▀░▄░░░░░░░▀▀░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(21,"█░░▀▀▀▀░░░░░░░░░░░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(22,"█░░░░░░░░░░░░░░░░░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(23,"█░░░░░░░░░░░░░░░░░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(24," █░░▄▄░░▄▄▄▄░░▄▄░░█ ", margin: Console.WindowWidth - 30);
            Renderer.Print(25," █░▄▀█░▄▀░░█░▄▀█░▄▀ ", margin: Console.WindowWidth - 30);
            Renderer.Print(26, "  ▀   ▀     ▀   ▀    ", margin: Console.WindowWidth - 30);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
        }
    }
}
