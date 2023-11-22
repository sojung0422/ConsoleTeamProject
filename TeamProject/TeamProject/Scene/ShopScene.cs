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

        #region Fields

        public List<Item> shopSaleItem = new();
        public List<Item> playerSaleItem = new();
        public List<TableFormatter<Item>> formattersBuy = new();
        public List<TableFormatter<Item>> formattersSale = new();

        //List<ItemTableFormatter> buyModeFormatters = new() {
        //        Renderer.ItemTableFormatters["Index"],
        //        Renderer.ItemTableFormatters["Name"],
        //        Renderer.ItemTableFormatters["ItemType"],
        //        Renderer.ItemTableFormatters["Effect"],
        //        Renderer.ItemTableFormatters["Cost"],
        //        new("", "보유 개수", 11, i => {
        //            if (Game.Player.Inventory.HasSameItem(i, out var res))
        //                return res.StackCount.HasValue ? $"{res.StackCount.Value} 개" : "보유중";
        //            else 
        //                return i.StackCount.HasValue ? "0 개" : "미보유"; })
        //};
        //List<ItemTableFormatter> saleModeFormatters = new() {
        //        Renderer.ItemTableFormatters["Index"],
        //        Renderer.ItemTableFormatters["Name"],
        //        Renderer.ItemTableFormatters["ItemType"],
        //        Renderer.ItemTableFormatters["Effect"],
        //        Renderer.ItemTableFormatters["SellCost"],
        //        Renderer.ItemTableFormatters["StackCount"]
        //};
        bool shopModeToggle = true; // true : 구매모드, false : 판매모드
        string msg = "";
        List<ActionOption> buyModeOptions = new();
        List<ActionOption> saleModeOptions = new();

        #endregion

        #region Scene

        public override void EnterScene() {
            // #1. 씬 설정.
            msg = "";
            MusicPlayer.Instance.music = "Store.mp3";
            MusicPlayer.Instance.PlayAsync(0.05f); // 음악파일명, 볼륨
            // #2. 아이템 정보 설정.
            shopSaleItem = Game.Items.ToList();
            playerSaleItem = Game.Player.Inventory.Items;
            shopSaleItem = shopSaleItem.OrderBy(x => { return x is Gear gear ? gear.GearType.String() : x.Type.String(); }).ToList();

            // #3. 선택지 설정.
            Options.Clear();
            UpdateSaleListOptions();    // 판매 리스트 선택지 생성.
            buyModeOptions.Clear();     // 구매 리스트 선택지 생성
            for (int i = 0; i < shopSaleItem.Count; i++) {
                var buyItem = shopSaleItem[i].DeepCopy();
                buyModeOptions.Add(new("", "", () => {
                    if (Game.Player.Inventory.HasSameItem(buyItem) && !buyItem.StackCount.HasValue) {
                        msg = "보유 중인 아이템입니다.";
                        return;
                    }
                    if (Game.Player.Gold < buyItem.Price) {
                        msg = "골드가 부족합니다.";
                        return;
                    }
                    Game.Player.Inventory.Add(buyItem.DeepCopy());
                    Game.Player.ChangeGold(-buyItem.Price);
                    msg = $"{buyItem.Name}을/를 샀습니다.";
                }));
            }


            // #4. 테이블 설정.
            formattersBuy = Managers.Table.GetFormatters<Item>(new string[] { "Index", "Name", "ItemType", "Effect", "Cost", "ShopCount" });
            formattersSale = Managers.Table.GetFormatters<Item>(new string[] { "Index", "Name", "ItemType", "Effect", "SellCost", "StackCount" });


            // draw
            Renderer.DrawBorder(Title);
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
                DrawScene();
                GetInput();
            }
            while (lastCommand != Command.Interact || lastCommand != Command.Exit);
        }

        protected override void DrawScene()
        {
            // Clear Table...
            int row = 3;
            for (int i = 0; i < 20; i++)
                Renderer.ClearLine(row + i);

            // Draw Scene...
            row = Renderer.Print(row + 1, "아이템을 구매하거나 판매할 수 있습니다.");
            row = Renderer.Print(row, $"[아이템 {(shopModeToggle ? "구매" : "판매")}]");
            row = Renderer.Print(row, $"현재 골드 : {Game.Player.Gold:#,##0} G");
            row = Renderer.DrawTable(++row,
                shopModeToggle ? shopSaleItem : playerSaleItem,
                shopModeToggle ? formattersBuy : formattersSale,
                selectedOptionIndex);
            //if (shopModeToggle)
            //    row = Renderer.DrawItemList(++row, shopSaleItem, buyModeFormatters, selectedOptionIndex);
            //else
            //    row = Renderer.DrawItemList(++row, playerSaleItem, saleModeFormatters, selectedOptionIndex);
            Renderer.Print(row + 1, msg);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓ : 선택지 이동] [방향키 ← → : 구매/판매 모드 변경] [Enter : 선택] [ESC : 뒤로가기]");
        }

        #endregion

        void UpdateSaleListOptions()
        {
            saleModeOptions.Clear();
            for (int i = 0; i < playerSaleItem.Count; i++)
            {
                var saleItem = playerSaleItem[i];
                saleModeOptions.Add(new("", "", () =>
                {
                    if (saleItem.StackCount > 1)
                        saleItem.StackCount--;
                    else
                    {
                        Game.Player.Inventory.Remove(saleItem);
                        UpdateSaleListOptions();
                    }
                    Game.Player.ChangeGold((int)(saleItem.Price * 0.85));
                    msg = $"{saleItem.Name}을/를 팔았습니다.";
                }));
            }
            if (selectedOptionIndex > 0)
                selectedOptionIndex--;
        }

        #region Input

        protected override void OnCommandMoveLeft() {
            shopModeToggle = !shopModeToggle;
            UpdateSaleListOptions();
            selectedOptionIndex = 0;
        }

        protected override void OnCommandMoveRight() {
            shopModeToggle = !shopModeToggle;
            UpdateSaleListOptions();
            selectedOptionIndex = 0;
        }

        #endregion
    }
}