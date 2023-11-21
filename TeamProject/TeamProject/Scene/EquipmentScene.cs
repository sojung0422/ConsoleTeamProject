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

    #region Scene

    public override void EnterScene()
    {
        step = EquipStep.Show;
        gearList = Game.Player.Inventory.Items.Where(item => item.Type == ItemType.Gear).ToList();  // 장비 아이템만 따로 리스트 복제
        selectedOptionIndex = 0;

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
            GetInput();
        }
        while (Managers.Scene.CurrentScene is EquipmentScene);
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
                Renderer.ItemTableFormatters["Effect"],
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
            Renderer.DrawItemList(++row, gearList, formatters, selectedOptionIndex);
            
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 장착] [ESC : 보기모드]");
        }
    }

    #endregion

    #region Input

    protected override void OnCommandMoveTop() {
        if (step == EquipStep.Equipment && selectedOptionIndex > 0)
            selectedOptionIndex--;
    }
    protected override void OnCommandMoveBottom() {
        if (step == EquipStep.Equipment && selectedOptionIndex < gearList.Count - 1)
            selectedOptionIndex++;
    }
    protected override void OnCommandInteract() {
        if (step == EquipStep.Show) step = EquipStep.Equipment;
        else if (step == EquipStep.Equipment) EquipFromInventory();
    }
    protected override void OnCommandExit() {
        if (step == EquipStep.Show) Options[0].Execute();
        else if (step == EquipStep.Equipment) step = EquipStep.Show;
    }

    #endregion

    #region Equipment

    /// <summary>
    /// 인벤토리에서 아이템 장착
    /// </summary>
    private void EquipFromInventory()
    {
        var item = gearList.ElementAtOrDefault(selectedOptionIndex);

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
