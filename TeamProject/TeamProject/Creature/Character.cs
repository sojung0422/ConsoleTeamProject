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
        public Inventory Inventory { get; }

        public Character(string name, string job, int level, int damage, int defense, int hp, int gold) {
            Name = name;
            Job = job;
            Level = level;
            DefaultDamage = damage;
            DefaultDefense = defense;
            DefaultHpMax = hp;
            Gold = gold;
            Inventory = new Inventory(this);

            Hp = HpMax;
        }

        private float hpMaxModifier;
        private float damageModifier;
        private float defenseModifier;
        private float mpMaxModifier;
    }
}
