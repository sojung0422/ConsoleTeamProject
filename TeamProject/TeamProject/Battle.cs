using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TeamProject
{
    public class Battle
    {
        public List<Creature> Monsters;
        public int MonsterCount;
        public Creature Player;

        public delegate void GameEvent(Creature creature);
        public event GameEvent OnCreatureDead;
        public Battle(Creature player)
        {
            Monsters = new List<Creature>();
            Player = player;

            OnCreatureDead += BattleEnd;

            // 몬스터 생성
            Monsters = Game.Stage.MonsterSpawn();
            MonsterCount = Monsters.Count;

            //Random random = new Random();
            //MonsterCount = random.Next(1, 5);
            //for (int i = 0; i < MonsterCount; i++)
            //{
            //    Creature monster;
            //    switch (random.Next(0, 3))
            //    {
            //        case 0:
            //            monster = new Monster("Slime", 10, 10, 2, 0, 0.2f, 0.2f);
            //            Monsters.Add(monster);
            //            break;
            //        case 1:
            //            monster = new Monster("Troll", 20, 20, 3, 0, 0.3f, 0.3f);
            //            Monsters.Add(monster);
            //            break;
            //        case 2:
            //            monster = new Monster("Hellhound", 30, 30, 5, 0, 0.2f, 0.2f);
            //            Monsters.Add(monster);
            //            break;
            //    }
            //}
        }

        public void BattleStart()
        {
            int turnCount = 0;
            int monsterAttackTurn = 0;
            while (!Player.IsDead() && !CheckAllMonstersDead())
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine($"스테이지 {Game.Stage.StageLevel}");
                Console.WriteLine($"{++turnCount}번째 턴");
                PrintBattleState();

                // player 턴
                Console.WriteLine();
                //Console.WriteLine("0. hp포션사용");
                Console.WriteLine("공격할 몬스터를 선택해주세요.");
                Renderer.PrintKeyGuide("[도망가기 : Q]");

                int monsterNum = -1;
                var input = Console.ReadLine();

                if (input == "q")
                    return;

                int.TryParse(input, out monsterNum);

                // 입력 값 예외처리
                while (monsterNum < 1 ||
                    monsterNum > MonsterCount ||
                    Monsters[monsterNum - 1].IsDead())
                {
                    Console.WriteLine("공격할 몬스터를 다시 입력해주세요.");
                    input = Console.ReadLine();
                    int.TryParse(input, out monsterNum);
                }
                Player.Attack(Monsters[monsterNum - 1]);
                Thread.Sleep(1000);

                if (CheckAllMonstersDead())
                    break;

                // monster 턴
                while (Monsters[monsterAttackTurn % MonsterCount].IsDead())
                    monsterAttackTurn++;
                Monsters[(monsterAttackTurn++) % MonsterCount].Attack(Player);
                Thread.Sleep(2000);
            }

            // 전투 종료
            if (CheckAllMonstersDead())
            {
                OnCreatureDead?.Invoke(Monsters[0]);
            }
            else if (Player.IsDead())
            {
                OnCreatureDead?.Invoke(Player);
            }

        }

        public void PrintBattleState()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine($"내 Hp : {Player.Hp}");
            Console.WriteLine("------------------------------");
            Console.WriteLine("몬스터");
            for (int i = 0; i < Monsters.Count; i++)
            {
                var monster = Monsters[i];
                if (monster.IsDead())
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{i + 1}. {monster.Name} Dead");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                    Console.WriteLine($"{i + 1}. {monster.Name}    Hp : {monster.Hp}");
            }
            Console.WriteLine("------------------------------");
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
                Console.WriteLine();
                Console.WriteLine("던전 클리어 !");

                // 클리어 보상(아이템, 골드, 레벨업 등)
                Game.Stage.Reward();


            }
            else
            {
                // 던전 클리어 실패
                Console.WriteLine();
                Console.WriteLine("던전 클리어 실패 !");

                /* 실패 보상(아이템, 골드, 레벨업 등)


                */
            }

            Console.WriteLine("아무 키나 입력하여 던전 나가기");
            Console.ReadLine();
            return;
        }
    }
}
