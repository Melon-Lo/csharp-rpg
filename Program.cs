using RPG.Models;
using RPG.Logic;

class Program
{
    static void Main(string[] args)
    {
        Player player = new Player(name: "小明", health: 100, maxHealth: 100, attackPower: 20, healPower: 100, speed: 2);
        Enemy Enemy = new Enemy(name: "哥布林", health: 100, maxHealth: 100, attackPower: 25, speed: 1);

        BattleLogic game = new BattleLogic();
        game.StartBattle(player, Enemy);
    }
}