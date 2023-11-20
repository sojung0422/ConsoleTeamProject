using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class TitleScene : Scene {

        public override string Title { get; protected set; } = "캐릭터 생성";

        static public string createName = string.Empty;
        private string errorMessage = string.Empty;

        public override void EnterScene() {
            
            do
            {
                DrawScene();                
            }
            while (ManageInput());
        }

        public override void NextScene() {
            // !! 여기가 문제 !! 여기서 받은 이름만 넘기는 방법은 무엇일까요?...
            Game.Player = new Character
            (
                createName,
                null,
                0,
                0,
                0,
                0,
                0,
                0,
                0
            );

            Managers.Scene.EnterScene<CreateCharacterScene>();
        }

        protected override void DrawScene() {
            Renderer.DrawBorder(Title);
            Renderer.Print(3, "스파르타 마을에 오신 여러분 환영합니다.");
            Renderer.Print(4, "당신의 이름은 무엇인가요?");
            Renderer.Print(10, errorMessage);
            ReadName();

        }

        /// <summary>
        /// 올바른 이름을 입력했는지 체크하고, 이름을 변경합니다.
        /// </summary>
        /// <param name="name">입력한 이름</param>
        public void OnNameChanged(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "오류: 이름을 입력해 주세요.";
                return;
            }

            if (Renderer.GetPrintingLength(name) > 10)
            {
                errorMessage = "오류: 이름이 너무 깁니다. 10글자 이내로 작성해 주세요.";
                return;
            }

            // 이름 결정
            createName = name;
        }

        /// <summary>
        /// 이름 입력
        /// </summary>
        private void ReadName()
        {            
            Console.SetCursorPosition(3, 7);
            var name = Console.ReadLine();

            // 이름 받아서 예외처리 후 createName에 이름 넣기 
            OnNameChanged(name);
        }

        private bool ManageInput()
        {
            // 이름 생성이 비어있으면 계속 실행
            if (createName == string.Empty)
            {
                return true;
            }
            else
                return false;
        }
    }
}
