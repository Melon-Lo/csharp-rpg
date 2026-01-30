namespace RPG.Models
{
    public class Battler
    {
        private int _health;

        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int AttackPower { get; set; }
        public int Defense { get; set; }
        public int CriticalRate { get; set; }
        public int Agility { get; set; }
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
            int defense,
            int criticalRate,
            int agility
        )
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = health;
            AttackPower = attackPower;
            Defense = defense;
            CriticalRate = criticalRate;
            Agility = agility;
        }
    }
}