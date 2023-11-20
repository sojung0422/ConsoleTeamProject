namespace TeamProject;

public class CreateCharacterScene : Scene
{
    public override string Title { get; protected set; } = "캐릭터 선택창";

    private List<Character> jobList;


    private Character selectPlayer;

    private string createName = string.Empty;
    private string errorMessage = string.Empty;

    #region Scene

    public override void EnterScene()
    {
        jobList = new List<Character>();

        for(int i = 0; i < jobList.Count; i++) { }
        do
        {
            DrawScene();
        }
        while (ManageInput());
    }

    public override void NextScene()
    {
        Managers.Scene.EnterScene<MainScene>();
    }

    #endregion

    #region Step

    protected override void DrawScene()
    {
        
        Renderer.DrawBorder(Title);
        Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");

        Renderer.Print(4, "당신의 이름은 무엇인가요?");
        Renderer.Print(6, errorMessage);
        Renderer.Print(4, "직업이 어떻게 되십니까?");
        int row = 5;

        List<JobTableFormatter> formatters = new() {
            Renderer.JobTableFormatters["Job"],
            Renderer.JobTableFormatters["Damage"],
            Renderer.JobTableFormatters["Defense"],
            Renderer.JobTableFormatters["HpMax"],
            Renderer.JobTableFormatters["Critical"],
            Renderer.JobTableFormatters["Avoid"],
         };
        row = Renderer.DrawJobList(++row, Game.Characters, formatters);

        Renderer.Print(14, "1 > 전사");
        Renderer.Print(15, "2 > 마법사");
        Renderer.Print(16, "3 > 도적");
                
        Renderer.Print(20, errorMessage);
        Renderer.PrintOptions(++row, Options, true);
    }
    #endregion

    /// <summary>
    /// 직업 선택지 입력
    /// </summary>
    private void ReadJob()
    {
        Console.SetCursorPosition(2, 18);
        var job = Console.ReadLine();
        OnJobChanged(job);
    }


    #region OnValueChanged

    /// <summary>
    /// 올바른 직업을 선택했는지 체크하고, 직업을 선택합니다.
    /// </summary>
    /// <param name="job">선택한 직업 인덱스</param>
    public void OnJobChanged(string? job)
    {
        if (string.IsNullOrEmpty(job))
        {
            errorMessage = "오류: 숫자를 입력해 주세요.";
            return;
        }

        bool isInt = Int32.TryParse(job, out int idx);

        if (!isInt)
        {
            errorMessage = "오류: 숫자가 아닌 문자를 입력했습니다. 다시 입력해 주세요.";
            return;
        }

        if (idx < 0 || idx > 2)
        {
            errorMessage = "오류: 선택지 범위 내의 숫자를 입력해 주세요.";
            return;
        }

        // 직업 결정
        selectPlayer = Game.Characters[idx];
    }

    #endregion

    /// <summary>
    /// 완성된 캐릭터 생성
    /// </summary>
    private void CreateCharacter()
    {
        Game.Player = new Character
        (
            createName,
            selectPlayer.Job,
            selectPlayer.Level,
            (int)selectPlayer.DefaultDamage,
            (int)selectPlayer.DefaultDefense,
            (int)selectPlayer.DefaultHpMax,
            selectPlayer.Gold,
            selectPlayer.Critical,
            selectPlayer.Avoid
        );
    

        // [우진영] 상점에서 아이템이 잘 구매되는지 확인하기 위해
        // 기본 아이템만 넣어놨습니다.
        Game.Player.Inventory.Add(Game.Items[0]);
        Game.Player.Inventory.Add(Game.Items[1]);
        Game.Player.Inventory.Add(Game.Items[2]);
    }

    private bool ManageInput()
    {
        ReadJob();

        bool isExit;

        if (selectPlayer is null)
            isExit = true;
        else 
            isExit = false;

       
        return isExit;
    }
}
