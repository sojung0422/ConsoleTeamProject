using System.ComponentModel;

namespace TeamProject;

public class EquipmentScene : Scene
{
    public override string Title { get; protected set; } = "장 비 창";

    // 장비 씬에서 단계 구분
    private enum EquipStep
    {
        Show,       // 기본 모드
        Equipment,  // 관리 모드
    }

    private EquipStep step;
    private List<Item> gearList = new();
    private int selectionIdx;

    #region Scene

    public override void EnterScene()
    {
        step = EquipStep.Show;
        gearList = Game.Player.Inventory.Items.Where(item => item.Type == ItemType.Gear).ToList();  // 장비 아이템만 따로 리스트 복제
        selectionIdx = 0;

        // #1. 선택지 설정.
        Options.Clear();
        Options.Add(Managers.Scene.GetOption("Back"));
        Renderer.DrawBorder(Title);
    }

    public override void NextScene()
    {
        do
        {
            DrawStep();
        }
        while (ManageInput());
    }

    private void DrawStep()
    {

        if (step == EquipStep.Show)
        {
            int row = 4;
            row = Renderer.Print(row, "장 비 창 - 보 기");

            List<ItemTableFormatter> formatters = new()
            {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Stat"],
                Renderer.ItemTableFormatters["Desc"],
            };
            Renderer.DrawItemList(++row, gearList, formatters);
            Renderer.PrintKeyGuide("[Enter : 관리모드] [ESC : 뒤로가기]");
        }
        else
        {
            int row = 4;
            row = Renderer.Print(row, "장 비 창 - 관 리");

            List<ItemTableFormatter> formatters = new()
            {
                Renderer.ItemTableFormatters["Index"],
                Renderer.ItemTableFormatters["Name"],
                Renderer.ItemTableFormatters["ItemType"],
                Renderer.ItemTableFormatters["Stat"],
                Renderer.ItemTableFormatters["Desc"],
            };
            Renderer.DrawItemList(++row, gearList, formatters, selectionIdx);
            
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 장착] [ESC : 보기모드]");
        }
    }

    private bool ManageInput()
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

        if (step == EquipStep.Show)
        {
            OnShowCommand(commands);
        }
        else
        {
            OnEquipCommand(commands);
        }

        // TODO: 깔끔히 do-while 탈출을 못한 추후 최적화를 위해 수정이 필요할수도..
        return true;
    }

    void OnShowCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.Interact:
                step = EquipStep.Equipment;
                break;
            case Command.Exit:
                Options[0].Execute();
                break;
        }
    }

    void OnEquipCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                    selectionIdx--;
                break;
            case Command.MoveBottom:
                if (selectionIdx < gearList.Count - 1)
                    selectionIdx++;
                break;
            case Command.Interact:
                EquipFromInventory();
                break;
            case Command.Exit:
                step = EquipStep.Show;
                break;
        }
    }

    #endregion

    #region Equipment

    /// <summary>
    /// 인벤토리에서 아이템 장착
    /// </summary>
    private void EquipFromInventory()
    {
        var item = gearList.ElementAtOrDefault(selectionIdx);

        if (item is Gear gear)
        {
            switch (gear.GearType)
            {
                case GearType.Weapon:
                    Game.Player.Equipment.Equip(GearSlot.Weapon, gear);
                    break;
                case GearType.Shield:
                    Game.Player.Equipment.Equip(GearSlot.Shield, gear);
                    break;
                case GearType.Armor:
                    Game.Player.Equipment.Equip(GearSlot.Armor, gear);
                    break;
            }
        }
    }

    #endregion
}
