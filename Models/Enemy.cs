namespace RPG.Models
{
    public class Enemy : Battler
    {
        public Enemy(string name, int health, int maxHealth, int attackPower, int speed)
            : base(name: name, health: health, maxHealth: maxHealth, attackPower: attackPower, speed: speed)
        { }
    }
}