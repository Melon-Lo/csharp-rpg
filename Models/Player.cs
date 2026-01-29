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

        public Weapon Weapon { get; set; }

        public override int TotalAttackPower => AttackPower + (Weapon?.AttackBonus ?? 0);

        public Player(
            string name,
            int health,
            int maxHealth,
            int healPower,
            int attackPower,
            int defense,
            int criticalRate,
            int agility,
            Weapon weapon
        ) : base(
            name: name,
            health: health,
            maxHealth: maxHealth,
            attackPower: attackPower,
            defense: defense,
            criticalRate: criticalRate,
            agility: agility
        )
        {
            HealPower = healPower;
            Weapon = weapon;
        }
    }
}