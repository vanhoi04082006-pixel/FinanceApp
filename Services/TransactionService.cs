/*
 * Tên file : TransactionService.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Quản lý logic giao dịch, cập nhật số dư và lưu trữ lịch sử tự động.
 * Version   : 1.2 (Tích hợp Auto-Save JSON)
 */
using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using FinanceApp.Monitoring;
using System;
using System.Collections.Generic;

namespace FinanceApp.Services
{
    public class TransactionService
    {
        // Kết nối với "nhà kho" dữ liệu duy nhất
        private DatabaseContext _data = DatabaseContext.Instance;

        // Danh sách các "thám tử" (Observer) đang theo dõi hệ thống
        private List<IObserver> _observers = new List<IObserver>();

        public bool AddTransaction(string walletId, string categoryId, decimal amount, TransactionType type, string note)
        {
            // 1. Tìm ví và hạng mục từ ID
            Wallet wallet = FindWalletById(walletId);
            Category category = FindCategoryById(categoryId);

            // 2. Kiểm tra tính hợp lệ của dữ liệu 🛡️
            if (wallet == null || category == null)
            {
                Console.WriteLine("Lỗi: Ví hoặc Hạng mục không tồn tại! ❌");
                return false;
            }

            // 3. Xử lý logic cộng/trừ số dư trong ví 
            if (type == TransactionType.Expense)
            {
                if (wallet.Balance >= amount)
                {
                    wallet.Balance -= amount;
                }
                else
                {
                    Console.WriteLine("Số dư không đủ để thực hiện chi tiêu! ❌");
                    return false;
                }
            }
            else
            {
                wallet.Balance += amount;
            }

            // 4. Khởi tạo đối tượng giao dịch mới
            int newId = wallet.Transactions.Count + 1;
            Transaction newTransaction = new Transaction(newId, amount, DateTime.Now, note, type, category);

            // 5. Lưu "biên lai" vào danh sách của ví 
            wallet.Transactions.Add(newTransaction);

            // BƯỚC 6: LƯU THAY ĐỔI VÀO FILE JSON 💾
            // Lưu lại số dư mới và giao dịch mới vừa tạo
            _data.SaveChanges();

            // 7. Sau khi lưu thành công, mới "phát loa" thông báo cho các thám tử (BudgetAlert, GoalTracker)
            Notify(newTransaction);

            return true;
        }

        public List<Transaction> GetTransactionHistory(string walletId)
        {
            Wallet wallet = FindWalletById(walletId);
            return wallet != null ? wallet.Transactions : new List<Transaction>();
        }

        // --- Phương thức hỗ trợ tìm kiếm ---
        public Wallet FindWalletById(string id)
        {
            foreach (var wallet in _data.Wallets)
            {
                if (wallet.Id == id) return wallet;
            }
            return null;
        }

        public Category FindCategoryById(string id)
        {
            foreach (var category in _data.Categories)
            {
                if (category.Id == id) return category;
            }
            return null;
        }

        // --- Hệ thống Observer ---
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        private void Notify(Transaction trans)
        {
            foreach (var observer in _observers)
            {
                observer.Update(trans);
            }
        }
    }
}