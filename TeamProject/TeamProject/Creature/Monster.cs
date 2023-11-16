using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TeamProject {
    public class Monster : Creature {
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
        public Monster(string name, float hp, float damage, float defense, float mp)
        {
            Name = name;
            Hp = hp;
            DefaultHpMax = hp;
            DefaultDamage = damage;
            DefaultDefense = defense;
            DefaultMpMax = mp;
        }

        public override void Attack(Creature creature)
        {
            Console.WriteLine($"{Name}이 {creature.Name}을 공격");
            creature.OnDamaged(DefaultDamage);
        }

        public override void OnDamaged(float damage)
        {
            int finalDamage = Math.Clamp((int)damage - (int)DefaultDefense / 2, 0, (int)DefaultDefense);
            Console.WriteLine($"{Name}이 {finalDamage}의 데미지를 입음");
            Console.WriteLine();
            Hp -= finalDamage;
        }
        public override bool IsDead()
        {
            if (hp <= 0) return true;
            return false;
        }
    }
}
