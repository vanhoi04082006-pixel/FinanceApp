/*
 * Tên file : TransactionService.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Chứa các logic cho chức năng giao dịch.
 * Version : 1.0
 */
using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Collections.Generic;

namespace FinanceApp.Services
{
    public class TransactionService
    {
        // Kết nối với "nhà kho" dữ liệu duy nhất
        private DatabaseContext _data = DatabaseContext.Instance;

        // Cập nhật phương thức để nhận thêm tham số 'note' (ghi chú)
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

            // 4. Khởi tạo đối tượng giao dịch mới với ĐỦ 6 tham số:
            // (id, amount, date, note, type, category)
            int newId = wallet.Transactions.Count + 1; // Tự tạo ID bằng số thứ tự
            Transaction newTransaction = new Transaction(newId, amount, DateTime.Now, note, type, category);

            // 5. Lưu "biên lai" vào lịch sử của ví 
            wallet.Transactions.Add(newTransaction);
            return true;
        }

        public List<Transaction> GetTransactionHistory(string walletId)
        {
            // 1. Tìm ví dựa vào ID
            Wallet wallet = FindWalletById(walletId);

            // 2. Kiểm tra xem ví có tồn tại không
            if (wallet != null)
            {
                // 3. Trả về danh sách giao dịch của ví này
                return wallet.Transactions;
            }
            else
            {
                // Trả về danh sách rỗng nếu không thấy ví
                return new List<Transaction>();
            }
        }

        // Phương thức hỗ trợ tìm ví theo mã ID 
        public Wallet FindWalletById(string id)
        {
            foreach (var wallet in _data.Wallets)
            {
                if (wallet.Id == id) return wallet;
            }
            return null;
        }

        // Phương thức hỗ trợ tìm hạng mục theo mã ID 
        public Category FindCategoryById(string id)
        {
            foreach (var category in _data.Categories)
            {
                if (category.Id == id) return category;
            }
            return null;
        }
    }
}