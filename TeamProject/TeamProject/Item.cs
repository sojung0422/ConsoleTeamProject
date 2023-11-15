using static System.Console;
namespace TeamProject
{
    public class Item
    {
        public int GearIndex { get; set; }
        public string GearName { get; set; }
        public string GearType { get; set; }
        public int GearState { get; set; }
        public string GearInfo { get; set; }

        public bool GearIsEquip { get; set; }


        public Item(int index, string name, string type, int state, string info, bool GearIsEquip = false)
        {
            GearIndex = index;
            GearName = name;
            GearType = type;
            GearState = state;
            GearInfo = info;
        }

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
            WriteLine($"- {GearName} | {GearType} + {GearState} | {GearInfo}");
        }

        public void DisplayEquip()
        {
            if (this.GearIsEquip == true)
            {
                WriteLine($"{GearIndex}. [E] {GearName} | {GearType} + {GearState} | {GearInfo}");
            }
            else
            {
                WriteLine($"{GearIndex}. {GearName} | {GearType} + {GearState} | {GearInfo}");
            }
        }
    }
}