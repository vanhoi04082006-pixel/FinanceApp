using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using FinanceApp.Core.Models;

namespace FinanceApp.Data
{
    public class SaveData
    {
        public List<Wallet> Wallets { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class FileHandler
    {
        private const string _filePath = "database.json";

        // Hàm lưu: Cất dữ liệu vào ổ cứng
        public void SaveDataToFile(SaveData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, jsonString);
        }

        // Hàm đọc: Lấy dữ liệu từ ổ cứng ra
        public SaveData LoadDataFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new SaveData
                {
                    Wallets = new List<Wallet>(),
                    Categories = new List<Category>()
                };
            }

            string jsonString = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<SaveData>(jsonString);
        }
    }
}