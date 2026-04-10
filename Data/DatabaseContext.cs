/*
 * Tên file : DatabaseContext.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : "Nhà kho" trung tâm (Singleton) lưu trữ danh sách Ví và Hạng mục.
 * Version   : 1.1 (Cập nhật ID Hạng mục sang kiểu String)
 */
using FinanceApp.Core.Models;
using System.Collections.Generic;

namespace FinanceApp.Data
{
    public sealed class DatabaseContext
    {
        // 1. Biến static để giữ thực thể duy nhất của nhà kho (Singleton Pattern) 🏠
        private static DatabaseContext _instance;

        // 2. Các danh sách quản lý dữ liệu chính
        public List<Wallet> Wallets { get; set; }
        public List<Category> Categories { get; set; }

        // 3. Hàm tạo riêng tư (Private Constructor)
        private DatabaseContext()
        {
            Wallets = new List<Wallet>();
            Categories = new List<Category>();

            // --- MỒI DỮ LIỆU MẪU (SEED DATA) --- 🧶

            // Tạo sẵn danh sách hạng mục để người dùng chọn theo số (1, 2, 3...)
            Categories.Add(new Category("FOOD", "Ăn uống 🍕"));
            Categories.Add(new Category("TRANSPORT", "Di chuyển 🚗"));
            Categories.Add(new Category("SALARY", "Tiền lương 💵"));
            Categories.Add(new Category("RENT", "Tiền nhà 🏠"));

            // Thêm một vài chiếc ví mặc định để test
            Wallets.Add(new CashWallet { Id = "W1", Name = "Ví Tiền Mặt", Balance = 100000 });
            Wallets.Add(new CardWallet { Id = "W2", Name = "Thẻ ATM", Balance = 0 });
        }

        // 4. Cổng duy nhất để truy cập vào DatabaseContext từ bất cứ đâu
        public static DatabaseContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseContext();
                }
                return _instance;
            }
        }
    }
}