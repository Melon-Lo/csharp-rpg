using RPG.Models;
using RPG.Logic;
using RPG.Data;

class Program
{
    static void Main(string[] args)
    {
        WeaponDatabase.Initialize();

        Player player = new Player(
            name: "小明",
            health: 100,
            maxHealth: 100,
            attackPower: 20,
            defense: 10,
            healPower: 100,
            criticalRate: 100,
            agility: 2,
            weapon: WeaponDatabase.GetById(1)
        );

        Enemy enemy = new Enemy(
            name: "史萊姆",
            health: 50,
            maxHealth: 50,
            attackPower: 25,
            defense: 5,
            criticalRate: 50,
            agility: 1
        );

        BattleSystem game = new BattleSystem();
        game.StartBattle(player, enemy);
    }
}