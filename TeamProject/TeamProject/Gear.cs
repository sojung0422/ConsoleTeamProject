using System;
using System.Collections.Generic;
using System.Linq;
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

        public Gear(int index, string name, string type, int state, string info, bool GearIsEquip = false)
        {
            ID = index;
            Name = name;
            GearType = type;
            GearState = state;
            Description = info;
        }

        public static Gear Empty = new(0, string.Empty, string.Empty, 0, string.Empty);


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
