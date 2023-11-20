using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TeamProject {
    public class Monster : Creature 
    {
        //public string 처치보상;
        public override float Hp
        {
            get => hp;
            set
            {
                if (value <= 0) hp = 0;
                else hp = value;
            }
        }
        public Monster(string name, float hp, float damage, float defense, float mp, float critical, float avoid)
        {
            Name = name;
            Hp = hp;
            DefaultHpMax = hp;
            DefaultDamage = damage;
            DefaultDefense = defense;
            DefaultCritical = critical;
            DefaultAvoid = avoid;
            DefaultMpMax = mp;
        }
        public Monster(Monster other) // 새로운 몬스터 인스턴스를 생성하기 위한 복사 생성자 추가
        {
            Name = other.Name;
            Hp = other.Hp;
            DefaultHpMax = other.DefaultHpMax;
            DefaultDamage = other.DefaultDamage;
            DefaultDefense = other.DefaultDefense;
            DefaultCritical = other.DefaultCritical;
            DefaultAvoid = other.DefaultAvoid;
            DefaultMpMax = other.DefaultMpMax;
        }


        public override void Attack(Creature creature, int line)
        {
            int printWidthPos = Console.WindowWidth / 2;
            bool isCritical = false;
            if (RandomChance(creature.DefaultAvoid)) // 상대방이 회피 했을때
            {
                Renderer.Print(line++, $"{creature.Name}이(가) 회피했습니다!", false, 100, printWidthPos);
            }
            else // 공격에 성공 했을 때
            {
                float damage = DefaultDamage;
                // 일정 확률로 치명타 적용
                if (RandomChance(DefaultCritical))
                {
                    damage *= 1.5f; // 기본 데미지의 1.5배
                    isCritical = true;
                }

                int finalDamage = Math.Clamp((int)damage - (int)DefaultDefense / 2, 0, (int)Hp);
                string battleText = $"{Name}이(가) {finalDamage}의 데미지로 공격하였습니다!";
                if (isCritical) battleText = "치명타 발생! " + battleText;
                Renderer.Print(line++, battleText, false, 100, printWidthPos);
                creature.OnDamaged(finalDamage);
            }
        }

        public override void OnDamaged(int damage)
        {
            Hp -= damage;
        }

        //확률에 따라 발생하는 메서드
        private bool RandomChance(float probability)
        {
            Random random = new Random();
            float randomValue = (float)random.NextDouble(); // 0.0에서 1.0 사이의 난수

            return randomValue < probability;
        }

        public override bool IsDead()
        {
            if (hp <= 0) return true;
            return false;
        }
    }
}
