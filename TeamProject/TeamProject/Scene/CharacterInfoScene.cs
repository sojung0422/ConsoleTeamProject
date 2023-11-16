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

            // 캐릭터 잘 생성됬는지 임시 출력 [박상원]
            Renderer.Print(5, $"Lv. {Program.player.Level}");
            Renderer.Print(6, $"{Program.player.Name} ( {Program.player.Job} )");
            Renderer.Print(7, $"공격력 : {Program.player.DefaultDamage}");
            Renderer.Print(8, $"방어력 : {Program.player.DefaultDefense}");
            Renderer.Print(9, $"체 력 : {Program.player.DefaultHpMax}");
            Renderer.Print(10, $"Gold : {Program.player.Gold} G");

            Renderer.PrintOptions(12, Options, true);
        }
    }
}
