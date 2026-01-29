namespace RPG.Models
{
    public class Battler
    {
        private int _health;
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int AttackPower { get; set; }
        public int Speed { get; set; }
        public float HealthPercentage => (float)Math.Round((float)_health / MaxHealth * 100, 1);

        public int Health
        {
            get => _health;
            set => _health = Math.Clamp(value, 0, MaxHealth);
        }

        public virtual int TotalAttackPower => AttackPower;

        public Battler(
            string name,
            int health,
            int maxHealth,
            int attackPower,
            int speed
        )
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            AttackPower = attackPower;
            Speed = speed;
        }
    }
}