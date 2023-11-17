using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    public class DungeonGateScene : Scene
    {
        public override string Title { get; protected set; } = "던 전 입 구";
        public override void EnterScene()
        {
            Options.Clear();
            Options.Add(Managers.Scene.GetOption("Main"));
            if(Game.Player.Hp >= 20)
                Options.Add(Managers.Scene.GetOption("DungeonEnter"));
            DrawScene();
        }

        public override void NextScene()
        {
            while (true)
            {
                DrawScene();
                if (!int.TryParse(Console.ReadLine(), out int index)) continue;
                if (index < 0 || Options.Count < index) continue;
                Options[index].Execute();
                break;
            }
        }
        protected override void DrawScene()
        {
            Renderer.DrawBorder(Title);
            Renderer.Print(5, $"Lv. {Game.Player.Level}");
            Renderer.Print(6, $"{Game.Player.Name} ( {Game.Player.Job} )");
            Renderer.Print(7, $"공격력 : {Game.Player.Damage}");
            Renderer.Print(8, $"방어력 : {Game.Player.Defense}");
            Renderer.Print(9, $"체 력 : {Game.Player.Hp} / {Game.Player.DefaultHpMax}");
            Renderer.Print(10, $"Gold : {Game.Player.Gold} G");
            if (Game.Player.Hp < 20)
                Renderer.Print(12, "체력이 부족하여 던전에 입장할 수 없습니다(체력 20이상 필요)");

            Renderer.PrintOptions(14, Options, true);
        }
    }
}
