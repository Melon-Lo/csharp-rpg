namespace RPG.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AttackBonus { get; set; }

        public Weapon(string name, int attackBonus)
        {
            Name = name;
            AttackBonus = attackBonus;
        }
    }
}