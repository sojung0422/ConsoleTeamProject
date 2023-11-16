namespace TeamProject;

// 장착 슬롯 타입
public enum EquipSlot
{
    Weapon,
    Armor,
}

public class Equipment
{
    private Dictionary<EquipSlot, Gear> equipped = new();
    
    // 아이템의 이름이 빈 문자열인 경우 => true
    private bool IsEmptyItem(Gear gear) => string.IsNullOrEmpty(gear.Name);

    public Equipment()
    {
        var slots = Enum.GetValues<EquipSlot>();

        foreach (var slot in slots)
        {
            if (equipped.GetValueOrDefault(slot) != null) continue;
            equipped[slot] = Gear.Empty;
        }
    }

    public void Equip(EquipSlot slot, Gear itemEquip)
    {
        equipped.TryGetValue(slot, out var item);

        // 같은 장비를 착용중 인가?
        if (item == itemEquip)
        {
            Unequip(slot);
            return;
        }

        // 해당 장비창이 비어있지 않은가?
        if (!IsEmptyItem(equipped[slot]))
        {
            Unequip(slot);
        }

        equipped[slot] = itemEquip;
    }

    public void Unequip(EquipSlot slot)
    {
        equipped[slot] = Gear.Empty;
    }
}
