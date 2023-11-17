namespace TeamProject;

public class EquipmentScene : Scene
{
    public override string Title { get; protected set; } = "장 비 관 리";

    public override void EnterScene()
    {
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
            if (!int.TryParse(Console.ReadLine(), out int index)) continue;
            if (index < 0 || Options.Count < index) continue;
            Options[index].Execute();
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
                Renderer.ItemTableFormatters["Equip"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["Desc"],
            };
        row = Renderer.DrawItemList(++row, Game.Player.Inventory.Items, formatters);

        Renderer.PrintOptions(++row, Options, true);
    }
}
