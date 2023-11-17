using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    public class ShopScene : Scene
    {
        public override string Title { get; protected set; } = "상  점";
        public List<Item> saleItems = new() {
            new Gear(1, "철제 검", "공격에 사용되는 무기", 1500, GearType.Weapon, 12),
            new Gear(2, "원형 방패", "적의 공격을 방어하는 방패", 1000, GearType.Shield, 6),
            new Gear(3, "사슬 갑옷", "철사 고리를 엮어 만든 갑옷", 2000, GearType.Armor, 4),
            new Gear(4, "다이아몬드 검", "더 쎈 무기", 3000, GearType.Weapon, 24),
            new Gear(5, "다이아몬드 방패", "더 단단한 방패", 2000, GearType.Shield, 12),
            new Gear(6, "다이아몬드 갑옷", "딴딴 갑옷 ", 4000, GearType.Armor, 8),
            new ConsumeItem(7, "체력 포션", "체력을 회복하는 물약", 150, 1),
            new ConsumeItem(8, "마나 포션", "마나를 회복하는 물약", 150, 1),
        };
        List<ItemTableFormatter> formatters = new() {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["Desc"],
                Renderer.ItemTableFormatters["Cost"],
        };
        public override void EnterScene()
        {
            // #1. 선택지 설정.
            Options.Clear();
            saleItems = saleItems.OrderBy(x => { return x is Gear gear ? gear.GearType.String() : x.Type.String(); }).ToList();
            for (int i = 0; i < saleItems.Count; i++)
            {
                var sales = saleItems[i];
                Options.Add(new("", $"| {i + 1}|", () => { Game.Player.Inventory.Add(sales.DeepCopy()); Managers.Scene.EnterScene<ShopScene>(); }));
            }


            DrawScene();
        }

        public override void NextScene()
        {
            //while (true)
            //{
            //    var key = Console.ReadKey(true);
            //    if (key.Key != ConsoleKey.Escape) continue;
            //    Options[0].Execute();
            //    break;
            //}

            do
            {
                Renderer.PrintOptions(9, Options, true, selectionIdx);
                
            }
            while (ManageInput());
        }

        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            int row = 4;
            row = Renderer.Print(row, "이 곳은 상점");
            row = Renderer.Print(row, "아이템을 구매하거나 판매할 수 있습니다.");
            row = Renderer.DrawItemList(++row, saleItems, formatters);
            //Renderer.PrintOptions(++row, Options, true);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택] [ESC: 뒤로가기]");
            //Renderer.PrintKeyGuide("[ESC : 뒤로가기]");
        }

        private int selectionIdx = 0;

        public bool ManageInput()
        {
            var key = Console.ReadKey(true);

            var commands = key.Key switch
            {
                ConsoleKey.UpArrow => Command.MoveTop,
                ConsoleKey.DownArrow => Command.MoveBottom,
                ConsoleKey.Enter => Command.Interact,
                ConsoleKey.Escape => Command.Exit,
                _ => Command.Nothing
            };

            OnCommand(commands);

            return commands != Command.Exit;
        }

        private void OnCommand(Command cmd)
        {
            switch (cmd)
            {
                case Command.MoveTop:
                    if (selectionIdx > 0)
                        selectionIdx--;
                    break;
                case Command.MoveBottom:
                    if (selectionIdx < Options.Count - 1)
                        selectionIdx++;
                    break;
                case Command.Interact:
                    Options[selectionIdx].Execute();
                    break;
                case Command.Exit:
                    Managers.Scene.GetOption("Back").Execute();
                    break;
            }
        }
    }
}
