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
            healPower: 100,
            speed: 2,
            weapon: WeaponDatabase.GetById(1)
        );

        Enemy enemy = new Enemy(
            name: "史萊姆",
            health: 100,
            maxHealth: 100,
            attackPower: 25,
            speed: 1
        );

        BattleSystem game = new BattleSystem();
        game.StartBattle(player, enemy);
    }
}