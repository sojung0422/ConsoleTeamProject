using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject;
public class StageRewardScene : Scene
{
    public override string Title { get; protected set; } = "보 상";


    #region Scene

    public override void EnterScene()
    {
        Options.Clear();

        Renderer.DrawBorder(Title);
        DrawScene();
    }

    public override void NextScene()
    {
        while (Game.currentStageReward.exp > 0 || Game.currentStageReward.gold > 0)
        {
            if (Game.currentStageReward.exp > 0)
            {
                Game.Player.TotalExp++;
                Game.currentStageReward.exp--;
                if (Game.Player.TotalExp >= Game.Player.NextLevelExp)
                    Game.Player.ChangeExp(0);
            }

            if (Game.currentStageReward.gold > 7)
            {
                Game.Player.Gold += 7;
                Game.currentStageReward.gold -= 7;
            }
            else
            {
                Game.Player.Gold += Game.currentStageReward.gold;
                Game.currentStageReward.gold = 0;
            }
            DrawScene();
            Thread.Sleep(10);
            if (Skip())
                break;
        }

        // 스킵했을 경우 남아있는 수치를 한번에 합침
        Game.Player.TotalExp += Game.currentStageReward.exp;
        Game.currentStageReward.exp = 0;
        if (Game.Player.TotalExp >= Game.Player.NextLevelExp)
            Game.Player.ChangeExp(0);
        Game.Player.Gold += Game.currentStageReward.gold;
        Game.currentStageReward.gold = 0;

        DrawScene();
        Managers.Game.SaveGame();
        GetInput();
        Managers.Scene.EnterScene<DungeonGateScene>();
    }

    protected override void DrawScene()
    {
        int row = Renderer.PrintCenter(3, $"스테이지 {Game.currentStageReward.stageNumber} {(Game.currentStageReward.isClear ? "클리어" : "실패")}");

        row = Renderer.PrintCenter(row + 3, $"Level");
        row = Renderer.PrintCenter(row, $"{Game.Player.Level}");
        row = DrawEXPBar(row + 1);
        row = Renderer.PrintCenter(row, $"{Game.Player.TotalExp} / {Game.Player.NextLevelExp}");
        row = Renderer.PrintCenter(row + 1, $"획득한 골드");
        if (Game.currentStageReward.gold > 0)
            row = Renderer.PrintCenter(row, $"{Game.Player.Gold} G +{Game.currentStageReward.gold} G");
        else
            row = Renderer.PrintCenter(row, $"{Game.Player.Gold} G");
        Renderer.PrintKeyGuide("[AnyKey] : 던전 입구로 돌아가기");
    }

    #endregion

    #region Input

    protected override void OnCommandExit()
    {

    }
    protected override void OnCommandInteract()
    {

    }

    #endregion

    int DrawEXPBar(int line)
    {
        string bar;
        int extraCnt = 50;
        float rate = (float)Game.Player.TotalExp / Game.Player.NextLevelExp;
        int barCnt = (int)(extraCnt * rate);
        if (barCnt > extraCnt) barCnt = extraCnt;
        extraCnt -= barCnt;
        Console.ForegroundColor = ConsoleColor.Green;
        bar = $"[{new('█', barCnt)}{new(' ', extraCnt)}]";
        line = Renderer.PrintCenter(line, bar);
        Console.ForegroundColor = ConsoleColor.Yellow;
        return line;
    }
    public bool Skip()
    {
        while (Console.KeyAvailable) // 버퍼에 입력이 있는 경우 처리
        {
            Console.ReadKey(true); // 입력을 읽고 버퍼를 비움
            return true;
        }
        return false;
    }
}
