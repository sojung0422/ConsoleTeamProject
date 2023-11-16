using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class Character : Creature{
        public string Job { get; protected set; }
        public float HpMax => DefaultHpMax + hpMaxModifier;
        public float Damage => DefaultDamage + damageModifier;
        public float Defense => DefaultDefense + defenseModifier;
        public float MpMax => DefaultMpMax + mpMaxModifier;
        public float Critical => DefaultCritical + criticalModifier; // 오태
        public float Avoid => DefaultCritical + avoidModifier; // 오태

        public int Gold { get; protected set; }
        public override float Hp {
            get => hp;
            set {
                if (value <= 0) hp = 0;
                else if (value >= HpMax) hp = HpMax;
                else hp = value;
            }
        }

        public Character(string name, string job, int level, int damage, int defense, int hp, int gold, float critical, float avoid) // 오태
        {
            Name = name;
            Job = job;
            Level = level;
            DefaultDamage = damage;
            DefaultDefense = defense;
            DefaultHpMax = hp;
            DefaultCritical = critical;
            DefaultAvoid = avoid;
            Gold = gold;

            Hp = HpMax;
        }
        public override void Attack(Creature creature) // 오태
        {
            Console.WriteLine($"{Name}이 {creature.Name}을 공격");

            if (RandomChance(creature.DefaultAvoid)) // 상대방이 회피 했을때
            {
                Console.WriteLine($"{creature.Name}가 회피했습니다!");
            }
            else // 공격에 성공 했을 때
            {
                // 일정 확률로 치명타 적용
                if (RandomChance(DefaultCritical))
                {
                    float criticalDamage = DefaultDamage * 2; // 기본 데미지의 2배
                    Console.WriteLine("치명타 발생!");
                    creature.OnDamaged(criticalDamage);
                }
                else
                {
                    creature.OnDamaged(DefaultDamage); // 기본 공격
                }
            }
        }

        public override void OnDamaged(float damage)
        {
            int finalDamage = Math.Clamp((int)damage - (int)DefaultDefense / 2, 0, (int)DefaultDefense);
            Console.WriteLine($"{Name}이 {finalDamage}의 데미지를 입음");
            Hp -= finalDamage;
        }

        private bool RandomChance(float probability) // 오태
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

        private float hpMaxModifier;
        private float damageModifier;
        private float defenseModifier;
        private float mpMaxModifier;
        private float criticalModifier; // 오태
        private float avoidModifier; // 오태
    }
}
