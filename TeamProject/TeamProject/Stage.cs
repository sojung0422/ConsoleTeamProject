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

        public List<Creature> MonsterSpawn() // 오태 수정
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
                Monster randomMonster = Game.Monsters[random.Next(0, Game.Monsters.Length)];
                Creature monster = new Monster(randomMonster); // 새로운 몬스터 인스터스 생성
                Monsters.Add(monster);
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
