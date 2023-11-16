using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class MainScene : Scene {
        public override string Title { get; protected set; } = "스파르타 마을";

        public override void EnterScene() {
            // #1. 선택지 설정.
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("ShowInfo"));
            Options.Add(Managers.Scene.GetOption("Inventory"));
            Options.Add(Managers.Scene.GetOption("Shop"));
            Options.Add(Managers.Scene.GetOption("Dungeon"));
            Options.Add(Managers.Scene.GetOption("Rest"));
            DrawScene();
        }
        public override void NextScene() {
            while (true) {
                DrawScene();
                if (!int.TryParse(Console.ReadLine(), out int index)) continue;
                if (index < 0 || Options.Count < index) continue;
                Options[index].Execute();
                break;
            }
        }

        protected override void DrawScene() {
            Renderer.DrawBorder(Title);
            Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");
            Renderer.Print(4, "이 곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Renderer.PrintOptions(6, Options, true);
        }
    }
}
