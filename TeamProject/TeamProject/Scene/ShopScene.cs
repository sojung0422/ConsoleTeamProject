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
        public List<Item> shopSaleItem;
        public List<Item> playerSaleItem;
        List<ItemTableFormatter> buyModeFormatters = new() {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Stat"],
                Renderer.ItemTableFormatters["Cost"],
                new("", "보유 개수", 11, i => {
                    if (Game.Player.Inventory.HasSameItem(i, out var res))
                        return res.StackCount.HasValue ? $"{res.StackCount.Value} 개" : "보유중";
                    else 
                        return i.StackCount.HasValue ? "0 개" : "미보유"; })
        };
        List<ItemTableFormatter> saleModeFormatters = new() {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Stat"],
                Renderer.ItemTableFormatters["SellCost"],
                Renderer.ItemTableFormatters["StackCount"]
        };
        private int selectionIdx = 0;
        bool shopModeToggle = true; // true : 구매모드, false : 판매모드
        int msgLine;
        List<ActionOption> buyModeOptions = new();
        List<ActionOption> saleModeOptions = new();
        public override void EnterScene()
        {
            shopSaleItem = Game.Items.ToList();
            playerSaleItem = Game.Player.Inventory.Items;
            Options.Clear();
            shopSaleItem = shopSaleItem.OrderBy(x => { return x is Gear gear ? gear.GearType.String() : x.Type.String(); }).ToList();

            // 구매 리스트 선택지 생성
            buyModeOptions.Clear();
            for (int i = 0; i < shopSaleItem.Count; i++)
            {
                var buyItem = shopSaleItem[i].DeepCopy();
                buyModeOptions.Add(new("", $"| {i + 1}|", () =>
                {
                    if (Game.Player.Inventory.HasSameItem(buyItem) && !buyItem.StackCount.HasValue)
                    {
                        Renderer.Print(msgLine, "보유 중인 아이템입니다.");
                        return;
                    }
                    if (Game.Player.Gold < buyItem.Price)
                    {
                        Renderer.Print(msgLine, "골드가 부족합니다.");
                        return;
                    }
                    Game.Player.Inventory.Add(buyItem.DeepCopy());
                    Game.Player.ChangeGold(-buyItem.Price);
                    Renderer.Print(msgLine, $"{buyItem.Name}을/를 샀습니다.");
                    Renderer.DrawItemList(8, shopSaleItem, buyModeFormatters);
                }));
            }

            // 판매 리스트 선택지 생성
            UpdateSaleListOptions();

            DrawScene();
        }

        public override void NextScene()
        {
            do
            {
                if (shopModeToggle)
                    Options = buyModeOptions;
                else
                    Options = saleModeOptions;
                msgLine = Renderer.PrintOptions(10, Options, true, selectionIdx) + 1;
                Renderer.Print(6, $"현재 골드 : {Game.Player.Gold:#,##0} G                             ");
            }
            while (ManageInput());
        }

        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            int row = 4;
            row = Renderer.Print(row, "아이템을 구매하거나 판매할 수 있습니다.");
            row = Renderer.Print(row, $"[아이템 {(shopModeToggle ? "구매" : "판매")}]");
            row = Renderer.Print(6, $"현재 골드 : {Game.Player.Gold:#,##0} G                             ");
            if (shopModeToggle)
                row = Renderer.DrawItemList(++row, shopSaleItem, buyModeFormatters);
            else
                row = Renderer.DrawItemList(++row, playerSaleItem, saleModeFormatters);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓ : 선택지 이동] [방향키 ← → : 구매/판매 모드 변경] [Enter : 선택] [ESC : 뒤로가기]");
        }

        void UpdateSaleListOptions()
        {
            saleModeOptions.Clear();
            for (int i = 0; i < playerSaleItem.Count; i++)
            {
                var saleItem = playerSaleItem[i];
                saleModeOptions.Add(new("", $"| {i + 1}|", () =>
                {
                    if (saleItem.StackCount > 1)
                        saleItem.StackCount--;
                    else
                    {
                        Game.Player.Inventory.Remove(saleItem);
                        UpdateSaleListOptions();
                        DrawScene();
                    }
                    Renderer.DrawItemList(8, playerSaleItem, saleModeFormatters);
                    Game.Player.ChangeGold((int)(saleItem.Price * 0.85));
                    Renderer.Print(msgLine, $"{saleItem.Name}을/를 팔았습니다.");
                }));
            }
            if (selectionIdx > 0)
                selectionIdx--;
        }

        public bool ManageInput()
        {
            var key = Console.ReadKey(true);

            var commands = key.Key switch
            {
                ConsoleKey.UpArrow => Command.MoveTop,
                ConsoleKey.DownArrow => Command.MoveBottom,
                ConsoleKey.LeftArrow => Command.MoveLeft,
                ConsoleKey.RightArrow => Command.MoveRight,
                ConsoleKey.Enter => Command.Interact,
                ConsoleKey.Escape => Command.Exit,
                _ => Command.Nothing
            };

            OnCommand(commands);

            return commands != Command.Exit;
        }

        private void OnCommand(Command cmd)
        {
            Renderer.Print(msgLine, "                                                                                                           ");
            Renderer.Print(msgLine + 1, "                                                                                                           ");
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
                case Command.MoveLeft:
                    shopModeToggle = !shopModeToggle;
                    UpdateSaleListOptions();
                    selectionIdx = 0;
                    DrawScene();
                    break;
                case Command.MoveRight:
                    shopModeToggle = !shopModeToggle;
                    UpdateSaleListOptions();
                    selectionIdx = 0;
                    DrawScene();
                    break;
                case Command.Interact:
                    if (Options.Count > 0)
                        Options[selectionIdx].Execute();
                    break;
                case Command.Exit:
                    Managers.Scene.GetOption("Back").Execute();
                    break;
            }
        }
    }
}