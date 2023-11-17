namespace TeamProject;

public static class Extensions
{
    // 아이템의 이름이 빈 문자열인 경우 => true
    public static bool IsEmptyItem(this Gear gear) => string.IsNullOrEmpty(gear.Name);

    // 아이템 타입을 문자로 리턴
    public static string String(this ItemType type)
    {
        return type switch
        {
            ItemType.Gear => "장비",
            ItemType.ConsumeItem => "소모품",
            _ => ""
        };
    }

    // 장비 타입을 문자로 리턴
    public static string String(this GearType type)
    {
        return type switch
        {
            GearType.Weapon => "무기",
            GearType.Shield => "방패",
            GearType.Armor => "방어구",
            _ => ""
        };
    }

    public static string StatToString(this Gear gear)
    {
        string stat = string.Empty;

        if (gear.Atk != 0) 
        {
            stat += $"ATK {gear.Atk} ";
        }
        
        if (gear.Def != 0)
        {
            stat += $"DEF {gear.Def} ";
        }
        
        if (gear.Crit != 0)
        {
            stat += $"CRIT {gear.Crit}% ";
        }
        
        if (gear.Dodge != 0)
        {
            stat += $"DODGE {gear.Dodge}%";
        }

        return stat;
    }
}
