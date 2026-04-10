using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using FinanceApp.Services;
using System;

namespace FinanceApp.UI
{
    public class MenuManager
    {
        // Khai báo các biến để chứa các Service được truyền vào từ Program.cs
        private WalletService _walletService;
        private TransactionService _transactionService;
        private CategoryService _categoryService;
        private TransferService _transferService;
        private DatabaseContext _data;

        // Hàm khởi tạo (Constructor) nhận các Service
        public MenuManager(WalletService walletService, TransactionService transactionService, CategoryService categoryService, TransferService transferService, DatabaseContext data)
        {
            _walletService = walletService;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _transferService = transferService;
            _data = data;
        }

        // Hàm chính để chạy Menu
        public void Start()
        {
            bool running = true;

            while (running)
            {
                ShowMenu();
                Console.Write("👉 Chọn chức năng: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": CreateWallet(); break;
                    case "2": ShowWallets(); break;
                    case "3": UpdateWallet(); break;
                    case "4": DeleteWallet(); break;

                    case "5": AddCategory(); break;
                    case "6": ShowCategories(); break;
                    case "7": UpdateCategory(); break;
                    case "8": DeleteCategory(); break;

                    case "9": AddTransaction(); break;
                    case "10": ShowTransactionHistory(); break;
                    case "11": TransferMoney(); break;
                    case "0":
                        running = false;
                        Console.WriteLine("👋 Thoát chương trình...");
                        break;

                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                        break;
                }
            }
        }

        // ===== MENU =====
        private void ShowMenu()
        {
            Console.WriteLine("\n===== QUẢN LÝ TÀI CHÍNH =====");
            Console.WriteLine("1. Tạo ví");
            Console.WriteLine("2. Xem ví");
            Console.WriteLine("3. Sửa ví");
            Console.WriteLine("4. Xóa ví");
            Console.WriteLine("-----");
            Console.WriteLine("5. Thêm hạng mục");
            Console.WriteLine("6. Xem hạng mục");
            Console.WriteLine("7. Sửa hạng mục");
            Console.WriteLine("8. Xóa hạng mục");
            Console.WriteLine("-----");
            Console.WriteLine("9. Thêm giao dịch");
            Console.WriteLine("10. Xem lịch sử giao dịch");
            Console.WriteLine("11. Chuyển tiền giữa các ví");
            Console.WriteLine("0. Thoát");
        }

        // ===== WALLET =====
        private void CreateWallet()
        {
            Console.Write("Tên ví: ");
            string name = Console.ReadLine();

            Console.Write("Loại (cash/card): ");
            string type = Console.ReadLine();

            Console.Write("Số dư: ");
            decimal balance = decimal.Parse(Console.ReadLine());

            _walletService.CreateWallet(type, name, balance);
            Console.WriteLine("✅ Tạo ví thành công!");
        }

        private void ShowWallets()
        {
            var wallets = _walletService.GetAllWallets();

            if (wallets.Count == 0)
            {
                Console.WriteLine("📭 Không có ví nào.");
                return;
            }

            foreach (var w in wallets)
                Console.WriteLine($"ID: {w.Id} | {w.Name} | {w.Balance:N0} VND");
        }

        private void UpdateWallet()
        {
            Console.Write("ID ví: ");
            string id = Console.ReadLine();

            Console.Write("Tên mới: ");
            string name = Console.ReadLine();

            Console.WriteLine(_walletService.UpdateWallet(id, name)
                ? "✨ Cập nhật thành công!"
                : "❌ Không tìm thấy!");
        }

        private void DeleteWallet()
        {
            Console.Write("ID ví: ");
            string id = Console.ReadLine();

            var result = _walletService.DeleteWallet(id);
            Console.WriteLine(result.success ? $"✅ {result.message}" : $"❌ {result.message}");
        }

        // ===== CATEGORY =====
        private void AddCategory()
        {
            Console.Write("Tên hạng mục: ");
            string name = Console.ReadLine();

            Console.WriteLine(_categoryService.AddCategory(name)
                ? "✅ Thành công!"
                : "❌ Đã tồn tại!");
        }

