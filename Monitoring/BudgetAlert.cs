using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Monitoring
{
    public class BudgetAlert : IObserver
    {
        private Wallet _wallet;      // "Chìa khóa" dẫn đến chiếc ví
        private string _categoryName;
        private decimal _limit;

        // Cập nhật Constructor để nhận thêm ví
        public BudgetAlert(Wallet wallet, string categoryName, decimal limit)
        {
            _wallet = wallet;
            _categoryName = categoryName;
            _limit = limit;
        }

        public void Update(Transaction trans)
        {
            // 1. Lọc: Chỉ kiểm tra nếu đúng hạng mục thám tử này đang canh giữ
            if (trans.Type == TransactionType.Expense && trans.Category.Name == _categoryName)
            {
                decimal totalSpent = 0;

                // 2. Tính toán: Duyệt lịch sử ví để cộng dồn tiền
                foreach (var t in _wallet.Transactions)
                {
                    if (t.Type == TransactionType.Expense && t.Category.Name == _categoryName)
                    {
                        totalSpent += t.Amount;
                    }
                }

                // 3. Kết luận: So sánh với hạn mức sau khi đã tính xong tổng
                if (totalSpent >= _limit)
                {
                    Console.WriteLine($"\n[CẢNH BÁO] 🚨 Hạng mục '{_categoryName}' đã tiêu: {totalSpent:N0} VNĐ.");
                    Console.WriteLine($"Hạn mức cho phép là: {_limit:N0} VNĐ. Bạn đã tiêu quá tay rồi nhé!");
                }
            }
        }
    }
}
