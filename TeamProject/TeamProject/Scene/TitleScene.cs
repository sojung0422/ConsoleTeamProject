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
            Options.Add(Managers.Scene.GetOption("LoadGame"));

            DrawScene();
        }

        public override void NextScene() {
            do {
                Renderer.PrintOptions(7, Options, true, selectionIdx);
            }
            while (ManageInput());
        }

        protected override void DrawScene() {
            Renderer.DrawBorder();
            Renderer.Print(5, "와 재미잇는 깨임");
            Renderer.PrintOptions(7, Options, true, selectionIdx);
            Renderer.PrintKeyGuide("[방향키 ↑ ↓: 선택지 이동] [Enter: 선택]");
        }

        private int selectionIdx = 0;
        public bool ManageInput() {
            var key = Console.ReadKey(true);

            var commands = key.Key switch {
                ConsoleKey.UpArrow => Command.MoveTop,
                ConsoleKey.DownArrow => Command.MoveBottom,
                ConsoleKey.Enter => Command.Interact,
                ConsoleKey.Escape => Command.Exit,
                _ => Command.Nothing
            };

            OnCommand(commands);

            return commands != Command.Interact;
        }

        private void OnCommand(Command cmd) {
            switch (cmd) {
                case Command.MoveTop:
                    if (selectionIdx > 0)
                        selectionIdx--;
                    break;
                case Command.MoveBottom:
                    if (selectionIdx < Options.Count - 1)
                        selectionIdx++;
                    break;
                case Command.Interact:
                    Options[selectionIdx].Execute();
                    break;
            }
        }
    }
}
