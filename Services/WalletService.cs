/*
 * Tên file : WalletService.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Điều khiển các thao tác với ví và lưu trữ thay đổi.
 * Version   : 1.2 (Tích hợp Auto-Save JSON)
 */
using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceApp.Services
{
    public class WalletService
    {
        private DatabaseContext _data;

        public WalletService()
        {
            _data = DatabaseContext.Instance;
        }

        public void CreateWallet(string type, string name, decimal balance)
        {
            string newId = GenerateWalletId();

            if (type.ToLower() == "cash")
            {
                _data.Wallets.Add(new CashWallet { Id = newId, Name = name, Balance = balance });
            }
            else if (type.ToLower() == "card")
            {
                _data.Wallets.Add(new CardWallet { Id = newId, Name = name, Balance = balance });
            }

            // LƯU THAY ĐỔI: Sau khi thêm ví mới
            _data.SaveChanges();
        }

        private string GenerateWalletId()
        {
            if (_data.Wallets.Count == 0) return "W1";

            string lastId = _data.Wallets[_data.Wallets.Count - 1].Id;
            string numberPart = lastId.Substring(1);
            int nextNumber = int.Parse(numberPart) + 1;

            return "W" + nextNumber;
        }

        public bool UpdateWallet(string id, string newName)
        {
            var wallet = _data.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet != null)
            {
                wallet.Name = newName;

                // LƯU THAY ĐỔI: Sau khi sửa tên ví
                _data.SaveChanges();
                return true;
            }
            return false;
        }

        public (bool success, string message) DeleteWallet(string id)
        {
            var wallet = _data.Wallets.FirstOrDefault(w => w.Id == id);
            if (wallet == null) return (false, "Không tìm thấy ví!");

            if (wallet.Balance > 0)
            {
                return (false, $"Không thể xóa ví '{wallet.Name}' vì vẫn còn {wallet.Balance:N0} VND.");
            }

            _data.Wallets.Remove(wallet);

            // LƯU THAY ĐỔI: Sau khi xóa ví
            _data.SaveChanges();
            return (true, "Đã xóa ví thành công.");
        }

        public List<Wallet> GetAllWallets()
        {
            return _data.Wallets;
        }
    }
}