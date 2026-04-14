/*
 * Tên file : TransferService.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Xử lý logic chuyển tiền giữa các ví và ghi lại lịch sử đối ứng.
 * Version   : 1.2 (Tích hợp Auto-Save JSON)
 */
using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Linq;

namespace FinanceApp.Services
{
    public class TransferService
    {
        // Kết nối với "nhà kho" dữ liệu
        private DatabaseContext _data = DatabaseContext.Instance;

        // Hàm thực hiện chuyển tiền
        public bool Transfer(string fromWalletId, string toWalletId, decimal amount, string note)
        {
            // Bước 1: Tìm ví nguồn và ví đích
            var fromWallet = _data.Wallets.FirstOrDefault(w => w.Id == fromWalletId);
            var toWallet = _data.Wallets.FirstOrDefault(w => w.Id == toWalletId);

            // Bước 2: Kiểm tra lỗi bảo mật
            if (fromWallet == null || toWallet == null)
            {
                Console.WriteLine("❌ Lỗi: Không tìm thấy ví nguồn hoặc ví đích!");
                return false;
            }

            if (fromWallet.Balance < amount)
            {
                Console.WriteLine("❌ Lỗi: Số dư ví nguồn không đủ để thực hiện giao dịch!");
                return false;
            }

            // Bước 3: Dịch chuyển dòng tiền
            fromWallet.Balance -= amount;
            toWallet.Balance += amount;

            // Bước 4: Lưu lịch sử (Tạo 2 biên lai)
            // 4.1 Biên lai ví nguồn (Tiền đi ra -> Expense)
            int fromId = fromWallet.Transactions.Count + 1;
            Transaction fromTrans = new Transaction(fromId, amount, DateTime.Now, note, TransactionType.Expense, null);
            fromWallet.Transactions.Add(fromTrans);

            // 4.2 Biên lai ví đích (Tiền đi vào -> Income)
            int toId = toWallet.Transactions.Count + 1;
            Transaction toTrans = new Transaction(toId, amount, DateTime.Now, note, TransactionType.Income, null);
            toWallet.Transactions.Add(toTrans);

            // BƯỚC 5: LƯU THAY ĐỔI VÀO FILE JSON 💾
            // Một lần gọi này sẽ lưu toàn bộ trạng thái mới của cả 2 ví và các giao dịch vừa thêm
            _data.SaveChanges();

            return true; // Thành công!
        }
    }
}