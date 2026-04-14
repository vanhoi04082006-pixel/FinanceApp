/*
 * Tên file : DatabaseContext.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : "Nhà kho" trung tâm tích hợp khả năng lưu trữ file JSON.
 * Version   : 1.2 (Tích hợp FileHandler & Persistence)
 */
using FinanceApp.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace FinanceApp.Data
{
    public sealed class DatabaseContext
    {
        // 1. Singleton Pattern 🏠
        private static DatabaseContext _instance;
        public static DatabaseContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DatabaseContext();
                return _instance;
            }
        }
        // 2. Các danh sách quản lý dữ liệu chính
        public List<Wallet> Wallets { get; set; }
        public List<Category> Categories { get; set; }

        // 3. Người trợ lý lo việc đọc/ghi file 📂
        private readonly FileHandler _fileHandler = new FileHandler();

        // 4. Hàm tạo riêng tư (Private Constructor)
        private DatabaseContext()
        {
            // Bước 1: Thử nạp dữ liệu từ file lên
            var data = _fileHandler.LoadDataFromFile();

            Wallets = data.Wallets ?? new List<Wallet>();
            Categories = data.Categories ?? new List<Category>();

            // Bước 2: Nếu là lần đầu chạy (chưa có dữ liệu), tạo mồi dữ liệu mẫu (Seed Data)
            if (Categories.Count == 0 && Wallets.Count == 0)
            {
                SeedData();
                // Lưu lại ngay file đầu tiên này
                SaveChanges();
            }
        }

        // --- MỒI DỮ LIỆU MẪU (CHỈ CHẠY 1 LẦN DUY NHẤT) --- 🧶
        private void SeedData()
        {
            // Tạo sẵn danh sách hạng mục
            Categories.Add(new Category("FOOD", "Ăn uống 🍕"));
            Categories.Add(new Category("TRANSPORT", "Di chuyển 🚗"));
            Categories.Add(new Category("SALARY", "Tiền lương 💵"));
            Categories.Add(new Category("RENT", "Tiền nhà 🏠"));

            // Thêm ví mặc định
            Wallets.Add(new CashWallet { Id = "W1", Name = "Ví Tiền Mặt", Balance = 100000 });
            Wallets.Add(new CardWallet { Id = "W2", Name = "Thẻ ATM", Balance = 0 });
        }

        // 5. Hàm lưu thay đổi vào ổ cứng 💾
        // Gọi hàm này bất cứ khi nào bạn Thêm, Sửa hoặc Xóa dữ liệu
        public void SaveChanges()
        {
            var dataToSave = new SaveData
            {
                Wallets = this.Wallets,
                Categories = this.Categories
            };
            _fileHandler.SaveDataToFile(dataToSave);
        }
    }
}