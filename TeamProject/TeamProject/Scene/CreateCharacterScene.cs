namespace TeamProject;

public class CreateCharacterScene : Scene
{
    public override string Title { get; protected set; } = "캐릭터 선택창";

    private List<Item> jobList;
    private enum CreateStep
    {
        Name,
        Job,
        Exit
    }

    private Character selectPlayer;

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
                Renderer.Print(4, "당신의 이름은 무엇인가요?");
                Renderer.Print(6, errorMessage);
                break;
            case CreateStep.Job:
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

                //Renderer.Print(4, "원하시는 직업을 선택해 주세요");
                //Renderer.Print(6, "                  |전사          |마법사          |도적");
                //Renderer.Print(7, "================================================================");
                //Renderer.Print(8, $"공격력            |{Game.Characters[0].DefaultDamage}            |{Game.Characters[1].DefaultDamage}               |{Game.Characters[2].DefaultDamage}");
                //Renderer.Print(9, $"방어력            |{Game.Characters[0].DefaultDefense}             |{Game.Characters[1].DefaultDefense}               |{Game.Characters[2].DefaultDefense}");
                //Renderer.Print(10, $"체력              |{Game.Characters[0].DefaultHpMax}           |{Game.Characters[1].DefaultHpMax}              |{Game.Characters[2].DefaultHpMax}");
                //Renderer.Print(11, $"크리티컬 확률     |{Game.Characters[0].DefaultCritical * 100:0}%           |{Game.Characters[1].DefaultCritical * 100:0}%             |{Game.Characters[2].DefaultCritical * 100:0}%");
                //Renderer.Print(12, $"회피율            |{Game.Characters[0].DefaultAvoid * 100:0}%           |{Game.Characters[1].DefaultAvoid * 100:0}%             |{Game.Characters[2].DefaultAvoid * 100:0}%");
                //Renderer.Print(14, "1 > 전사");
                //Renderer.Print(15, "2 > 마법사");
                //Renderer.Print(16, "3 > 도적");
                //Renderer.Print(20, errorMessage);
                //Renderer.PrintKeyGuide("[원하는 직업의 번호를 입력해주세요]");
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
        Console.SetCursorPosition(2, 18);
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
        selectPlayer = Game.Characters[idx];
        NextStep();
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

        // 게임 클래스에 저장된 아이템 등록
        for (int i = 0; i < Game.Items.Length; i++)
        {
            Game.Player.Inventory.Add(Game.Items[i]);
        }
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
