﻿using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TeamProject {
    public static class Renderer {

        // =================================================================================================================================================
        private static readonly int printMargin = 2;                                // 벽에 붙어서 출력되지 않기 위해 주는 Margin. (벽의 길이 = 1)
        private static readonly ConsoleColor bgColor = ConsoleColor.Black;       // 콘솔 기본 백그라운드 컬러.
        private static readonly ConsoleColor textColor = ConsoleColor.Yellow;       // 콘솔 기본 텍스트 컬러.
        private static readonly ConsoleColor highlightColor = ConsoleColor.Green;   // 콘솔 기본 하이라이트 컬러.
        // =================================================================================================================================================

        #region Fields


        private static int width;       // 화면 크기.
        private static int height;      // 화면 크기.

        #endregion

        public static void Initialize() { //콘솔 게임 시작 시 기본 환경 설정
            Console.Title = "GameName"; //콘솔 창 제목 지정
            Console.ForegroundColor = textColor; //글자색
            Console.BackgroundColor = bgColor; //배경색
            Console.Clear(); //전체 화면 지우기
            Console.OutputEncoding = Encoding.UTF8; //한글이나 특수문자 깨지지 않게 함
            Console.CursorVisible = false; //깜빡이는 커서 안 보이게
        }

        #region Print

        /// <summary>
        /// 해당 줄에 내용을 출력합니다.
        /// </summary>
        /// <param name="line">출력할 줄</param>
        /// <param name="content">출력할 내용</param>
        /// <param name="isHighlightNumber">숫자를 강조할 지 여부</param>
        /// <returns>출력한 후 다음 줄을 리턴합니다.</returns>
        public static int Print(int line, string content, bool isHighlightNumber = false, int delay = 0, int margin = 2, bool clear = false) {
            if (clear) ClearLine(line);
            Console.SetCursorPosition(margin, line++);
            if (isHighlightNumber) {
                foreach (char c in content) {
                    if (char.IsDigit(c)) {
                        Console.ForegroundColor = highlightColor;
                        Console.Write(c);
                        Console.ForegroundColor = textColor;
                    }
                    else Console.Write(c);
                    if (delay > 0) Thread.Sleep(delay / content.Length);
                }
            }
            else {
                if (delay > 0) {
                    int characterDelay = delay / content.Length;
                    foreach (char c in content) {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter) {
                            delay = 0;
                            characterDelay = 0;
                        }
                        Console.Write(c);
                        Thread.Sleep(characterDelay);
                    }
                }
                else Console.WriteLine(content);
            }
            return line;
        }
        /// <summary>
        /// 해당 줄의 가운데에 내용을 출력합니다.
        /// </summary>
        /// <param name="line">출력할 줄</param>
        /// <param name="content">출력할 내용</param>
        /// <returns>출력한 후 다음 줄을 리턴합니다.</returns>
        public static int PrintCenter(int line, string content) //가운데 정렬을 위한 함수
        {
            #region
            /*
             int pad = width - 3 - GetPrintingLength(content);
             ㄴ width: 콘솔 전체 가로 너비
             ㄴ GetPrintingLength(content): 문자열의 실제 길이를 계산하는 함수
             ㄴ 예: "Hello" → 5, "안녕" → 4 (한글은 보통 2칸 차지하니까)
             ㄴ - 3: 양쪽 테두리나 여백 때문에 조금 빼주는 거야
             
             **  결과적으로: pad는 글자 앞뒤로 넣을 수 있는 전체 여백을 뜻해
             
            -------------------------------------------------------------------------------------------------------

            [ return Print(line, "".PadLeft(pad) + content + "".PadRight(pad - 1)); ]

            Print(...): 실제로 콘솔에 글자를 출력해주는 함수
            line: 몇 번째 줄에 출력할지 (예: 7번째 줄)
            내용: 왼쪽 공백 + content + 오른쪽 공백
            → 이걸 하나의 문자열로 만든 다음 출력

            "" → 빈 문자열
            아무 문자도 없는 상태. 그냥 빈 공간.
            → 이 빈 문자열에 .PadLeft()나 .PadRight()를 써서 공백을 만든다는 뜻

            [ PadLeft(pad) ]
            왼쪽에 공백을 pad만큼 추가해주는 함수
            예: pad = 5이면 → " " (공백 5개짜리 문자열)
            이건 출력하려는 글자 앞에 공백을 추가해서, 왼쪽으로 띄워주는 역할

            content
            실제로 출력하고 싶은 글자 내용

            ""​.PadRight(pad - 1)
            오른쪽에 공백을 pad - 1만큼 추가하는 부분
            예: pad = 5면 → " " (공백 4개)
            왜 pad - 1이냐면?
            ➜ 전체 공백 수가 홀수일 때 균형 맞추기 위해임


            [ 이처럼 코드를 작성해야 하는 이유 ]
            가운데 정렬은 글자 길이, 화면 너비에 따라 공백을 정확히 계산해서 넣어야 함
            그걸 자동으로 해주는 함수가 없기 때문에, 이렇게 PadLeft, PadRight로 직접 만들어준 거임

            return Print(...)는:
            이 문자열을 출력하고,
            그 다음 줄 번호 (line + 1)를 반환해주는 역할

             */
            #endregion
            int pad = width - 3 - GetPrintingLength(content); //pad는 글자 앞 뒤로 넣을 수 있는 전체 여백
            if (pad % 2 != 0) //만약 여백 수가 홀수일 때
            {
                pad /= 2;
                return Print(line, "".PadLeft(pad) + content + "".PadRight(pad - 1));
            }
            else //짝수일 경우
            {
                pad /= 2;
                return Print(line, "".PadLeft(pad) + content + "".PadRight(pad));
            }

            //int correctLength = GetPrintingLength(content);
            //int start = (width - correctLength / 2);
            //if (start < 0) start = 0;
            //Console.SetCursorPosition(start, line++);
            //Console.WriteLine(content);
            //return line;
        }

        public static int PrintOptions(int line, List<ActionOption> options, bool fromZero = true, int selectionLine = 0) {
            for (int i = 0; i < options.Count; i++) {
                ActionOption option = options[i];
                Console.SetCursorPosition(printMargin, line);

                // [박상원] 선택된 옵션인 경우 초록색 글씨로 표현
                if (selectionLine == i) {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                //Console.Write(fromZero ? i : i + 1);
                //Console.Write(". ");
                Console.Write(option.Description);
                Console.ForegroundColor = ConsoleColor.Yellow;
                line++;
            }
            return line;
        }

        public static void PrintBattleText(int line, List<Creature> monsters, bool fromZero = true, int selectionLine = 0) {
            int margin = 33;
            int printWidthPos = Console.WindowWidth / 2 - margin;
            for (int i = 0; i < 2 + monsters.Count; i++) {
                Print(line + i, new string(' ', printWidthPos), false, 0, margin);
            }
            if (selectionLine >= 0)
                Print(line, "공격할 몬스터를 선택하세요.");
            Print(line++, "-------------------------", false, 0, margin);
            Print(line++, "        몬스터             ", false, 0, margin);
            Print(line++, "-------------------------", false, 0, margin);
            for (int i = 0; i < monsters.Count; i++) {
                Creature monster = monsters[i];
                Console.SetCursorPosition(margin, line);
                Console.Write(new string(' ', printWidthPos));
                Console.SetCursorPosition(margin, line);

                if (monster.IsDead()) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }


                if (selectionLine == i) {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write(fromZero ? i : i + 1);
                Console.Write(". ");
                if (monster.IsDead()) {
                    Console.WriteLine($"{monster.Name,-8} : Dead");
                    Console.SetCursorPosition(margin, ++line);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    PrintHPBar(monster);
                    line++;
                }
                else {
                    Console.WriteLine($"{monster.Name,-8} : {monster.Hp}/{monster.DefaultHpMax}");
                    Console.SetCursorPosition(margin, ++line);
                    Console.ForegroundColor = ConsoleColor.Red;
                    PrintHPBar(monster);
                    line++;

                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                line++;
            }
            Print(line++, "-------------------------", false, 0, margin);
        }

        //몬스터 HP 출력
        public static void PrintHPBar(Creature monster) {
            // HP 상태바 길이 조절
            int statusBarLength = 15;
            // HP 백분율 계산
            int hpPercentage = (int)((double)monster.Hp / monster.DefaultHpMax * statusBarLength);
            string statusBar = new string('█', hpPercentage) + new string(' ', statusBarLength - hpPercentage);
            // HP 상태 바 출력
            Console.WriteLine($"[{statusBar}]");
        }

        public static void PrintSelectAction(int line, List<string> actionText, bool fromZero = true, int selectionLine = 0) {
            int printWidthPos = 30;
            line++;
            for (int i = 0; i < height - 21; i++) {
                Print(line + i, new string(' ', printWidthPos));
            }
            Print(line++, "---------------------------");
            PrintPlayerState(line);
            line += 3;
            Print(line++, "---------------------------");
            for (int i = 0; i < actionText.Count; i++) {
                // 라인 클리어
                Console.SetCursorPosition(printMargin, line);
                Console.Write(new string(' ', printWidthPos));
                Console.SetCursorPosition(printMargin, line);

                Console.Write(fromZero ? i : i + 1);
                Console.Write(". ");

                if (selectionLine == i) {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(actionText[i]);
                //Console.Write(fromZero ? i : i + 1);
                //Console.Write(". ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                line++;
            }
            Print(line++, "---------------------------");

        }

        //플레이어 상태 출력
        public static void PrintPlayerState(int line) {
            Print(line, new string(' ', 30));
            Print(line, $"내 캐릭터 : {Game.Player.Name,-8} [{Game.Player.Job}]");
            line++;
            // 상태바 길이 조절
            int statusBarLength = 18;

            // HP, MP 백분율 계산
            int hpPercentage = (int)((double)Game.Player.Hp / Game.Player.HpMax * statusBarLength);
            string HPBar = new string('█', hpPercentage) + new string(' ', statusBarLength - hpPercentage);

            int mpPercentage = (int)((double)Game.Player.Mp / Game.Player.MpMax * statusBarLength);
            string MPBar = new string('█', mpPercentage) + new string(' ', statusBarLength - mpPercentage);

            Print(line, "".PadLeft(25, ' '), false, 0, 2);
            // HP,MP 상태 바 출력
            Console.SetCursorPosition(printMargin, line);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{HPBar}] {Game.Player.Hp}/{Game.Player.HpMax}  ");
            line++;

            Print(line, "".PadLeft(25, ' '), false, 0, 2);
            Console.SetCursorPosition(printMargin, line);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[{MPBar}] {Game.Player.Mp}/{Game.Player.MpMax}  ");

            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public static void ClearPlayerLine()
        {
            for (int line = 7; line <= 11; line++)
            {
                int printWidthPos = Console.WindowWidth / 2;
                Renderer.ClearLine(line, printWidthPos - 3, printWidthPos);
            }
        }

        /// <summary>
        /// 선택한 줄의 그려진 메시지를 지웁니다. DrawBorder()로 그려진 테두리는 지워지지 않습니다.
        /// </summary>
        /// <param name="line">지울 줄의 번호입니다.</param>
        public static void ClearLine(int line, int exclusionLength = 0, int margin = 2) => Print(line, "".PadLeft(width - 3 - exclusionLength, ' '), false, 0, margin);

        /// <summary>
        /// 키 조작 설명문 고정 위치의 출력
        /// </summary>
        /// <param name="keyGuide">출력할 설명문</param>
        public static void PrintKeyGuide(string keyGuide) {
            ClearLine(height - 2);
            Print(height - 2, keyGuide);
        }

        #endregion

        public static int DrawTable<T>(int startRow, List<T> items, List<TableFormatter<T>> formatters, int selectedIndex = -1) {
            // #1. 그리기 준비.
            int row = startRow;

            // #2. 타이틀 및 구분선 그리기.
            string title = "|", horizontal = "|";
            for (int i = 0; i < formatters.Count; i++) {
                TableFormatter<T> formatter = formatters[i];
                title += $"{formatter.GetTitle()}|";
                horizontal += $"{formatter.GetLine()}|";
            }
            Print(row++, title);
            Print(row++, horizontal);

            // #3. 본문 행 그리기.
            for (int i = 0; i < items.Count; i++) {
                T item = items[i];
                string content = "|";
                for (int j = 0; j < formatters.Count; j++) {
                    TableFormatter<T> formatter = formatters[j];
                    if (formatter.key == "Index") content += $"{formatter.GetString(i + 1)}|";
                    else content += $"{formatter.GetString(item)}|";
                }
                if (selectedIndex == i) {
                    Console.ForegroundColor = highlightColor;
                    Print(row++, content);
                    Console.ForegroundColor = textColor;
                }
                else Print(row++, content);
            }
            return row;
        }

        
        public static string GetTableElementString(int maxLength, string data, bool isTitle = false) {
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
}
