using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class InventoryInfoScene : Scene {
        public override string Title { get; protected set; } = "인 벤 토 리";

        public override void EnterScene() {
            // #1. 선택지 설정.
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("Back"));

            DrawScene();
        }

        public override void NextScene() {
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Escape) continue;
                Options[0].Execute();
                break;
            }
        }

        protected override void DrawScene() {
            Renderer.DrawBorder(Title);
            int row = 4;
            row = Renderer.Print(row, "이 곳은 인벤토리");
            row = Renderer.Print(row, "인벤토리 정보를 보여줍니다.");

            List<ItemTableFormatter> formatters = new() {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Desc"],
                Renderer.ItemTableFormatters["StackCount"],
            };
            row = Renderer.DrawItemList(++row, Game.Player.Inventory.Items, formatters);

            Renderer.PrintOptions(++row, Options, true);
            Renderer.PrintKeyGuide("[ESC : 뒤로가기]");
        }


    }
}
