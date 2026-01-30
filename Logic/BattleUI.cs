using DevKit.Utils;

namespace RPG.UI
{
    public static class BattleUI
    {
        public static int GameSpeedMs { get; set; } = 1000;

        public static void Log(string message, bool wait = true)
        {
            ColorConsole.WriteEmbeddedColorLine(message);
            if (wait) Thread.Sleep(GameSpeedMs);
        }

        public static void Wait(int customMs = -1)
        {
            Thread.Sleep(customMs >= 0 ? customMs : GameSpeedMs);
        }

        public static void Clear() => Console.Clear();
    }
}