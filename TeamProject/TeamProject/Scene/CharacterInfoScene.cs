using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class CharacterInfoScene : Scene {
        public override string Title { get; protected set; } = "상태 보기";
        public override void EnterScene() {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("Back"));
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
            Renderer.Print(3, "캐릭터의 정보가 표시됩니다.");

            // ==== 캐릭터 정보 표시 ====

            // ==========================

            Renderer.PrintOptions(10, Options, true);
        }
    }
}
