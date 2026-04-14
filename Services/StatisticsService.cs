/*
 * Tên file : StatisticsService.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 12/04/2026
 * Mục đích : Tính toán thống kê tài chính bằng thuật toán duyệt mảng thủ công.
 * Version   : 1.0
 */
using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Collections.Generic;

namespace FinanceApp.Services
{
    public class StatisticsService
    {
        private DatabaseContext _data = DatabaseContext.Instance;

        // --- 1. TỔNG THU / TỔNG CHI ---
        public decimal GetTotalAmount(TransactionType type)
        {
            decimal total = 0;

            foreach (var wallet in _data.Wallets)
            {
                foreach (var trans in wallet.Transactions)
                {
                    // FIX BUG: Bỏ qua các giao dịch có Category == null (Chuyển tiền nội bộ)
                    if (trans.Type == type && trans.Category != null)
                    {
                        total += trans.Amount;
                    }
                }
            }
            return total;
        }

        // --- 2. THỐNG KÊ THEO THÁNG / NĂM ---
        public decimal GetTotalByTime(TransactionType type, int month, int year)
        {
            decimal total = 0;

            foreach (var wallet in _data.Wallets)
            {
                foreach (var trans in wallet.Transactions)
                {
                    // FIX BUG: Bỏ qua các giao dịch có Category == null (Chuyển tiền nội bộ)
                    if (trans.Type == type &&
                        trans.Category != null &&
                        trans.Date.Month == month &&
                        trans.Date.Year == year)
                    {
                        total += trans.Amount;
                    }
                }
            }
            return total;
        }

        // --- 3. THỐNG KÊ THEO HẠNG MỤC (CATEGORY) ---
        // Hàm này trả về một "Từ điển" chứa: Tên hạng mục -> Tổng tiền
        public Dictionary<string, decimal> GetStatisticsByCategory(TransactionType type)
        {
            // Key: Tên hạng mục, Value: Tổng tiền
            Dictionary<string, decimal> stats = new Dictionary<string, decimal>();

            foreach (var wallet in _data.Wallets)
            {
                foreach (var trans in wallet.Transactions)
                {
                    // Chỉ thống kê cho đúng loại (ví dụ: chỉ thống kê chi tiêu)
                    if (trans.Type == type && trans.Category != null)
                    {
                        string categoryName = trans.Category.Name;

                        // Nếu trong từ điển đã có tên hạng mục này rồi thì cộng thêm tiền
                        if (stats.ContainsKey(categoryName))
                        {
                            stats[categoryName] += trans.Amount;
                        }
                        // Nếu chưa có thì tạo mới một dòng trong từ điển
                        else
                        {
                            stats.Add(categoryName, trans.Amount);
                        }
                    }
                }
            }
            return stats;
        }

        // --- 4. TÌM HẠNG MỤC CHI NHIỀU NHẤT ---
        public string GetTopSpendingCategory()
        {
            // Lấy danh sách thống kê chi tiêu theo hạng mục
            var categoryStats = GetStatisticsByCategory(TransactionType.Expense);

            string topCategory = "Không có dữ liệu";
            decimal maxAmount = -1;

            // Thuật toán tìm Max trong Dictionary
            foreach (var item in categoryStats)
            {
                if (item.Value > maxAmount)
                {
                    maxAmount = item.Value;
                    topCategory = item.Key;
                }
            }

            return maxAmount > 0 ? $"{topCategory} ({maxAmount:N0} VND)" : topCategory;
        }
    }
}