using RPG.Models;
using RPG.Logic;
using RPG.Data;

class Program
{
    static void Main(string[] args)
    {
        WeaponDatabase.Initialize();

        Player player1 = new Player(
            name: "小明",
            maxHealth: 100,
            health: 100,
            maxMana: 10,
            mana: 10,
            attackPower: 20,
            defense: 10,
            healPower: 100,
            criticalRate: 100,
            agility: 2,
            weapon: WeaponDatabase.GetById(1)
        );

        Player player2 = new Player(
            name: "小美",
            maxHealth: 20,
            health: 20,
            maxMana: 10,
            mana: 10,
            attackPower: 20,
            defense: 10,
            healPower: 100,
            criticalRate: 100,
            agility: 2,
            weapon: WeaponDatabase.GetById(1)
        );

        Enemy enemy1 = new Enemy(
            name: "小地瓜",
            maxHealth: 50,
            health: 50,
            attackPower: 25,
            defense: 5,
            criticalRate: 50,
            agility: 1
        );

        Enemy enemy2 = new Enemy(
            name: "小南瓜",
            maxHealth: 50,
            health: 50,
            attackPower: 25,
            defense: 5,
            criticalRate: 50,
            agility: 1
        );

        BattleSystem game = new BattleSystem();
        game.StartBattle(new Player[] { player1, player2 }, new Enemy[] { enemy1, enemy2 });
    }
}