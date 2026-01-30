namespace RPG.Models
{
    public class Player : Battler
    {
        private int _mana;
        private int _healPower;

        public int MaxMana { get; set; }
        public int Mana
        {
            get => _mana;
            set => _mana = Math.Clamp(value, 0, MaxMana);
        }
        public int HealPower
        {
            get => _healPower;
            set => _healPower = value;
        }
        public float ManaPercentage => (float)Math.Round((float)_mana / MaxMana * 100, 1);

        public Weapon Weapon { get; set; }
        public override int TotalAttackPower => AttackPower + (Weapon?.AttackBonus ?? 0);

        public Player(
            string name,
            int maxHealth,
            int health,
            int maxMana,
            int mana,
            int healPower,
            int attackPower,
            int defense,
            int criticalRate,
            int agility,
            Weapon weapon
        ) : base(
            name: name,
            maxHealth: maxHealth,
            health: health,
            attackPower: attackPower,
            defense: defense,
            criticalRate: criticalRate,
            agility: agility
        )
        {
            HealPower = healPower;
            Weapon = weapon;
            MaxMana = maxMana;
            Mana = mana;
        }
    }
}