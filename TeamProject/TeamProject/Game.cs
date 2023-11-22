namespace TeamProject;

public class Game
{
    public static Character Player { get; set; }
    //public static Monster Monster { get; set; }

    public static Stage Stage = new Stage();
    // name, job, level, damage, defense, hp, gold, critical, avoid
    public static Character[] Characters =
    {
        new Character("", "전사", 1, 10, 5, 100, 50, 1500, 0.15f, 0.10f),
        new Character("", "마법사", 1, 6, 2, 80, 100, 3000, 0.1f, 0.2f),
        new Character("", "도적", 1, 8, 4, 90, 70, 5000, 0.35f, 0.30f)
    };
    // name, hp, damage, defense, mp, critical, avoid
    public static Monster[] Monsters =
    {
        new Monster("슬라임", 10, 10, 2, 0, 0.2f, 0.2f),
        new Monster("트롤", 20, 20, 3, 0, 0.3f, 0.3f),
        new Monster("헬하운드", 30, 30, 5, 0, 0.2f, 0.2f)
    };
    // id, name, description, price, type, stackCount
    public static Item[] Items =
    {
        new Gear(1, "철제 검", "공격에 사용되는 무기", 1500, GearType.Weapon, 12, 0, 0, 0),
        new Gear(2, "원형 방패", "적의 공격을 방어하는 방패", 1000, GearType.Shield, 0, 6, 0, 0),
        new Gear(3, "사슬 갑옷", "철사 고리를 엮어 만든 갑옷", 2000, GearType.Armor, 0, 3, 0, 5),
        new Gear(4, "다이아몬드 검", "더 쎈 무기", 3000, GearType.Weapon, 33, 0, 12, 0),
        new Gear(5, "다이아몬드 방패", "더 단단한 방패", 2000, GearType.Shield, 0, 15, 0, 0),
        new Gear(6, "다이아몬드 갑옷", "딴딴 갑옷 ", 4000, GearType.Armor, 8, 8, 8, 8),
        new HealingPotion(7, "체력 포션", "체력을 회복하는 물약", 150, 1, 50),
        new ManaPotion(8, "마나 포션", "마나를 회복하는 물약", 150, 1, 50),
    };
  
    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        Renderer.Initialize();
        Managers.Game.Initialize();
        Managers.Scene.Initialize();
        Managers.Scene.EnterScene<TitleScene>();
    }
}
