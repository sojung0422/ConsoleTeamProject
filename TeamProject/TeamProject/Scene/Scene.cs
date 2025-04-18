using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class Scene {
        public virtual string Title { get; protected set; } //virtual → 자식 클래스에서 재정의(override) 가능 / protected set → 자식 클래스에서만 값을 변경할 수 있어

        protected List<ActionOption> Options { get; set; } = new(); //화면에 출력되는 선택지 목록 -> 메뉴 옵션들을 저장함
        //얘가 화면에 출력되는 선택지들임(씬의 모음인 거지 - 리스트로 저장한)
        protected int selectedOptionIndex = 0;
        protected Command lastCommand = Command.Nothing;

        /// <summary>
        /// 씬 진입 시 액션.
        /// </summary>
        public virtual void EnterScene() {

        }
        /// <summary>
        /// 다음 씬으로 넘어가기 위한 조건.
        /// </summary>
        public virtual void NextScene() {

        }
        /// <summary>
        /// 씬 보여주기.
        /// </summary>
        protected virtual void DrawScene() {

        }

        protected void GetInput() {
            lastCommand = Command.Nothing;
            lastCommand = Console.ReadKey(true).Key switch {
                ConsoleKey.UpArrow => Command.MoveTop,
                ConsoleKey.DownArrow => Command.MoveBottom,
                ConsoleKey.LeftArrow => Command.MoveLeft,
                ConsoleKey.RightArrow => Command.MoveRight,
                ConsoleKey.Enter => Command.Interact,
                ConsoleKey.Escape => Command.Exit,
                _ => Command.Nothing,
            };

            OnCommand(lastCommand);
        }

        private void OnCommand(Command command) {
            switch (command) {
                case Command.MoveTop: OnCommandMoveTop(); break;
                case Command.MoveBottom: OnCommandMoveBottom(); break;
                case Command.MoveLeft: OnCommandMoveLeft(); break;
                case Command.MoveRight: OnCommandMoveRight(); break;
                case Command.Interact: OnCommandInteract(); break;
                case Command.Exit: OnCommandExit(); break;
            }
        }

        protected virtual void OnCommandMoveTop() {
            if (selectedOptionIndex > 0) selectedOptionIndex--;
        }
        protected virtual void OnCommandMoveBottom() {
            if (selectedOptionIndex < Options.Count - 1) selectedOptionIndex++;
        }
        protected virtual void OnCommandMoveLeft() {

        }
        protected virtual void OnCommandMoveRight() {
                
        }
        protected virtual void OnCommandInteract() {
            if (Options.Count > 0)
                Options[selectedOptionIndex].Execute();
        }
        protected virtual void OnCommandExit() {
            if (Managers.Scene.PrevScene != null)
                Managers.Scene.GetOption("Back").Execute();
        }
    }
}
