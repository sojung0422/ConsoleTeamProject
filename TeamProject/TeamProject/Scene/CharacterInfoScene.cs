namespace TeamProject;

public class CharacterInfoScene : Scene 
{
    public override string Title { get; protected set; } = "상 태 보 기";
    public override void EnterScene() 
    {
        Options.Clear();
        Options.Add(Managers.Scene.GetOption("Back"));
        DrawScene();
    }

    public override void NextScene()
    {
        while (true) 
        {
            DrawScene();
            if (!int.TryParse(Console.ReadLine(), out int index)) continue;
            if (index < 0 || Options.Count < index) continue;
            Options[index].Execute();
            break;
        }
    }
    protected override void DrawScene() 
    {
        Renderer.DrawBorder(Title);
        Renderer.Print(3, "캐릭터의 정보가 표시됩니다.");

        // ==== 캐릭터 정보 표시 ====

        // 캐릭터 잘 생성됬는지 임시 출력 [박상원]
        Renderer.Print(5, $"Lv. {Game.Player.Level}");
        Renderer.Print(6, $"{Game.Player.Name} ( {Game.Player.Job} )");
        Renderer.Print(7, $"공격력 : {Game.Player.DefaultDamage}");
        Renderer.Print(8, $"방어력 : {Game.Player.DefaultDefense}");
        Renderer.Print(9, $"체 력 : {Game.Player.DefaultHpMax}");
        Renderer.Print(10, $"Gold : {Game.Player.Gold} G");

        Renderer.PrintOptions(12, Options, true);
    }
}
