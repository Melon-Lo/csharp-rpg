using RPG.Models;
using DevKit.Utils;

namespace RPG.Logic
{
    public class BattleSystem
    {
        private Random _random = new Random();

        private void Attack(Battler actor, Battler target)
        {
            // 基本傷害
            float baseDamage = actor.TotalAttackPower - target.Defense;

            // 計算是否暴擊
            bool isCritical = _random.NextDouble() < actor.CriticalRate / 100.0f;
            float multiplier = isCritical ? 2.0f : 1.0f;
            int finalDamage = (int)Math.Max(MathF.Round(baseDamage * multiplier), 1f);

            target.Health -= finalDamage;

            string actorColor = (actor is Player) ? "blue" : "darkmagenta";
            string targetColor = (target is Player) ? "blue" : "darkmagenta";
            string criticalColor = "yellow";
            string damageColor = "red";

            ColorConsole.WriteEmbeddedColorLine(
                $"[{actorColor}]{actor.Name}[/{actorColor}]發動攻擊！" +
                $"{(isCritical ? $"[{criticalColor}]暴擊！[/{criticalColor}]" : "")}" +
                $"[{targetColor}]{target.Name}[/{targetColor}]受到 [{damageColor}]{finalDamage}[/{damageColor}] 點傷害。" +
                $"[{targetColor}]{target.Name}[/{targetColor}]當前血量: {target.Health} ({target.HealthPercentage}%)"
            );
        }

        private void Heal(Player actor, Player target)
        {
            target.Health += actor.HealPower;

            string nameColor = "blue";
            string healColor = "green";

            ColorConsole.WriteEmbeddedColorLine(
                $"[{nameColor}]{actor.Name}[/{nameColor}]對[{nameColor}]{target.Name}[/{nameColor}]發動治療！" +
                $"[{nameColor}]{target.Name}[/{nameColor}]回復了 [{healColor}]{actor.HealPower}[/{healColor}] 點血量。" +
                $"[{nameColor}]{actor.Name}[/{nameColor}]當前血量: {target.Health} ({target.HealthPercentage}%)"
            );
        }

        private void ExecuteTurn(Battler actor, Battler target)
        {
            string actorColor = (actor is Player) ? "blue" : "darkmagenta";

            ColorConsole.WriteEmbeddedColorLine($"\n--- [{actorColor}]{actor.Name}[/{actorColor}]的回合 ---");

            // 如果是玩家行動
            if (actor is Player player)
            {
                bool acted = false;

                while (!acted)
                {
                    Console.WriteLine("請選擇行動: 1. 攻擊 / 2. 治療");
                    string choice = Console.ReadLine() ?? "";

                    if (choice == "1")
                    {
                        Attack(actor: player, target: target);
                        acted = true;
                    }
                    else if (choice == "2")
                    {
                        Heal(actor: player, target: player);
                        acted = true;
                    }
                    else
                    {
                        Console.WriteLine("無效的選擇，請重新選擇。");
                    }
                }
            }
            else
            {
                // 怪物自動攻擊
                Attack(actor: actor, target: target);
            }
        }

        public void StartBattle(Player player, Enemy Enemy)
        {
            string playerColor = "blue";
            string enemyColor = "darkmagenta";

            Console.WriteLine("戰鬥開始！");
            ColorConsole.WriteEmbeddedColorLine($"[{playerColor}]{player.Name}[/{playerColor}] (血量: {player.Health}/{player.MaxHealth}) VS [{enemyColor}]{Enemy.Name}[/{enemyColor}] (血量: {Enemy.Health}/{Enemy.MaxHealth})");

            // 根據速度決定順序
            Battler first, second;

            if (player.Agility >= Enemy.Agility)
            {
                first = player;
                second = Enemy;
            }
            else
            {
                first = Enemy;
                second = player;
            }

            string firstColor = (first is Player) ? "blue" : "darkmagenta";
            ColorConsole.WriteEmbeddedColorLine($"[{firstColor}]{first.Name}[/{firstColor}]取得先機，率先發動攻擊！");

            Thread.Sleep(1000);

            // 戰鬥迴圈
            while (player.Health > 0 && Enemy.Health > 0)
            {
                // 第一順位攻擊
                ExecuteTurn(actor: first, target: second);
                if (first.Health <= 0 || second.Health <= 0) break;

                Thread.Sleep(1000);

                // 第二順位攻擊
                ExecuteTurn(actor: second, target: first);
                if (first.Health <= 0 || second.Health <= 0) break;

                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);

            string loser = player.Health <= 0 ? player.Name : Enemy.Name;
            string loserColor = player.Health <= 0 ? playerColor : enemyColor;
            ColorConsole.WriteEmbeddedColorLine($"[{loserColor}]{loser}[/{loserColor}] 被擊敗了！");

            Thread.Sleep(1000);

            string winner = player.Health > 0 ? player.Name : Enemy.Name;
            string winnerColor = player.Health > 0 ? playerColor : enemyColor;
            ColorConsole.WriteEmbeddedColorLine($"[{winnerColor}]{winner}[/{winnerColor}]獲勝！戰鬥結束。");
        }
    }
}