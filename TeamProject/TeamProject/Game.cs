namespace TeamProject;

public class Game
{
    public static Character player;
    public static List<Gear> InvenGears = new List<Gear>();
    private static int inputKey;

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
