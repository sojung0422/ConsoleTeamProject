using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject;

public class BattleScene : Scene
{
    public override string Title { get; protected set; } = $"지하 감옥";
    public List<Creature> Monsters;
    public int MonsterCount;
    protected int line;

    public delegate void GameEvent(Creature creature);
    public event GameEvent OnCreatureDead;
    public BattleScene()
    {
        Monsters = new List<Creature>();

        OnCreatureDead += BattleEnd;
    }
    public override void EnterScene()
    {
        // 몬스터 생성
        Monsters = Game.Stage.MonsterSpawn();
        MonsterCount = Monsters.Count;
        DrawScene();
    }

    public override void NextScene()
    {
        Renderer.PrintKeyGuide(new string(' ', Console.WindowWidth - 2));
        while (Console.KeyAvailable) // 버퍼에 입력이 있는 경우 처리
        {
            Console.ReadKey(true); // 입력을 읽고 버퍼를 비움
        }
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
        while (!CheckAllMonstersDead() && !Game.Player.IsDead())
        {
            selectionIdx = 0;
            while (Monsters[selectionIdx].IsDead())
                selectionIdx++;
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 공격할 몬스터 이동] [Enter: 선택] [ESC: 던전 포기]");

            do
            {
                Renderer.PrintBattleText(3, Monsters, true, selectionIdx);
                while (Console.KeyAvailable) // 버퍼에 입력이 있는 경우 처리
                {
                    Console.ReadKey(true); // 입력을 읽고 버퍼를 비움
                }
            }
            while (ManageInput());
            Renderer.PrintBattleText(3, Monsters, true, selectionIdx);

            if (CheckAllMonstersDead())
                break;

            // 몬스터 턴
            line = 7;
            foreach (var monster in Monsters)
            {
                if (Game.Player.IsDead())
                    break;
                if (monster.IsDead())
                    continue;
                Thread.Sleep(1000);
                monster.Attack(Game.Player, line++);
                Renderer.PrintBattleText(3, Monsters, true, selectionIdx);
            }

            for (int i = 0; i < Monsters.Count; i++)
            {
                Creature monster = Monsters[i];
                int statusBarLength = 20;  // 상태 바의 길이

                // HP의 백분율 계산
                int hpPercentage = (int)((double)monster.Hp / monster.DefaultHpMax * statusBarLength);

                // HP 상태 바 출력
                Console.ForegroundColor = ConsoleColor.Red;
                Renderer.Print(5 + i, $"몬스터 {i + 1}: [{new string('■', hpPercentage)}{new string(' ', statusBarLength - hpPercentage)}] {monster.Hp}/{monster.DefaultHpMax}", false, 3, 0);
            }

            Renderer.Print(++line, $"공격할 몬스터를 선택해주세요.", false, 0, Console.WindowWidth / 2);
        }
        Renderer.PrintBattleText(3, Monsters, true, -1);

        

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


    private int selectionIdx = 0;

    public bool ManageInput()
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

        OnCommand(commands);

        return commands != Command.Interact;
    }

    private void OnCommand(Command cmd)
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
                Game.Player.Attack(Monsters[selectionIdx], 5);
                break;
            case Command.Exit:
                Managers.Scene.GetOption("Back").Execute();
                break;

        }
    }

    // ================= 전투 관련 ================= //

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
            Game.Stage.Reward();


        }
        else
        {
            // 던전 클리어 실패
            Renderer.Print(12 + MonsterCount, "던전 클리어 실패!");

            //실패 보상(아이템, 골드, 레벨업 등)
            Game.Stage.Reward();


        }
    }

    // ============================================ //
}

