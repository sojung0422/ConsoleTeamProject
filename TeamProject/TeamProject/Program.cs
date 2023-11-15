using static System.Console;
namespace TeamProject
{
    internal class Program
    {
        private static Character player;
        public static List<Item> InvenGears = new List<Item>();
        private static int inputKey;
        static void Main(string[] args)
        {
            Item gear0 = new Item(1, "천 옷", "방어력", 2, "평상시에 입는 옷이다.");
            Item gear1 = new Item(2, "녹슨 검", "공격력", 5, "흔하게 볼 수 있는 검이다.");

            InvenGears.Add(gear0);
            InvenGears.Add(gear1);

            player = new Character("규라니", "전사", 1, 10, 5, 100, 1500);

            MainTitle();
        }


        static void MainTitle()
        {
            Clear();

            WriteLine();
            WriteLine("저희 마을에 어서 오세요.");
            WriteLine("던전으로 들어가기 전 채비를 갖추시죠.");
            WriteLine();
            WriteLine("1. 상태보기");
            WriteLine("2. 인벤토리");
            WriteLine();

            WriteLine("원하는 행동을 입력을 해주세요.");

            inputKey = CheckInputKey(1, 2);
            switch (inputKey)
            {
                case 1:
                    DisPlayMyState(InvenGears);
                    break;

                case 2:
                    DisplayInventory();
                    break;
            }
        }

        static void DisPlayMyState(List<Item> gear)
        {
            Clear();

            int atk = 0;
            int armor = 0;


            // 공격력, 방어력 타입의 능력치들을 다 합치기
            for (int i = 0; i < gear.Count; i++)
            {
                if (gear[i].GearType == "공격력" && gear[i].GearIsEquip == true)
                {
                    atk += gear[i].GearState;
                }
                else if (gear[i].GearType == "방어력" && gear[i].GearIsEquip == true)
                {
                    armor += gear[i].GearState;
                }
            }
            // 추후에는 장착할 때 스탯을 더하고 저장하는 방식이면 더 좋을 것 같다.

            WriteLine();
            WriteLine("상태보기");
            WriteLine("캐릭터의 정보가 표시됩니다.");
            WriteLine();
            WriteLine($"Lv.{player.Level}");
            WriteLine($"{player.Name}({player.Job})");
            WriteLine($"공격력 :{player.Damage} + {atk}");
            WriteLine($"방어력 : {player.Defense} + {armor}");

            WriteLine($"체력 : {player.Hp}");
            WriteLine($"Gold : {player.Gold} G");
            WriteLine();
            WriteLine("1. 나가기");

            inputKey = CheckInputKey(1, 1);
            switch (inputKey)
            {
                case 1:
                    MainTitle();
                    break;
            }
        }

        static void DisplayInventory()
        {
            Clear();

            WriteLine("");
            WriteLine("인벤토리");
            WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            WriteLine("");
            WriteLine("[아이템 목록]");
            WriteLine("");
            InvenGears[0].Display();
            InvenGears[1].Display();
            WriteLine("");
            WriteLine("2. 장착 관리");
            WriteLine("1. 나가기");
            WriteLine("");
            WriteLine("원하는 행동을 입력을 해주세요.");

            inputKey = CheckInputKey(1, 2);
            switch (inputKey)
            {
                case 1:
                    MainTitle();
                    break;

                case 2:
                    DisplayEquipControl(InvenGears);
                    break;

            }
        }

        static void DisplayEquipControl(List<Item> gear)
        {

            Clear();
            WriteLine("인벤토리 - 장착관리");
            WriteLine("장비의 번호를 입력하면 아이템을 장착, 해제 할 수 있습니다.");
            WriteLine("");
            WriteLine("[아이템 목록]");
            gear[0].DisplayEquip();
            gear[1].DisplayEquip();



            WriteLine("");
            WriteLine("");
            WriteLine("0. 나가기");

            inputKey = CheckInputKey(0, 2);
            switch (inputKey)
            {
                case 1:
                    EquipControl(inputKey);
                    DisplayEquipControl(InvenGears);
                    break;

                case 2:
                    EquipControl(inputKey);
                    DisplayEquipControl(InvenGears);
                    break;

                case 0:
                    DisplayInventory();
                    break;
            }
        }

        static void EquipControl(int index)
        {
            if (InvenGears[inputKey - 1].GearIsEquip != true)
                InvenGears[inputKey - 1].GearEquip();
            else
                InvenGears[inputKey - 1].GearUnEquip();
        }

        static int CheckInputKey(int min, int max)
        {
            while (true)
            {
                string input = ReadLine();
                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                WriteLine("잘못된 입력입니다.");
            }
        }
    }
}