using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public static  class Renderer {

        // =================================================================================================================================================
        private static readonly int printMargin = 2;                                // 벽에 붙어서 출력되지 않기 위해 주는 Margin. (벽의 길이 = 1)
        private static readonly ConsoleColor bgColor = ConsoleColor.Black;       // 콘솔 기본 백그라운드 컬러.
        private static readonly ConsoleColor textColor = ConsoleColor.Yellow;       // 콘솔 기본 텍스트 컬러.
        private static readonly ConsoleColor highlightColor = ConsoleColor.Green;   // 콘솔 기본 하이라이트 컬러.
        // =================================================================================================================================================

        #region Fields

        public static Dictionary<string, ItemTableFormatter> ItemTableFormatters = new();
        public static Dictionary<string, JobTableFormatter> JobTableFormatters = new();

        private static int width;       // 화면 크기.
        private static int height;      // 화면 크기.

        #endregion

        public static void Initialize() {
            Console.Title = "GameName";
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = bgColor;
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;

            ItemTableFormatters["Index"] = new("Index", "", 2, null);
            ItemTableFormatters["Name"] = new("Name", "이름", 22, i => {
                if (i is Gear gear)
                {
                    if (gear.IsEquip) return $"[E] {gear.Name}";
                    else return gear.Name;
                }
                else return i.Name;
            });
            ItemTableFormatters["ItemType"] = new("ItemType", "타입", 15, i => {
                if (i is Gear gear) return gear.GearType.String();
                else return i.Type.String();
            });
            ItemTableFormatters["Stat"] = new("Stat", "스탯", 34, i => {
                if (i is Gear gear) return gear.StatToString();
                else return string.Empty;
            });
            ItemTableFormatters["Desc"] = new("Desc", "설명", 30, i => i.Description);
            ItemTableFormatters["Cost"] = new("Cost", "비용", 10, i => i.Price.ToString());
            ItemTableFormatters["SellCost"] = new("SellCost", "비용", 10, i => (i.Price * 0.85f).ToString());

            JobTableFormatters["Job"] = new("Job", "직업", 10, c => c.Job.ToString());
            JobTableFormatters["Damage"] = new("DefaultDamage", "공격력", 10, c => c.DefaultDamage.ToString());
            JobTableFormatters["Defense"] = new("DefaultDefense", "방어력", 10, c => c.DefaultDefense.ToString());
            JobTableFormatters["HpMax"] = new("DefalutHpMax", "체 력", 10, c => c.DefaultHpMax.ToString());
            JobTableFormatters["Critical"] = new("Critical", "크리율", 20, c => c.Critical.ToString("0%"));
            JobTableFormatters["Avoid"] = new("Avoid", "회피율", 20, c => c.Critical.ToString("0%"));

        }

        #region Print

        /// <summary>
        /// 해당 줄에 내용을 출력합니다.
        /// </summary>
        /// <param name="line">출력할 줄</param>
        /// <param name="content">출력할 내용</param>
        /// <param name="isHighlightNumber">숫자를 강조할 지 여부</param>
        /// <returns>출력한 후 다음 줄을 리턴합니다.</returns>
        public static int Print(int line, string content, bool isHighlightNumber = false) {
            Console.SetCursorPosition(printMargin, line++);
            if (isHighlightNumber) {
                foreach (char c in content) {
                    if (int.TryParse(c.ToString(), out int num)) {
                        Console.ForegroundColor = highlightColor;
                        Console.Write(num);
                        Console.ForegroundColor = textColor;
                    }
                }
            }
            else {
                Console.WriteLine(content);
            }
            return line;
        }
        /// <summary>
        /// 해당 줄의 가운데에 내용을 출력합니다.
        /// </summary>
        /// <param name="line">출력할 줄</param>
        /// <param name="content">출력할 내용</param>
        /// <returns>출력한 후 다음 줄을 리턴합니다.</returns>
        public static int PrintCenter(int line, string content) {
            int correctLength = GetPrintingLength(content);
            int start = (width - correctLength / 2);
            if (start < 0) start = 0;
            Console.SetCursorPosition(start, line++);
            Console.WriteLine(content);
            return line;
        }

        public static void PrintOptions(int line, List<ActionOption> options, bool fromZero = true, int selectionLine = 0) {
            for (int i = 0; i < options.Count; i++) {
                ActionOption option = options[i];
                Console.SetCursorPosition(printMargin, line);

                // [박상원] 선택된 옵션인 경우 초록색 글씨로 표현
                if (selectionLine == i)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                //Console.Write(fromZero ? i : i + 1);
                //Console.Write(". ");
                Console.Write(option.Description);
                Console.ForegroundColor = ConsoleColor.Yellow;
                line++;
            }
        }

        /// <summary>
        /// 키 조작 설명문 고정 위치의 출력
        /// </summary>
        /// <param name="keyGuide">출력할 설명문</param>
        public static void PrintKeyGuide(string keyGuide)
        {
            Print(height - 2, keyGuide);
        }

        #endregion

        #region Inventory
        public static int DrawItemList(int startRow, List<Item> items, List<ItemTableFormatter> formatterList, int selectionIdx = -1) {
            // #1. 그리기 준비.
            int row = startRow;

            // #2. 상위 행 그리기.
            string title = "|";
            string horizontal = "|";
            for (int i = 0; i < formatterList.Count; i++) {
                ItemTableFormatter formatter = formatterList[i];
                title += $"{formatter.GetTitle()}|";
                horizontal += $"{formatter.GetString()}|";
            }
            Print(row++, title);
            Print(row++, horizontal);

            // #3. 본문 행 그리기.
            for (int i = 0; i < items.Count; i++) {
                Item item = items[i];
                string content = "|";
                for (int j = 0; j < formatterList.Count; j++) {
                    ItemTableFormatter formatter = formatterList[j];
                    if (formatter.key == "Index") content += $"{formatter.GetString(i + 1)}|";          // 아이템 번호 출력.
                    //else if (formatter.key == "Type") content += $"{formatter.GetString(false)}|";    // [박상원] 타입 대신 이름 앞에 출력하도록 주석 처리함
                    else content += $"{formatter.GetString(item)}|";                                    // 아이템 정보 출력.
                }

                // 선택 부분 아이템 글씨 컬러 바꾸기
                if (selectionIdx == i)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Print(row++, content);
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            return row;
        }
        
        public static int DrawJobList(int startRow, Character[] characters, List<JobTableFormatter> formatterList) {
            // #1. 그리기 준비.
            int row = startRow;

            // #2. 상위 행 그리기.
            string title = "|";
            string horizontal = "|";
            for (int i = 0; i < formatterList.Count; i++) {
                JobTableFormatter formatter = formatterList[i];
                title += $"{formatter.GetTitle()}|";
                horizontal += $"{formatter.GetString()}|";
            }
            Print(row++, title);
            Print(row++, horizontal);

            // #3. 본문 행 그리기.
            for (int i = 0; i < characters.Length; i++) {
                Character character = characters[i];
                string content = "|";
                for (int j = 0; j < formatterList.Count; j++) {
                    JobTableFormatter formatter = formatterList[j];
                    if (formatter.key == "Index") content += $"{formatter.GetString(i + 1)}|";          // 아이템 번호 출력.
                    
                    else content += $"{formatter.GetString(character)}|";                                    // 아이템 정보 출력.
                }
                Print(row++, content);
            }
            return row;
        }
        public static string GetInventoryElementString(int maxLength, string data, bool isTitle = false) {
            int dataLength = GetPrintingLength(data);
            if (data == "=") return new string('=', maxLength);
            StringBuilder builder = new();
            int spaceCount = maxLength - dataLength;
            int margin = isTitle ? 2 : 1;
            int leftCount = Math.Clamp(spaceCount / 2, 0, margin);
            builder.Append(' ', leftCount);
            builder.Append(data);
            builder.Append(' ', spaceCount - leftCount);
            return builder.ToString();
        }

        #endregion

        #region Border

        public static void DrawBorder(string title = "") {
            Console.Clear();
            width = Console.WindowWidth;
            height = Console.WindowHeight;

            Console.SetCursorPosition(0, 0);
            Console.Write(new string('=', width));
            for (int i = 1; i < height - 1; i++) {
                Console.SetCursorPosition(0, i);
                Console.Write('║');
                Console.SetCursorPosition(width - 1, i);
                Console.Write('║');
            }
            if (!string.IsNullOrEmpty(title)) {
                Console.SetCursorPosition(0, 2);
                Console.Write(new string('=', width));
                int correctLength = GetPrintingLength(title);
                int horizontalStart = (width - correctLength) / 2;
                if (horizontalStart < 0) horizontalStart = 3;
                Console.SetCursorPosition(horizontalStart, 1);
                Console.WriteLine(title);
            }
            Console.SetCursorPosition(0, height - 1);
            Console.Write(new string('=', width));
        }

        #endregion

        #region Utilities

        public static int GetPrintingLength(string line) => line.Sum(c => IsKorean(c) ? 2 : 1);
        private static bool IsKorean(char c) => '가' <= c && c <= '힣';

        #endregion
    }

    public class ItemTableFormatter {
        public string key;
        public string description;
        public int length;
        public Func<Item, string>? dataSelector;

        public ItemTableFormatter(string key, string description, int length, Func<Item, string>? dataSelector) {
            this.key = key;
            this.description = description;
            this.length = length;
            this.dataSelector = dataSelector;
        }

        public string GetTitle() => Renderer.GetInventoryElementString(length, description, true);
        public string GetString() => Renderer.GetInventoryElementString(length, "=", false);
        public string GetString(int index) => Renderer.GetInventoryElementString(length, index.ToString(), false);
        public string GetString(bool isEquipped) => Renderer.GetInventoryElementString(length, isEquipped ? "[E]" : "", false);
        public string GetString(Item item) => Renderer.GetInventoryElementString(length, dataSelector(item), false);
    }

    public class JobTableFormatter
    {
        public string key;
        public string description;
        public int length;
        public Func<Character, string>? dataSelector;

        public JobTableFormatter(string key, string description, int length, Func<Character, string>? dataSelector)
        {
            this.key = key;
            this.description = description;
            this.length = length;
            this.dataSelector = dataSelector;
        }

        public string GetTitle() => Renderer.GetInventoryElementString(length, description, true);
        public string GetString() => Renderer.GetInventoryElementString(length, "=", false);
        public string GetString(int index) => Renderer.GetInventoryElementString(length, index.ToString(), false);        
        public string GetString(Character character) => Renderer.GetInventoryElementString(length, dataSelector(character), false);
    }
}
