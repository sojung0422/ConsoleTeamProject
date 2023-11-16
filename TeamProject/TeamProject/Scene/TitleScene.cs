using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class TitleScene : Scene {
        public override void EnterScene() {
            DrawScene();
        }

        public override void NextScene() {
            // #1. 다음 씬으로 넘어가기 위해 아무 키나 누를 때까지 대기.
            Console.ReadKey();
            Managers.Scene.EnterScene<MainScene>();
        }

        protected override void DrawScene() {
            Renderer.DrawBorder();
            Renderer.Print(5, "플레이하려면 아무 키나 누르세요");
        }
    }
}
