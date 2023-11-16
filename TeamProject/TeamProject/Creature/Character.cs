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
        public int Gold { get; protected set; }
        public override float Hp {
            get => hp;
            set {
                if (value <= 0) hp = 0;
                else if (value >= HpMax) hp = HpMax;
                else hp = value;
            }
        }

        public Character(string name, string job, int level, int damage, int defense, int hp, int gold) {
            Name = name;
            Job = job;
            Level = level;
            DefaultDamage = damage;
            DefaultDefense = defense;
            DefaultHpMax = hp;
            Gold = gold;

            Hp = HpMax;
        }
        public override void Attack(Creature creature)
        {
            Console.WriteLine($"{Name}이 {creature.Name}을 공격");
            creature.OnDamaged(DefaultDamage);
        }

        public override void OnDamaged(float damage)
        {
            int finalDamage = Math.Clamp((int)damage - (int)DefaultDefense/2, 0, (int)DefaultDefense);
            Console.WriteLine($"{Name}이 {finalDamage}입음");
            Hp -= finalDamage;
        }
        public override bool IsDead()
        {
            if (hp <= 0) return false;
            return true;
        }

        private float hpMaxModifier;
        private float damageModifier;
        private float defenseModifier;
        private float mpMaxModifier;
    }
}
