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
    public float Atk { get; set; }    // 공격력
    public float Def { get; set; }    // 방어력
    public float Crit { get; set; }   // 치명타
    public float Dodge { get; set; }    // 회피율
    public bool IsEquip { get; set; }

    public Gear(int id, string name, string description, int price, GearType gearType, float atk, float def, float crit, float dodge, ItemType itemType = ItemType.Gear, bool isEquip = false) : base(id, name, description, price, itemType)
    {
        GearType = gearType;
        Atk = atk;
        Def = def;
        Crit = crit;
        Dodge = dodge;
        IsEquip = isEquip;
        OnRemoved += (owner) => { if (owner.Equipment.Equipped[(GearSlot)GearType] == this) owner.Equipment.Unequip((GearSlot)GearType); };
    }

    public Gear(Gear reference) : base(reference) {
        GearType = reference.GearType;
        Atk = reference.Atk;
        Def = reference.Def;
        Crit = reference.Crit;
        Dodge = reference.Dodge;
        IsEquip = reference.IsEquip;
        OnRemoved += (owner) => { if (owner.Equipment.Equipped[(GearSlot)GearType] == this) owner.Equipment.Unequip((GearSlot)GearType); };
    }

    // [박상원]
    // 장비 슬롯이 비어있는 경우, 빈 아이템 생성
    public static Gear Empty = new(-1, string.Empty, string.Empty, 0, GearType.None, 0, 0, 0, 0, 0);
    public override Item DeepCopy() => new Gear(this);
}
