namespace TeamProject;

public class CreateCharacterScene : Scene
{
    private enum CreateStep
    {
        Name,
        Job,
        Exit
    }

    private Character[] players =
    {
        new Character("", "전사", 1, 10, 5, 100, 1500),
        new Character("", "마법사", 1, 6, 2, 80, 3000),
        new Character("", "도적", 1, 8, 4, 90, 5000)
    };
    private Character selectPlayer;

    public override string Title { get; protected set; } = "캐릭터 선택창";

    private CreateStep step = CreateStep.Name;
    private string createName = string.Empty;
    private string errorMessage = string.Empty;

    #region Scene

    public override void EnterScene()
    {
        do
        {
            DrawStep();
        }
        while (ManageInput());
    }

    public override void NextScene()
    {
        Managers.Scene.EnterScene<MainScene>();
    }

    #endregion

    #region Step

    private void DrawStep()
    {
        Renderer.DrawBorder(Title);
        Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");

        switch (step)
        {
            case CreateStep.Name:
                Renderer.Print(4, "원하시는 이름을 설정해 주세요");
                Renderer.Print(6, errorMessage);
                break;
            case CreateStep.Job:
                Renderer.Print(4, "원하시는 직업을 선택해 주세요");
                Renderer.Print(6, "0. 전사");
                Renderer.Print(7, "1. 마법사");
                Renderer.Print(8, "2. 도적");
                Renderer.Print(10, errorMessage);
                break;
        }
    }

    private void NextStep()
    {
        errorMessage = string.Empty;
        step += 1;

        if (step == CreateStep.Exit)
        {
            CreateCharacter();
        }
    }

    #endregion

    #region Read

    /// <summary>
    /// 이름 입력
    /// </summary>
    private void ReadName()
    {
        Console.SetCursorPosition(2, 7);
        var name = Console.ReadLine();
        OnNameChanged(name);
    }

    /// <summary>
    /// 직업 선택지 입력
    /// </summary>
    private void ReadJob()
    {
        Console.SetCursorPosition(2, 11);
        var job = Console.ReadLine();
        OnJobChanged(job);
    }

    #endregion

    #region OnValueChanged

    /// <summary>
    /// 올바른 이름을 입력했는지 체크하고, 이름을 변경합니다.
    /// </summary>
    /// <param name="name">입력한 이름</param>
    public void OnNameChanged(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            errorMessage = "오류: 이름을 입력해 주세요.";
            return;
        }
        
        if (Renderer.GetPrintingLength(name) > 10)
        {
            errorMessage = "오류: 이름이 너무 깁니다. 10글자 이내로 작성해 주세요.";
            return;
        }

        // 이름 결정
        createName = name;
        NextStep();
    }

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
        selectPlayer = players[idx];
        NextStep();
    }

    #endregion

    /// <summary>
    /// 완성된 캐릭터 생성
    /// </summary>
    private void CreateCharacter()
    {
        Program.player = new Character
        (
            createName, 
            selectPlayer.Job, 
            selectPlayer.Level, 
            (int)selectPlayer.DefaultDamage,
            (int)selectPlayer.DefaultDefense,
            (int)selectPlayer.DefaultHpMax,
            selectPlayer.Gold
        );
        Program.player.Inventory.Add(new Item(1, "Test01", "Test01아이템임니다", 10));
        Program.player.Inventory.Add(new Item(2, "Test02", "Test02아이템임니다", 10));
    }

    private bool ManageInput()
    {
        switch (step)
        {
            case CreateStep.Name:
                ReadName();
                break;
            case CreateStep.Job:
                ReadJob();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        bool isExit = (step != CreateStep.Exit);
        return isExit;
    }
}
