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
            }
            return Monsters;
        }

        public void Reward()
        {
            // 스테이지별 보상
            Console.WriteLine("보상 지급");
            StageLevel++;
        }
    }
}
