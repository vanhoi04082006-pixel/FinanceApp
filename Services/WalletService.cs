/*
 * Tên file : TransactionType.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Đây là nơi chứa các quy tắc để thao tác với ví.
 * Version : 1.0
 */
using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Services
{
    public class WalletService
    {
        // Biến này đóng vai trò là "kết nối nội bộ" đến nhà kho dữ liệu 
        private DatabaseContext _data;

        public WalletService()
        {
            // Lấy chìa khóa duy nhất và cất vào biến _data
            _data = DatabaseContext.Instance;
        }
        // Xóa tham số 'id' ở đây, chỉ giữ lại type, name và balance
        // Không còn tham số 'string id' ở đây nữa nhé
        public void CreateWallet(string type, string name, decimal balance)
        {
            // 1. Máy tính tự động sản xuất mã ID mới ngay ở đầu
            string newId = GenerateWalletId();

            // 2. Truyền mã newId vừa tạo vào vị trí của thuộc tính Id
            if (type.ToLower() == "cash")
            {
                _data.Wallets.Add(new CashWallet { Id = newId, Name = name, Balance = balance });
            }
            else if (type.ToLower() == "card")
            {
                _data.Wallets.Add(new CardWallet { Id = newId, Name = name, Balance = balance });
            }
        }

        private string GenerateWalletId()
        {
            // 1. Nếu kho chưa có chiếc ví nào, mặc định trả về ID đầu tiên
            if (_data.Wallets.Count == 0)
            {
                return "W1";
            }

            // 2. Tìm ID của chiếc ví cuối cùng trong danh sách (Ví dụ: "W5")
            string lastId = _data.Wallets[_data.Wallets.Count - 1].Id;

            // 3. Bỏ chữ 'W' ở đầu để lấy phần số (Lấy ra chuỗi "5")
            string numberPart = lastId.Substring(1);

            // 4. Chuyển chuỗi "5" thành số nguyên (int) rồi cộng thêm 1 thành 6
            int nextNumber = int.Parse(numberPart) + 1;

            // 5. Ghép chữ 'W' trở lại với con số mới và trả về (Thành "W6")
            return "W" + nextNumber;
        }

        public bool UpdateWallet(string id, string newName)
        {
            var wallet = _data.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet != null)
            {
                wallet.Name = newName;
                return true;
            }
            return false;
        }

        public (bool success, string message) DeleteWallet(string id)
        {
            var wallet = _data.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null) return (false, "Không tìm thấy ví!");

            // Logic bảo vệ: Không cho xóa nếu còn tiền
            if (wallet.Balance > 0)
            {
                return (false, $"Không thể xóa ví '{wallet.Name}' vì vẫn còn {wallet.Balance:N0} VND. Hãy tiêu hết tiền hoặc chuyển sang ví khác trước!");
            }

            _data.Wallets.Remove(wallet);
            return (true, "Đã xóa ví thành công.");
        }

        public List<Wallet> GetAllWallets()
        {
            // Ta cần lấy danh sách ví từ "nhà kho" và trả về
            return _data.Wallets;
        }
    }
}