using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace TeamProject;

public class BattleScene : Scene
{
    private MusicPlayer musicPlayer;
    public override string Title { get; protected set; } = $"지하 감옥";
    public List<Creature> Monsters;
    public int MonsterCount;
    public List<string> ActionTextList;
    public List<string> AttackTextList;

    protected int startTextLine;
    protected int line;

    public delegate void GameEvent(Creature creature);
    public event GameEvent OnCreatureDead;
    public BattleScene()
    {
        startTextLine = 4;
        Monsters = new List<Creature>();
        ActionTextList = new List<string>();
        AttackTextList = new List<string>();

        OnCreatureDead += BattleEnd;

        musicPlayer = new MusicPlayer();
    }
    public override async void EnterScene()
    {
        // 몬스터 생성
        Monsters = Game.Stage.MonsterSpawn();
        MonsterCount = Monsters.Count;

        ActionTextList.Clear();
        ActionTextList.Add("1. 몬스터 공격");
        ActionTextList.Add("2. 체력 회복");
        ActionTextList.Add("3. 던전 포기");

        AttackTextList.Clear();
        AttackTextList.Add("1. 기본 공격");
        AttackTextList.Add("2. 스킬[미구현]");

        musicPlayer.PlayAsync("BGM1.mp3", 0.01f); // 음악파일명, 볼륨

        DrawScene();
    }

    public override void NextScene()
    {
        musicPlayer.Stop();

        Renderer.PrintKeyGuide(new string(' ', Console.WindowWidth - 2));
        ClearBuffer();
        for (int count = 9; count > 0; count--)
        {
            Renderer.PrintKeyGuide($"[아무 키 : 던전 입구]   {count}초 뒤 던전 입구로 이동");
            Thread.Sleep(1000);

            if (Console.KeyAvailable)
            {
                Console.ReadKey(true);
                break;
            }
        }
        Managers.Scene.GetOption("Back").Execute();
    }
    protected override void DrawScene()
    {
        Renderer.DrawBorder(Title);
        // player : 7 ~ 11, monster : 15 ~ 26
        Renderer.Print(3, $"{Game.Stage.StageLevel} 스테이지", false, 0, Console.WindowWidth / 2 - 5);
        Renderer.Print(4, new string('-', 55), false, 0, Console.WindowWidth / 2);
        Renderer.Print(5, $"{Game.Player.Name}의 공격", false, 0, Console.WindowWidth / 2);
        Renderer.Print(6, new string('-', 55), false, 0, Console.WindowWidth / 2);
        Renderer.Print(12, new string('-', 55), false, 0, Console.WindowWidth / 2);
        Renderer.Print(13, $"몬스터의 공격", false, 0, Console.WindowWidth / 2);
        Renderer.Print(14, new string('-', 55), false, 0, Console.WindowWidth / 2);
        Renderer.Print(27, new string('-', 55), false, 0, Console.WindowWidth / 2);
        Renderer.PrintBattleText(startTextLine, Monsters, true, -1);
        Renderer.PrintKeyGuide(new string(' ', Console.WindowWidth - 2));
        while (!CheckAllMonstersDead() && !Game.Player.IsDead())
        {
            SelectAction();

            if (CheckAllMonstersDead())
                break;


            // 몬스터 턴
            line = 15;
            foreach (var monster in Monsters)
            {
                if (Game.Player.IsDead())
                    break;
                if (monster.IsDead())
                    continue;
                Thread.Sleep(1000);
                monster.Attack(Game.Player, line++);
                Renderer.PrintPlayerState(6);
            }
        }

        // 전투 종료
        if (CheckAllMonstersDead())
        {
            OnCreatureDead(Monsters[0]);
        }
        else if (Game.Player.IsDead())
        {
            OnCreatureDead(Game.Player);
        }

    }

    // ================= 키조작 관련 ================= //

    private int selectionIdx = 0;
    private int previousSelectionIdx = 0;
    private Stack<int> idxStack = new Stack<int>();

    public bool ManageInput(BattleAction action)
    {
        var key = Console.ReadKey(true);

        var commands = key.Key switch
        {
            ConsoleKey.UpArrow => Command.MoveTop,
            ConsoleKey.DownArrow => Command.MoveBottom,
            ConsoleKey.Enter => Command.Interact,
            ConsoleKey.Escape => Command.Exit,
            _ => Command.Nothing
        };
        switch (action)
        {
            case BattleAction.SelectAction:
                ActionOnCommand(commands);
                break;
            case BattleAction.SelectSkill:
                // 스킬 사용
                SkillOnCommand(commands);
                break;
            case BattleAction.SelectAttack:
                // 공격
                SelectAttackOnCommand(commands);
                break; ;
            case BattleAction.Attack:
                AttackOnCommand(commands);
                break;
            case BattleAction.UsePotion:
                // 포션 사용
                bool isPotionNotUsed = true;
                UsePotionOnCommand(commands, ref isPotionNotUsed);
                if (commands == Command.Interact)
                    return isPotionNotUsed;
                break;
        }
        // 첫 화면과 몬스터 공격에선 esc기능을 제외
        if ((action == BattleAction.SelectAction) || (action == BattleAction.Attack))
            return commands != Command.Interact;
        return commands != Command.Interact && commands != Command.Exit;
    }

