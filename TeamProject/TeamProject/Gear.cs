using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace TeamProject
{
    public class Gear : Item
    {
        public string GearType { get; set; }
        public int GearState { get; set; }
        public bool GearIsEquip { get; set; }

        public Gear(int id, string name, string description, int price, string type, int state, bool isEquip = false) : base(id, name, description, price)
        {
            GearType = type;
            GearState = state;
            GearIsEquip = isEquip;
        }

        // [박상원]
        // 장비 슬롯이 비어있는 경우, 빈 아이템 생성
        public static Gear Empty = new(-1, string.Empty, string.Empty, 0, string.Empty, 0);

        public void GearEquip()
        {
            GearIsEquip = true;
        }

        public void GearUnEquip()
        {
            GearIsEquip = false;
        }

        public void Display()
        {
            WriteLine($"- {Name} | {GearType} + {GearState} | {Description}");
        }

        public void DisplayEquip()
        {
            if (this.GearIsEquip == true)
            {
                WriteLine($"{Name}. [E] {Name} | {GearType} + {GearState} | {Description}");
            }
            else
            {
                WriteLine($"{Name}. {Name} | {GearType} + {GearState} | {Description}");
            }
        }
    }
}
