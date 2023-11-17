namespace TeamProject;

public class Game
{
    public static Character player;
    //public static List<Gear> InvenGears = new List<Gear>();
    //private static int inputKey;

    public static Character[] characters =
    {
        new Character("", "전사", 1, 10, 5, 100, 1500, 0.1f, 0.1f),
        new Character("", "마법사", 1, 6, 2, 80, 3000, 0.2f, 0.2f),
        new Character("", "도적", 1, 8, 4, 90, 5000, 0.3f, 0.3f)
    };

    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        Renderer.Initialize();
        Managers.Scene.Initialize();
        Managers.Scene.EnterScene<CreateCharacterScene>();
    }
}