    private void ActionOnCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                {
                    --selectionIdx;
                }
                break;
            case Command.MoveBottom:
                if (selectionIdx < ActionTextList.Count - 1)
                {
                    ++selectionIdx;
                }
                break;
            case Command.Interact:
                if (selectionIdx == 0)
                {
                    IdxStackPush();
                    SelectAttack();
                }
                else if (selectionIdx == 1)
                {
                    IdxStackPush();
                    UsePotion();
                }
                else if (selectionIdx == 2)
                {
                    musicPlayer.Stop();
                    Managers.Scene.GetOption("Back").Execute();
                }
                break;
        }
    }

    private void SelectAttackOnCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                {
                    --selectionIdx;
                }
                break;
            case Command.MoveBottom:
                if (selectionIdx < AttackTextList.Count - 1)
                {
                    ++selectionIdx;
                }
                break;
            case Command.Interact:
                if (selectionIdx == 0)
                {
                    IdxStackPush();
                    MonsterAttack();
                }
                else
                {
                    IdxStackPush();
                    SelectSkill();
                }
                break;
            case Command.Exit:
                IdxStackPop();
                SelectAction();
                break;
        }
    }

    private void AttackOnCommand(Command cmd)
    {
        int tempSelectionIdx;
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                {
                    tempSelectionIdx = selectionIdx;
                    --selectionIdx;
                    while (Monsters[selectionIdx].IsDead())
                    {
                        if (--selectionIdx < 0)
                        {
                            selectionIdx = tempSelectionIdx;
                            break;
                        }
                    }
                }
                break;
            case Command.MoveBottom:
                if (selectionIdx < Monsters.Count - 1)
                {
                    tempSelectionIdx = selectionIdx;
                    ++selectionIdx;
                    while (Monsters[selectionIdx].IsDead())
                    {
                        if (++selectionIdx > Monsters.Count - 1)
                        {
                            selectionIdx = tempSelectionIdx;
                            break;
                        }
                    }
                }
                break;
            case Command.Interact:
                Game.Player.Attack(Monsters[selectionIdx], 7);
                break;
                /*  // 던전포기 -> ActionOnCommand로 이동
                    case Command.Exit:
                    // 던전 포기 패널티
                    Game.Player.Hp -= 20;
                    Managers.Scene.GetOption("Back").Execute();
                    break;
                */
        }
    }

    private void SkillOnCommand(Command cmd)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                {
                    --selectionIdx;
                }
                break;
            case Command.MoveBottom:
                // TODO: 스킬개수 Count - 1 로 변경
                if (selectionIdx < ActionTextList.Count - 1)
                {
                    ++selectionIdx;
                }
                break;
            case Command.Interact:
                switch (selectionIdx)
                {
                    case 0:
                        // 0번 스킬 사용
                        break;
                    case 1:
                        // 1번 스킬 사용
                        break;
                }
                break;
            case Command.Exit:
                IdxStackPop();
                SelectAttack();
                break;
        }
    }
    private void UsePotionOnCommand(Command cmd, ref bool isPotionNotUsed)
    {
        switch (cmd)
        {
            case Command.MoveTop:
                if (selectionIdx > 0)
                {
                    --selectionIdx;
                }
                break;
            case Command.MoveBottom:
                if (selectionIdx < 2)
                {
                    ++selectionIdx;
                }
                break;
            case Command.Interact:
                switch (selectionIdx)
                {
                    case 0:
                        // hp 포션 사용
                        if (Game.Player.Inventory.HasSameItem(Game.Items[6], out var hpPotion))
                        {
                            // 포션이 있을 때
                            Renderer.ClearLine(startTextLine, 30);
                            if (Game.Player.Hp >= Game.Player.HpMax)
                                Renderer.Print(startTextLine, $"이미 체력이 최대입니다!");
                            else
                            {
                                hpPotion.Use(Game.Player);
                                Renderer.Print(startTextLine, $"HP 포션을 사용했습니다!");
                                isPotionNotUsed = false;
                            }
                        }
                        else
                        {
                            // 포션이 없을 때
                            Renderer.ClearLine(startTextLine, 30);
                            Renderer.Print(startTextLine, $"HP 포션이 부족합니다!");
                        }
                        break;
                    case 1:
                        // mp 포션 사용
                        if (Game.Player.Inventory.HasSameItem(Game.Items[7], out var mpPotion))
                        {
                            // 포션이 있을 때
                            Renderer.ClearLine(startTextLine, 30);
                            if (Game.Player.Mp >= Game.Player.MpMax)
                                Renderer.Print(startTextLine, $"이미 마나가 최대입니다!");
                            else
                            {
                                mpPotion.Use(Game.Player);
                                Renderer.Print(startTextLine, $"MP 포션을 사용했습니다!");
                                isPotionNotUsed = false;
                            }
                        }
                        else
                        {
                            // 포션이 없을 때
                            Renderer.ClearLine(startTextLine, 30);
                            Renderer.Print(startTextLine, $"MP 포션이 부족합니다!");
                        }
                        break;
                }
                break;
            case Command.Exit:
                IdxStackPop();
                SelectAction();
                break;
        }
    }
    // ============================================ //

    // ================= 전투 관련 ================= //

    public void SelectAction()
    {
        idxStack.Clear();
        Renderer.Print(startTextLine, $"원하는 행동을 선택해주세요.");
        Renderer.PrintKeyGuide("[방향키 ↑ ↓: 이동] [Enter: 선택] [ESC: 뒤로가기]");
        do
        {
            Renderer.PrintSelectAction(startTextLine, ActionTextList, true, selectionIdx);
            ClearBuffer();
        }
        while (ManageInput(BattleAction.SelectAction));
    }

    public void MonsterAttack()
    {
        // 공격
        selectionIdx = 0;
        while (Monsters[selectionIdx].IsDead())
            selectionIdx++;

        // 선택한 옵션 색 초기화
        Renderer.PrintSelectAction(startTextLine, AttackTextList, true, -1);
        Renderer.PrintBattleText(startTextLine, Monsters, true, -1);

        do
        {
            Renderer.PrintBattleText(startTextLine, Monsters, true, selectionIdx);
            ClearBuffer();
        }
        while (ManageInput(BattleAction.Attack));
    }

    public void SelectAttack()
    {
        Renderer.Print(startTextLine, $"공격할 방법을 선택해주세요.");
        do
        {
            Renderer.PrintSelectAction(startTextLine, AttackTextList, true, selectionIdx);
            ClearBuffer();
        }
        while (ManageInput(BattleAction.SelectAttack));
    }

    public void SelectSkill()
    {
        // 스킬 구현 후 구현할 예정
    }

    public void UsePotion()
    {
        Renderer.Print(startTextLine, $"사용할 포션을 선택해주세요.");
        int? hpPotionCount;
        int? mpPotionCount;

        do
        {
            if (Game.Player.Inventory.HasSameItem(Game.Items[6], out var hpPotion))
                hpPotionCount = (hpPotion.StackCount == null) ? 0 : hpPotion.StackCount;
            else
                hpPotionCount = 0;

            if (Game.Player.Inventory.HasSameItem(Game.Items[7], out var mpPotion))
                mpPotionCount = (mpPotion.StackCount == null) ? 0 : mpPotion.StackCount;
            else
                mpPotionCount = 0;

            List<string> potionStateList = new List<string>
        {
            $"1. HP 포션 : {hpPotionCount}개",
            $"2. MP 포션 : {mpPotionCount}개"
        };
            Renderer.PrintSelectAction(startTextLine, potionStateList, true, selectionIdx);
            ClearBuffer();
        }
        while (ManageInput(BattleAction.UsePotion));
        Renderer.PrintPlayerState(6);
    }

    public bool CheckAllMonstersDead()
    {
        foreach (var monster in Monsters)
        {
            if (!monster.IsDead())
                return false;
        }
        return true;
    }

    public void BattleEnd(Creature creature)
    {
        // TODO: 전투 종료
        if (creature is Monster)
        {
            // 던전 클리어
            Renderer.Print(12 + MonsterCount, "던전 클리어!");

            // 클리어 보상(아이템, 골드, 레벨업 등)
            Game.Stage.ClearReward();


        }
        else
        {
            // 던전 클리어 실패
            Renderer.Print(12 + MonsterCount, "던전 클리어 실패!");

            //실패 보상(아이템, 골드, 레벨업 등)
            Game.Stage.FailReward();


        }
    }

    // ============================================ //

    public void ClearBuffer()
    {
        while (Console.KeyAvailable) // 버퍼에 입력이 있는 경우 처리
        {
            Console.ReadKey(true); // 입력을 읽고 버퍼를 비움 
        }
    }

    public void IdxStackPush()
    {
        idxStack.Push(selectionIdx);
        selectionIdx = 0;
    }
    public void IdxStackPop()
    {
        selectionIdx = idxStack.Pop();
    }
}

