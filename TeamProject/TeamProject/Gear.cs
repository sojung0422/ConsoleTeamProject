namespace TeamProject;

public enum GearType
{
    Weapon,
    Shield,
    Armor,
    None,
}

public class Gear : Item
{
    public GearType GearType { get; set; }
    public int GearState { get; set; }
    public bool IsEquip { get; set; }

    public Gear(int id, string name, string description, int price, GearType gearType, int state, ItemType itemType = ItemType.Gear, bool isEquip = false) : base(id, name, description, price, itemType)
    {
        GearType = gearType;
        GearState = state;
        IsEquip = isEquip;
    }

    public Gear(Gear reference) : base(reference) {
        GearType = reference.GearType;
        GearState = reference.GearState;
        IsEquip = reference.IsEquip;
    }

    // [박상원]
    // 장비 슬롯이 비어있는 경우, 빈 아이템 생성
    public static Gear Empty = new(-1, string.Empty, string.Empty, 0, GearType.None, 0);
    public override Item DeepCopy() => new Gear(this);
}
