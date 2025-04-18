using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class TitleScene : Scene {
        public override void EnterScene() {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("NewGame"));
            MusicPlayer.Instance.music = "Start.mp3";
            MusicPlayer.Instance.PlayAsync(1f); // 음악파일명, 볼륨
            // [우진영] 임시로 예외처리 해두었습니다.
            if (Managers.Game.data.character != null)
                Options.Add(Managers.Scene.GetOption("LoadGame"));

            DrawScene();
        }
        #region
        /*
         Renderer.PrintOptions(20, Options, true, selectedOptionIndex);
         옵션 선택지를 출력하는 함수
         
         Renderer는 콘솔에 메뉴, 텍스트 등 그려주는 도우미 클래스
         
         (20, Options, true, selectedOptionIndex) 뜻은:
         
         인자	                 의미
         20	                     출력 시작 y좌표 (화면 세로 줄 위치)
         Options	             선택지 목록 (List<ActionOption>)
         true	                 현재 선택된 항목을 하이라이트 처리할지 여부
         selectedOptionIndex	 현재 선택된 항목의 인덱스 (0부터 시작)
         */
        #endregion
        public override void NextScene() {

            do {
                Renderer.PrintOptions(20, Options, true, selectedOptionIndex);
                GetInput();
            }
            while (lastCommand != Command.Interact);
        }

        protected override void DrawScene()
        {
            Renderer.DrawBorder();

            Renderer.PrintCenter(6,   "██    ██ ███    ██ ██████  ███████ ██████      ████████  █████  ██      ███████ ");
            Renderer.PrintCenter(7,   "██    ██ ████   ██ ██   ██ ██      ██   ██        ██    ██   ██ ██      ██      ");
            Renderer.PrintCenter(8,   "██    ██ ██ ██  ██ ██   ██ █████   ██████         ██    ███████ ██      █████   ");
            Renderer.PrintCenter(9,   "██    ██ ██  ██ ██ ██   ██ ██      ██   ██        ██    ██   ██ ██      ██      ");
            Renderer.PrintCenter(10, "  ██████  ██   ████ ██████  ███████ ██   ██        ██    ██   ██ ███████ ███████  ");
                                                                              

            Renderer.Print(17," ▄▀▄▀▀▀▀▄▀▄", margin: Console.WindowWidth - 30);
            Renderer.Print(18," █░░░░░░░░▀▄      ▄ ", margin: Console.WindowWidth - 30);
            Renderer.Print(19,"█░░▀░░▀░░░░░▀▄▄  █░█", margin: Console.WindowWidth - 30);
            Renderer.Print(20,"█░▄░█▀░▄░░░░░░░▀▀░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(21,"█░░▀▀▀▀░░░░░░░░░░░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(22,"█░░░░░░░░░░░░░░░░░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(23,"█░░░░░░░░░░░░░░░░░░█", margin: Console.WindowWidth - 30);
            Renderer.Print(24," █░░▄▄░░▄▄▄▄░░▄▄░░█ ", margin: Console.WindowWidth - 30);
            Renderer.Print(25," █░▄▀█░▄▀░░█░▄▀█░▄▀ ", margin: Console.WindowWidth - 30);
            Renderer.Print(26, "  ▀   ▀     ▀   ▀    ", margin: Console.WindowWidth - 30);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
        }
    }
}
