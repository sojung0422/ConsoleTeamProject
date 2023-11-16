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
            Index = index;
            ItemName = name;
            GearType = type;
            GearState = state;
            ItemInfo = info;
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
            WriteLine($"- {ItemName} | {GearType} + {GearState} | {ItemInfo}");
        }

        public void DisplayEquip()
        {
            if (this.GearIsEquip == true)
            {
                WriteLine($"{ItemName}. [E] {ItemName} | {GearType} + {GearState} | {ItemInfo}");
            }
            else
            {
                WriteLine($"{ItemName}. {ItemName} | {GearType} + {GearState} | {ItemInfo}");
            }
        }
    }
}
