namespace TeamProject;

public class CreateCharacterScene : Scene
{
    public override string Title { get; protected set; } = "캐릭터 선택창";

    private enum CreateStep
    {
        Name,
        Job,
        CreateCharacter,
        Exit
    }

    private Character selectPlayer;
    private List<JobTableFormatter> formatters;
    private CreateStep step = CreateStep.Name;
    private string createName = string.Empty;
    private string errorMessage = string.Empty;

    #region Scene

    public override void EnterScene() {
        step = CreateStep.Name;
        formatters = new() {
                Renderer.JobTableFormatters["Job"],
                Renderer.JobTableFormatters["Damage"],
                Renderer.JobTableFormatters["Defense"],
                Renderer.JobTableFormatters["HpMax"],
                Renderer.JobTableFormatters["MpMax"],
                Renderer.JobTableFormatters["Critical"],
                Renderer.JobTableFormatters["Avoid"],
            };
        DrawScene();
    }

    public override void NextScene() {
        while (step == CreateStep.Name) {
            DrawStep();
            ReadName();
        }
        while (step == CreateStep.Job) {
            DrawStep();
            GetInput();
        }
    }
    protected override void DrawScene() {
        Renderer.DrawBorder(Title);
        Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");
    }
    
    private void DrawStep() {
        switch (step) {
            case CreateStep.Name:
                Renderer.ClearLine(4);
                Renderer.Print(4, "당신의 이름은 무엇인가요?");
                Renderer.ClearLine(6);
                Renderer.Print(6, errorMessage);
                Renderer.PrintKeyGuide("[Enter: 결정]");
                break;
            case CreateStep.Job:
                Renderer.Print(4, "직업이 어떻게 되십니까?");
                int row = 5;
                row = Renderer.DrawJobList(row, Game.Characters, formatters, selectedOptionIndex) + 1;
                Renderer.ClearLine(row);
                Renderer.Print(row, errorMessage);
                Renderer.PrintKeyGuide("[Enter: 결정]");
                break;
        }
    }

    #endregion

    #region Step

    private void NextStep()
    {
        errorMessage = string.Empty;
        step += 1;

        if (step == CreateStep.CreateCharacter)
        {
            CreateCharacter();
        }
        else if (step == CreateStep.Exit) {
            Managers.Scene.EnterScene<MainScene>();
        }
    }

    #endregion

    #region Input

    protected override void OnCommandMoveTop() {
        if (step == CreateStep.Job && selectedOptionIndex > 0)
            selectedOptionIndex--;
    }
    protected override void OnCommandMoveBottom() {
        if (step == CreateStep.Job && selectedOptionIndex < Game.Characters.Length - 1)
            selectedOptionIndex++;
    }
    protected override void OnCommandInteract() {
        if (step == CreateStep.Job) {
            ReadJob();
        }
    }
    protected override void OnCommandExit() {
        
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

        selectPlayer = Game.Characters[selectedOptionIndex];
        NextStep();
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
            (int)selectPlayer.DefaultMpMax,
            selectPlayer.Gold,
            selectPlayer.Critical,
            selectPlayer.Avoid,
            selectPlayer.PlayerSkill
        );

        //게임 클래스에 저장된 아이템 등록
        //for (int i = 0; i < Game.Items.Length; i++)
        //{
        //    Game.Player.Inventory.Add(Game.Items[i]);
        //}

        // [우진영] 상점에서 아이템이 잘 구매되는지 확인하기 위해
        // 기본 아이템만 넣어놨습니다.
        Gear basicWeapon = Game.Items[0].DeepCopy() as Gear;
        Gear basicShield = Game.Items[1].DeepCopy() as Gear;
        Gear basicArmor = Game.Items[2].DeepCopy() as Gear;
        Game.Player.Inventory.Add(basicWeapon);
        Game.Player.Inventory.Add(basicShield);
        Game.Player.Inventory.Add(basicArmor);
        Game.Player.Equipment.Equip((GearSlot)basicWeapon.GearType, basicWeapon);
        Game.Player.Equipment.Equip((GearSlot)basicShield.GearType, basicShield);
        Game.Player.Equipment.Equip((GearSlot)basicArmor.GearType, basicArmor);

        Managers.Game.data.character = Game.Player;
        Managers.Game.data.stage = Game.Stage;
        Managers.Game.SaveGame();

        NextStep();
    }

    //private bool ManageInput()
    //{
    //    switch (step)
    //    {
    //        case CreateStep.Name:
    //            ReadName();
    //            break;
    //        case CreateStep.Job:
    //            ReadJob();
    //            break;
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }

    //    bool isExit = (step != CreateStep.Exit);
    //    return isExit;
    //}
}
