using System.Text.Json;
using RPG.Models;

namespace RPG.Data
{
    public class WeaponDatabase
    {
        private static List<Weapon> _weapons = new List<Weapon>();

        public static void Initialize()
        {
            try
            {
                string jsonString = File.ReadAllText("Data/weapons.json");
                _weapons = JsonSerializer.Deserialize<List<Weapon>>(jsonString) ?? new List<Weapon>();

                // Console.WriteLine($"成功載入 {_weapons.Count} 把武器");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"讀取 JSON 失敗: {ex.Message}");
                throw;
            }
        }

        public static Weapon GetById(int id)
        {
            return _weapons.FirstOrDefault(w => w.Id == id) ?? new Weapon("空手", 0);
        }
    }
}