        private void ShowCategories()
        {
            var list = _categoryService.GetAllCategories();
            int i = 1;

            foreach (var c in list)
                Console.WriteLine($"{i++}. {c.Name} ({c.Id})");
        }

        private void UpdateCategory()
        {
            Console.Write("ID: ");
            string id = Console.ReadLine();

            Console.Write("Tên mới: ");
            string name = Console.ReadLine();

            Console.WriteLine(_categoryService.UpdateCategory(id, name)
                ? "✨ Thành công!"
                : "❌ Không tìm thấy!");
        }

        private void DeleteCategory()
        {
            Console.Write("ID: ");
            string id = Console.ReadLine();

            Console.WriteLine(_categoryService.DeleteCategory(id)
                ? "✅ Đã xóa!"
                : "❌ Không tìm thấy!");
        }

        // ===== TRANSACTION =====
        private void AddTransaction()
        {
            Console.Write("Wallet ID: ");
            string wId = Console.ReadLine();

            Console.WriteLine("Chọn hạng mục:");
            for (int i = 0; i < _data.Categories.Count; i++)
                Console.WriteLine($"{i + 1}. {_data.Categories[i].Name}");

            int choice = int.Parse(Console.ReadLine());
            string cId = _data.Categories[choice - 1].Id;

            Console.Write("Số tiền: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Ghi chú: ");
            string note = Console.ReadLine();

            Console.Write("Loại (1: Thu, 2: Chi): ");
            string typeInput = Console.ReadLine();

            var type = typeInput == "1" ? TransactionType.Income : TransactionType.Expense;

            Console.WriteLine(_transactionService.AddTransaction(wId, cId, amount, type, note)
                ? "🎉 Thành công!"
                : "❌ Thất bại!");
        }

        private void ShowTransactionHistory()
        {
            Console.WriteLine("\n--- LỊCH SỬ GIAO DỊCH ---");
            Console.Write("Nhập mã ví bạn muốn xem (Ví dụ: W1): ");
            string walletId = Console.ReadLine();

            // Gọi "động cơ" lấy lịch sử
            var history = _transactionService.GetTransactionHistory(walletId);

            if (history == null || history.Count == 0)
            {
                Console.WriteLine("📭 Ví này chưa có giao dịch nào, hoặc mã ví không tồn tại.");
            }
            else
            {
                Console.WriteLine($"\n--- CUỐN SỔ GIAO DỊCH CỦA VÍ: {walletId} ---");
                foreach (var trans in history)
                {
                    // Kiểm tra xem là Thu hay Chi để in ra cho đẹp
                    string typeString = trans.Type == TransactionType.Income ? "Thu 📈" : "Chi 📉";

                    // In ra đầy đủ thông tin: Ngày, Thu/Chi, Số tiền, Hạng mục, Ghi chú
                    string categoryName = trans.Category != null ? trans.Category.Name : "Chuyển tiền nội bộ";
                    Console.WriteLine($"[{trans.Date:dd/MM/yyyy}] {typeString} | {trans.Amount:N0} VNĐ | {categoryName} | Note: {trans.Note}");
                }
            }
        }

        // ===== TRANSFER =====
        private void TransferMoney()
        {
            Console.WriteLine("\n--- CHUYỂN TIỀN ---");
            Console.Write("Mã ví CHUYỂN đi (Nguồn - Ví dụ: W1): ");
            string fromId = Console.ReadLine();

            Console.Write("Mã ví NHẬN tiền (Đích - Ví dụ: W2): ");
            string toId = Console.ReadLine();

            Console.Write("Số tiền cần chuyển: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Ghi chú: ");
            string note = Console.ReadLine();

            // Gọi "động cơ" chuyển tiền
            bool success = _transferService.Transfer(fromId, toId, amount, note);

            if (success)
            {
                Console.WriteLine("✅ Chuyển tiền thành công! Hai biên lai đã được tạo tự động.");
            }
        }
    }
}