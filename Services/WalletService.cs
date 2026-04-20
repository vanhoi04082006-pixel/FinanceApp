/*
 * Tên file : WalletService.cs
 * Người tạo : Bùi Văn Hội
 * Mục đích : Điều khiển thao tác ví. Đã áp dụng Factory Pattern.
 * Version   : 2.0 (Pro Architecture)
 */
using FinanceApp.Core.Factories; // Gọi thư mục nhà máy
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

        // Băng chuyền kết nối các nhà máy 🏭
        private readonly Dictionary<string, IFinanceFactory> _factories;

        public WalletService()
        {
            _data = DatabaseContext.Instance;

            // Đăng ký các nhà máy vào hệ thống
            // Nhập "cash" -> Gọi CashFactory | Nhập "card" -> Gọi CardFactory
            _factories = new Dictionary<string, IFinanceFactory>(StringComparer.OrdinalIgnoreCase)
            {
                { "cash", new CashFactory() },
                { "card", new CardFactory() }
            };
        }

        public void CreateWallet(string type, string name, decimal balance)
        {
            string newId = GenerateWalletId();

            // KIỂM TRA: Hệ thống có nhà máy nào tên như chữ 'type' người dùng nhập không?
            if (_factories.TryGetValue(type, out IFinanceFactory factory))
            {
                // Nếu có, ra lệnh cho nhà máy đó sản xuất ví
                Wallet newWallet = factory.CreateWallet(newId, name, balance);

                // Cất vào kho và lưu file
                _data.Wallets.Add(newWallet);
                _data.SaveChanges();
            }
            else
            {
                Console.WriteLine($"❌ Hệ thống chưa hỗ trợ tạo loại ví '{type}' này.");
            }
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
            _data.SaveChanges();
            return (true, "Đã xóa ví thành công.");
        }

        public List<Wallet> GetAllWallets()
        {
            return _data.Wallets;
        }
    }
}