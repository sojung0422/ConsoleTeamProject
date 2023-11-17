namespace TeamProject;

// 장착 슬롯 타입
public enum GearSlot
{
    Weapon,
    Shield,
    Armor,
}

public class Equipment
{
    private Dictionary<GearSlot, Gear> equipped = new();
    public IReadOnlyDictionary<GearSlot, Gear> Equipped => equipped;

    public Equipment()
    {
        var slots = Enum.GetValues<GearSlot>();

        foreach (var slot in slots)
        {
            if (equipped.GetValueOrDefault(slot) != null) continue;
            equipped[slot] = Gear.Empty;
        }
    }

    public void Equip(GearSlot slot, Gear gear)
    {
        equipped.TryGetValue(slot, out var item);

        // 같은 장비를 착용중 인가?
        if (item == gear)
        {
            Unequip(slot);
            return;
        }

        // 해당 장비창이 비어있지 않은가?
        if (!equipped[slot].IsEmptyItem())
        {
            Unequip(slot);
        }

        equipped[slot] = gear;
        StatAdd(equipped[slot]);
        equipped[slot].IsEquip = true;
    }

    public void Unequip(GearSlot slot)
    {
        StatSubtract(equipped[slot]);
        equipped[slot].IsEquip = false;
        equipped[slot] = Gear.Empty;
    }

    public void StatAdd(Gear gear)
    {
        Game.Player.DefaultDamage += gear.Atk;
        Game.Player.DefaultDefense += gear.Def;
        Game.Player.DefaultCritical += (gear.Crit * 0.01f);
        Game.Player.DefaultAvoid += (gear.Dodge * 0.01f);
    }

    public void StatSubtract(Gear gear)
    {
        Game.Player.DefaultDamage -= gear.Atk;
        Game.Player.DefaultDefense -= gear.Def;
        Game.Player.DefaultCritical -= (gear.Crit * 0.01f);
        Game.Player.DefaultAvoid -= (gear.Dodge * 0.01f);
    }
}
