using System.ComponentModel;

namespace TeamProject;

public class EquipmentScene : Scene
{
    public override string Title { get; protected set; } = "장 비 관 리";

    private List<Item> gearList;


    public override void EnterScene()
    {
        // 장비 아이템만 따로 리스트 복제
        gearList = Game.Player.Inventory.Items.Where(item => item.Type == ItemType.Gear).ToList();

        // #1. 선택지 설정.
        Options.Clear();
        Options.Add(Managers.Scene.GetOption("Back"));

        DrawScene();
    }

    public override void NextScene()
    {
        while (true)
        {
            DrawScene();
            var key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Escape) continue;
            Options[0].Execute();
            break;
        }
    }

    protected override void DrawScene()
    {
        Renderer.DrawBorder(Title);
        int row = 4;
        row = Renderer.Print(row, "이 곳은 장비관리");
        row = Renderer.Print(row, "장비 아이템을 장착 관리할 수 있습니다");

        List<ItemTableFormatter> formatters = new() {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Equip"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["Desc"],
            };
        row = Renderer.DrawItemList(++row, gearList, formatters);

        Renderer.PrintOptions(++row, Options, true);
        Renderer.PrintKeyGuide("[ESC : 뒤로가기]");
    }
}
