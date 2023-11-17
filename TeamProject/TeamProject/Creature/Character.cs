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
        public float Critical => DefaultCritical + criticalModifier;
        public float Avoid => DefaultAvoid + avoidModifier;

        public int NextLevelExp;
        public int TotalExp;

        public int Gold { get; protected set; }
        public override float Hp {
            get => hp;
            set {
                if (value <= 0) hp = 0;
                else if (value >= HpMax) hp = HpMax;
                else hp = value;
            }
        }
        public Inventory Inventory { get; }
        public Equipment Equipment { get; }

        public Character(string name, string job, int level, int damage, int defense, int hp, int gold, float critical, float avoid)
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
            
            Inventory = new Inventory(this);
            Equipment = new Equipment();
            NextLevelExp = 100;
            TotalExp = 0;
            Hp = HpMax;
        }

        public override void Attack(Creature creature)
        {
            Console.WriteLine();
            Console.WriteLine($"{Name}이 {creature.Name}을 공격");

            if (RandomChance(creature.DefaultAvoid)) // 상대방이 회피 했을때
            {
                Console.WriteLine($"{creature.Name}가 회피했습니다!");
                Console.WriteLine();
            }
            else // 공격에 성공 했을 때
            {
                // 일정 확률로 치명타 적용
                if (RandomChance(DefaultCritical))
                {
                    float criticalDamage = DefaultDamage * 1.5f; // 기본 데미지의 1.5배
                    Console.WriteLine("치명타 발생!");
                    creature.OnDamaged(criticalDamage);
                }
                else creature.OnDamaged(DefaultDamage); // 기본 공격
            }
        }

        public override void OnDamaged(float damage)
        {
            int finalDamage = Math.Clamp((int)damage - (int)DefaultDefense / 2, 0, (int)Hp);
            Console.WriteLine($"{finalDamage}의 데미지를 입음");
            Console.WriteLine();
            Hp -= finalDamage;
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

        public void ChangeExp(int expAmount)
        {
            int levelsToAdvance = 0;
            TotalExp += expAmount;
            while(TotalExp >= NextLevelExp)
            {
                TotalExp -= NextLevelExp;
                levelsToAdvance++;
                NextLevelExp += 50;
            }
            LevelUp(levelsToAdvance);
        }

        public void LevelUp(int levelsToAdvance)
        {
            if (levelsToAdvance == 0)
                return;
            Level += levelsToAdvance;
            // 임시 -> 레벨업당 공1, 방어0.5 증가
            DefaultDamage += 1.0f * levelsToAdvance;
            DefaultDefense += 0.5f * levelsToAdvance;

            // 출력
            Console.WriteLine($"레벨 {Level- levelsToAdvance} -> {Level}");
            Console.WriteLine($"공격력 {Damage - 1.0f * levelsToAdvance} -> {Damage}");
            Console.WriteLine($"방어력 {Defense - 0.5f * levelsToAdvance} -> {Defense}");
        }

        private float hpMaxModifier;
        private float damageModifier;
        private float defenseModifier;
        private float mpMaxModifier;
        private float criticalModifier;
        private float avoidModifier;
    }
}
