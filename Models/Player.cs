namespace RPG.Models
{
    public class Player : Battler
    {
        private int _healPower;

        public int HealPower
        {
            get => _healPower;
            set => _healPower = value;
        }

        public Player(string name, int health, int maxHealth, int healPower, int attackPower, int speed)
            : base(name: name, health: health, maxHealth: maxHealth, attackPower: attackPower, speed: speed)
        {
            HealPower = healPower;
        }
    }
}