using System;
using System.Collections.Generic;
using System.Data;
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
            Random random = new Random();
            MonsterCount = random.Next(1, 5);
            for (int i = 0; i < MonsterCount; i++)
            {
                Creature monster;
                switch (random.Next(0, 3))
                {
                    case 0:
                        monster = new Monster("Slime", 10, 10, 2, 0);
                        Monsters.Add(monster);
                        break;
                    case 1:
                        monster = new Monster("Troll", 20, 20, 3, 0);
                        Monsters.Add(monster);
                        break;
                    case 2:
                        monster = new Monster("Hellhound", 30, 30, 5, 0);
                        Monsters.Add(monster);
                        break;
                }
            }
        }

        public void BattleStart()
        {
            while (!Player.IsDead() && !CheckAllMonstersDead())
            {
                PrintBattleState();
                // player 턴
                Console.WriteLine();
                //Console.WriteLine("0. hp포션사용");
                Console.WriteLine("공격할 몬스터를 선택해주세요.");
                int monsterNum = 0;

                // 입력 값 예외처리
                while (int.TryParse(Console.ReadLine(), out monsterNum) ||
                    monsterNum < 1 ||
                    monsterNum > MonsterCount ||
                    Monsters[monsterNum].IsDead())
                {
                    Console.WriteLine("공격할 몬스터를 다시 입력해주세요.");
                }
                Console.Clear();
                Player.Attack(Monsters[monsterNum]);
                PrintBattleState();
                Thread.Sleep(1000);

                if (CheckAllMonstersDead())
                    break;

                // monster 턴
                Monsters[monsterNum].Attack(Player);
                PrintBattleState();
                Thread.Sleep(1000);
            }

            // 전투 종료
            if (CheckAllMonstersDead())
            {
                OnCreatureDead?.Invoke(Monsters[0]);
            }
            else
            {
                OnCreatureDead?.Invoke(Player);
            }

        }

        public void PrintBattleState()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine();
            Console.WriteLine($"{Player.Name}의 Hp : {Player.Hp}");
            Console.WriteLine("------------------------------");
            for (int i = 0; i < Monsters.Count; i++)
            {
                var monster = Monsters[i];
                if (monster.IsDead())
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{i + 1}. {monster.Name} Dead");
                    Console.ResetColor();
                }
                else
                    Console.WriteLine($"{i + 1}. {monster.Name} Hp : {monster.Hp}");
            }
        }

        public bool CheckAllMonstersDead()
        {
            foreach(var monster in Monsters)
            {
                if (!monster.IsDead())
                    return false;
            }
            return true;
        }

        public void BattleEnd(Creature creature)
        {
            Console.Write("전투 종료");
        }
    }
}
