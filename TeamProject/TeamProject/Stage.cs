using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    public class Stage
    {
        public int StageLevel { get; set; }
        public int MonsterMinCount;
        public int MonsterMaxCount;
        public int MonsterCount;

        public Stage()
        {
            StageLevel = 1;
            MonsterMinCount = 1;
            MonsterMaxCount = 4;
        }
        public Monster GetRandomMonster() // 오태
        {
            Random random = new Random();
            return Game.Monsters[random.Next(0, Game.Monsters.Length)];
        }

        public List<Creature> MonsterSpawn()
        {
            MonsterMinCount += StageLevel / 10;
            MonsterMaxCount += StageLevel / 10;
            Random random = new Random();
            MonsterCount = random.Next(MonsterMinCount, MonsterMaxCount);
            // 스테이지별 몬스터 소환
            List<Creature> Monsters = new List<Creature>();
            // 몬스터 생성
            for(int i = 0; i < MonsterCount; i++)
            {
                Creature monster = GetRandomMonster();
                Monsters.Add(monster);
                //Creature monster;
                /*switch (random.Next(0, 3))
                {
                    case 0:
                        monster = new Monster("Slime", 10, 10, 2, 0, 0.2f, 0.2f);
                        Monsters.Add(monster);
                        break;
                    case 1:
                        monster = new Monster("Troll", 20, 20, 3, 0, 0.3f, 0.3f);
                        Monsters.Add(monster);
                        break;
                    case 2:
                        monster = new Monster("Hellhound", 30, 30, 5, 0, 0.2f, 0.2f);
                        Monsters.Add(monster);
                        break;
                }*/
            }
            return Monsters;
        }

        public void Reward()
        {
            // 스테이지 보상
            //아이템

            //경험치 : 스테이지*20
            int expReward = StageLevel * 100;
            Game.Player.ChangeExp(expReward);

            // 스테이지 1 증가
            StageLevel++;
        }
    }
}
