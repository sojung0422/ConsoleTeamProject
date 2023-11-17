namespace TeamProject;

public static class Extensions
{
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
}
