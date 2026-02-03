using RPG.Models;
using RPG.UI;
using DevKit.Utils;

namespace RPG.Logic
{
    public class BattleSystem
    {
        private Random _random = new Random();

        private HashSet<Battler> DeadUnitsThisBattle = new HashSet<Battler>();

        // 攻擊：接受 Battler
        private void Attack(Battler actor, Battler target)
        {
            float baseDamage = actor.TotalAttackPower - target.Defense;
            bool isCritical = _random.NextDouble() < actor.CriticalRate / 100.0f;
            float multiplier = isCritical ? 2.0f : 1.0f;
            int finalDamage = (int)Math.Max(MathF.Round(baseDamage * multiplier), 1f);

            target.Health -= finalDamage;

            string actorColor = (actor is Player) ? "blue" : "darkmagenta";
            string targetColor = (target is Player) ? "blue" : "darkmagenta";

            BattleUI.Log(
                $"[{actorColor}]{actor.Name}[/{actorColor}]發動攻擊！" +
                $"{(isCritical ? "[yellow]暴擊！[/yellow]" : "")}" +
                $"[{targetColor}]{target.Name}[/{targetColor}]受到 [red]{finalDamage}[/red] 點傷害。"
            );
        }

        // 治療：接受 Player
        private void Heal(Player actor, Player target)
        {
            int costMana = 10;
            if (actor.Mana < costMana)
            {
                BattleUI.Log($"[blue]{actor.Name}[/blue]法力不足！(需 {costMana} MP)");
                // 注意：這裡不直接 call HandlePlayerChoice，交給迴圈控制
                return;
            }

            actor.Mana -= costMana;
            target.Health += actor.HealPower;

            BattleUI.Log(
                $"[blue]{actor.Name}[/blue]對[blue]{target.Name}[/blue]發動治療！" +
                $"回復了 [green]{actor.HealPower}[/green] 點血量。"
            );
        }

        // 執行回合：接受雙方陣列
        private void ExecuteTurn(Battler actor, Player[] players, Enemy[] enemies)
        {
            // 檢查發動者是否還活著
            if (actor.Health <= 0) return;

            // --- 顯示狀態面板 (Player 永遠在上方) ---
            BattleUI.Log("---------------------------", false);

            foreach (var p in players)
            {
                BattleUI.Log($"[blue]{p.Name}[/blue]: HP {p.Health}/{p.MaxHealth} | MP {p.Mana}/{p.MaxMana}", false);
            }
            foreach (var e in enemies)
            {
                BattleUI.Log($"[darkmagenta]{e.Name}[/darkmagenta]: HP {e.Health}/{e.MaxHealth}", false);
            }

            BattleUI.Log("---------------------------", false);

            BattleUI.Log($"\n--- {(actor is Player ? "[blue]" : "[darkmagenta]")}{actor.Name}[/{(actor is Player ? "blue" : "darkmagenta")}] 的回合 ---", false);

            if (actor is Player player)
            {
                // 傳入活著的敵人作為攻擊目標，活著的隊友作為治療目標
                var aliveEnemies = enemies.Where(e => e.Health > 0).Cast<Battler>().ToList();
                var aliveAllies = players.Where(p => p.Health > 0).Cast<Battler>().ToList();

                HandlePlayerChoice(player, aliveEnemies, aliveAllies);
            }
            else
            {
                // 怪物 AI：隨機攻擊一個活著的玩家
                var alivePlayers = players.Where(p => p.Health > 0).ToList();
                if (alivePlayers.Any())
                {
                    var target = alivePlayers[_random.Next(alivePlayers.Count)];
                    Attack(actor, target);
                }
            }

            // 掃描所有戰鬥參與者是否死亡
            CheckAllDeaths(players, enemies);

            // 停頓，進入下一動
            BattleUI.Wait();
        }

        private void HandlePlayerChoice(Player player, List<Battler> enemies, List<Battler> allies)
        {
            bool acted = false;
            while (!acted)
            {
                BattleUI.Log("請選擇行動: 1. 攻擊 / 2. 治療", false);
                string choice = Console.ReadLine() ?? "";

                if (choice == "1")
                {
                    var target = SelectTarget(enemies, "攻擊");
                    if (target != null) { Attack(player, target); acted = true; }
                }
                else if (choice == "2")
                {
                    int costMana = 10;
                    if (player.Mana < costMana)
                    {
                        BattleUI.Log($"法力不足，請重新選擇行動。 (目前法力: {player.Mana} / 所需法力: {costMana})", false);
                        continue;
                    }
                    var target = SelectTarget(allies, "治療");
                    if (target != null && target is Player targetPlayer) { Heal(player, targetPlayer); acted = true; }
                }
                else
                {
                    BattleUI.Log("無效的選擇，請重新選擇行動。", false);
                }
            }
        }

        // 開啟戰鬥
        public void StartBattle(Player[] players, Enemy[] enemies)
        {
            BattleUI.Log("戰鬥開始！");

            // 建立行動順序清單 (按敏捷排序)
            var turnOrder = new List<Battler>();
            turnOrder.AddRange(players);
            turnOrder.AddRange(enemies);

            while (players.Any(p => p.Health > 0) && enemies.Any(e => e.Health > 0))
            {
                // 每回合重新按敏捷排序 (應對可能的戰鬥中數值變化)
                var currentRoundOrder = turnOrder
                    .Where(b => b.Health > 0)
                    .OrderByDescending(b => b.Agility)
                    .ToList();

                foreach (var actor in currentRoundOrder)
                {
                    if (players.All(p => p.Health <= 0) || enemies.All(e => e.Health <= 0)) break;
                    if (actor.Health <= 0) continue;

                    ExecuteTurn(actor, players, enemies);
                }
            }

            if (players.Any(p => p.Health > 0))
                BattleUI.Log("[blue]戰鬥勝利！[/blue]");
            else
                BattleUI.Log("[darkmagenta]全軍覆沒……[/darkmagenta]");
        }

        private Battler? SelectTarget(List<Battler> targets, string actionName)
        {
            if (targets.Count == 0) return null;
            if (targets.Count == 1) return targets[0];

            while (true)
            {
                BattleUI.Log($"\n請選擇{actionName}對象 (0. 返回):", false);
                for (int i = 0; i < targets.Count; i++)
                    BattleUI.Log($"{i + 1}. {targets[i].Name} (HP: {targets[i].Health} / {targets[i].MaxHealth})", false);
                if (int.TryParse(Console.ReadLine(), out int index))
                {
                    if (index == 0) return null;
                    if (index > 0 && index <= targets.Count) return targets[index - 1];
                }
            }
        }

        private void CheckAllDeaths(Player[] players, Enemy[] enemies)
        {
            var allParticipants = players.Cast<Battler>().Concat(enemies);

            foreach (var b in allParticipants)
            {
                // 偵測死亡：血量為 0 且還沒被記錄
                if (b.Health <= 0 && !DeadUnitsThisBattle.Contains(b))
                {
                    string color = (b is Player) ? "blue" : "darkmagenta";
                    BattleUI.Log($"☠️ [{color}]{b.Name}[/{color}]倒下了！", wait: false);
                    DeadUnitsThisBattle.Add(b);
                }

                // 偵測復活：血量大於 0 且竟然在死亡名單內 (我方成員有復活的可能)
                // 這樣即便不改 Heal 方法，這裡也會自動幫你把標記洗掉
                else if (b.Health > 0 && DeadUnitsThisBattle.Contains(b))
                {
                    DeadUnitsThisBattle.Remove(b);
                }
            }
        }
    }
}