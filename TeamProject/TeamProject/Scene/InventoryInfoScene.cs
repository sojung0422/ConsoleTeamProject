using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class InventoryInfoScene : Scene {
        public override string Title { get; protected set; } = "인벤토리";

        public override void EnterScene() {
            // #1. 선택지 설정.
            Options.Clear();
            // ...

            DrawScene();
        }

        public override void NextScene() {

        }

        protected override void DrawScene() {
            Renderer.DrawBorder(Title);
            int row = 4;
            row = Renderer.Print(row, "이 곳은 인벤토리");
            row = Renderer.Print(row, "인벤토리 정보를 보여줍니다.");
            Renderer.PrintOptions(++row, Options, true);
        }


    }
}
