/*
 * Tên file : MenuManager.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 12/04/2026
 * Mục đích : Quản lý giao diện Console và điều hướng người dùng.
 * Version   : 1.3 (Tích hợp chức năng Thống kê Số 14)
 */
using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using FinanceApp.Services;
using FinanceApp.Monitoring;
using System;

namespace FinanceApp.UI
{
    public class MenuManager
    {
        // Khai báo các biến để chứa các Service
        private WalletService _walletService;
        private TransactionService _transactionService;
        private CategoryService _categoryService;
        private TransferService _transferService;
        private StatisticsService _statisticsService; // THÊM MỚI: Trợ lý Thống kê
        private DatabaseContext _data;

        // Cập nhật Constructor để nhận thêm StatisticsService
        public MenuManager(WalletService walletService,
                           TransactionService transactionService,
                           CategoryService categoryService,
                           TransferService transferService,
                           StatisticsService statisticsService, // THÊM MỚI
                           DatabaseContext data)
        {
            _walletService = walletService;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _transferService = transferService;
            _statisticsService = statisticsService; // Gán giá trị
            _data = data;
        }

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
                    case "12": SetupBudgetAlert(); break;
                    case "13": SetupGoalTracker(); break;

                    // KÍCH HOẠT CHỨC NĂNG 14
                    case "14": ShowStatistics(); break;

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
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                 FINANCE APP MANAGEMENT                       ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ WALLET (Quản lý ví)                                          ║");
            Console.WriteLine("║  1. Tạo ví        2. Xem danh sách ví      3. Sửa ví         ║");
            Console.WriteLine("║  4. Xóa ví                                                   ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ CATEGORY (Hạng mục)                                          ║");
            Console.WriteLine("║  5. Thêm hạng mục   6. Xem hạng mục    7. Sửa hạng mục       ║");
            Console.WriteLine("║  8. Xóa hạng mục                                             ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ TRANSACTION (Giao dịch)                                      ║");
            Console.WriteLine("║  9. Thêm giao dịch  10. Lịch sử giao dịch                    ║");
            Console.WriteLine("║  11. Chuyển tiền giữa ví                                     ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ MONITORING (Theo dõi & cảnh báo)                             ║");
            Console.WriteLine("║  12. Cảnh báo ngân sách                                      ║");
            Console.WriteLine("║  13. Mục tiêu tiết kiệm                                      ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ STATISTICS                                                   ║");
            Console.WriteLine("║  14. Thống kê thu chi                                        ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ SYSTEM                                                       ║");
            Console.WriteLine("║  0. Thoát                                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        }

        // ===== WALLET =====
        private void CreateWallet()
        {
            Console.Write("Tên ví: ");
            string name = Console.ReadLine();

            Console.Write("Loại (cash/card): ");
            string type = Console.ReadLine();

            Console.Write("Số dư: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal balance))
            {
                _walletService.CreateWallet(type, name, balance);
                Console.WriteLine("✅ Tạo ví thành công!");
            }
            else Console.WriteLine("❌ Vui lòng nhập số hợp lệ.");
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
            Console.WriteLine(_walletService.UpdateWallet(id, name) ? "✨ Cập nhật thành công!" : "❌ Không tìm thấy!");
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
            Console.WriteLine(_categoryService.AddCategory(name) ? "✅ Thành công!" : "❌ Đã tồn tại!");
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
            Console.WriteLine(_categoryService.UpdateCategory(id, name) ? "✨ Thành công!" : "❌ Không tìm thấy!");
        }

        private void DeleteCategory()
        {
            Console.Write("ID: ");
            string id = Console.ReadLine();
            Console.WriteLine(_categoryService.DeleteCategory(id) ? "✅ Đã xóa!" : "❌ Không tìm thấy!");
        }

        // ===== TRANSACTION =====
        private void AddTransaction()
        {
            Console.Write("Wallet ID: ");
            string wId = Console.ReadLine();

            Console.WriteLine("Chọn hạng mục:");
            for (int i = 0; i < _data.Categories.Count; i++)
                Console.WriteLine($"{i + 1}. {_data.Categories[i].Name}");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > _data.Categories.Count)
            {
                Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                return;
            }
            string cId = _data.Categories[choice - 1].Id;

            Console.Write("Số tiền: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Ghi chú: ");
            string note = Console.ReadLine();

            Console.Write("Loại (1: Thu, 2: Chi): ");
            string typeInput = Console.ReadLine();
            var type = typeInput == "1" ? TransactionType.Income : TransactionType.Expense;

            Console.WriteLine(_transactionService.AddTransaction(wId, cId, amount, type, note) ? "🎉 Thành công!" : "❌ Thất bại!");
        }

        private void ShowTransactionHistory()
        {
            Console.WriteLine("\n--- LỊCH SỬ GIAO DỊCH ---");
            Console.Write("Nhập mã ví bạn muốn xem (Ví dụ: W1): ");
            string walletId = Console.ReadLine();

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
                    string typeString = trans.Type == TransactionType.Income ? "Thu 📈" : "Chi 📉";
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

            if (_transferService.Transfer(fromId, toId, amount, note))
            {
                Console.WriteLine("✅ Chuyển tiền thành công! Hai biên lai đã được tạo tự động.");
            }
        }

        // ===== MONITORING =====
        private void SetupBudgetAlert()
        {
            Console.WriteLine("\n--- 🚨 THIẾT LẬP CẢNH BÁO CHI TIÊU ---");
            ShowWallets();
            Console.Write("Nhập ID ví muốn theo dõi (VD: W1): ");
            string wId = Console.ReadLine();
            var wallet = _data.Wallets.Find(w => w.Id == wId);

            if (wallet == null)
            {
                Console.WriteLine("❌ Không tìm thấy ví!"); return;
            }

            Console.WriteLine("\nChọn hạng mục muốn đặt hạn mức:");
            for (int i = 0; i < _data.Categories.Count; i++)
                Console.WriteLine($"{i + 1}. {_data.Categories[i].Name}");

            Console.Write("Nhập số thứ tự: ");
            int catChoice = int.Parse(Console.ReadLine());
            string categoryName = _data.Categories[catChoice - 1].Name;

            Console.Write($"Nhập số tiền tối đa muốn chi cho '{categoryName}': ");
            decimal limit = decimal.Parse(Console.ReadLine());

            BudgetAlert alert = new BudgetAlert(wallet, categoryName, limit);
            _transactionService.Attach(alert);

            Console.WriteLine($"✅ Đã kích hoạt cảnh báo: Nếu ví '{wallet.Name}' chi quá {limit:N0} VNĐ cho '{categoryName}', hệ thống sẽ báo động!");
        }

        private void SetupGoalTracker()
        {
            Console.WriteLine("\n--- 🎯 THIẾT LẬP MỤC TIÊU TIẾT KIỆM ---");
            ShowWallets();
            Console.Write("👉 Nhập ID ví dùng để tiết kiệm (VD: W1): ");
            string wId = Console.ReadLine();
            var wallet = _data.Wallets.Find(w => w.Id == wId);

            if (wallet == null)
            {
                Console.WriteLine("❌ Không tìm thấy ví!"); return;
            }

            Console.Write("👉 Nhập tên mục tiêu (VD: Mua Laptop, Đi du lịch...): ");
            string goalName = Console.ReadLine();

            Console.Write($"👉 Nhập số tiền cần đạt được cho '{goalName}': ");
            decimal targetAmount = decimal.Parse(Console.ReadLine());

            GoalTracker tracker = new GoalTracker(wallet, goalName, targetAmount);
            _transactionService.Attach(tracker);

            Console.WriteLine($"✅ Đã bắt đầu theo dõi mục tiêu '{goalName}'! Hãy chăm chỉ nạp tiền vào ví nhé.");
        }

        // ===== STATISTICS (CHỨC NĂNG MỚI) =====
        private void ShowStatistics()
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("               📊 BÁO CÁO THỐNG KÊ TÀI CHÍNH             ");
            Console.WriteLine("=======================================================");

            // 1. Tổng quan Toàn thời gian
            decimal totalIncome = _statisticsService.GetTotalAmount(TransactionType.Income);
            decimal totalExpense = _statisticsService.GetTotalAmount(TransactionType.Expense);

            Console.WriteLine("\n[1] TỔNG QUAN (Từ trước đến nay):");
            Console.WriteLine($"    + Tổng thu nhập đã nhận: {totalIncome:N0} VND");
            Console.WriteLine($"    - Tổng chi tiêu đã xuất: {totalExpense:N0} VND");

            // 2. Thống kê theo Tháng hiện tại
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            decimal monthIncome = _statisticsService.GetTotalByTime(TransactionType.Income, currentMonth, currentYear);
            decimal monthExpense = _statisticsService.GetTotalByTime(TransactionType.Expense, currentMonth, currentYear);

            Console.WriteLine($"\n[2] TRONG THÁNG NÀY ({currentMonth}/{currentYear}):");
            Console.WriteLine($"    + Tiền vào: {monthIncome:N0} VND");
            Console.WriteLine($"    - Tiền ra : {monthExpense:N0} VND");
            if (monthIncome >= monthExpense)
                Console.WriteLine($"    => Bạn đang thặng dư: {(monthIncome - monthExpense):N0} VND. Tuyệt vời! 🌟");
            else
                Console.WriteLine($"    => Bạn đang thâm hụt: {(monthExpense - monthIncome):N0} VND. Hãy cẩn thận! ⚠️");

            // 3. Phân tích hạng mục chi tiêu
            Console.WriteLine("\n[3] PHÂN TÍCH CHI TIÊU THEO HẠNG MỤC:");
            var expenseStats = _statisticsService.GetStatisticsByCategory(TransactionType.Expense);

            if (expenseStats.Count == 0)
            {
                Console.WriteLine("    📭 Chưa có giao dịch chi tiêu nào để thống kê.");
            }
            else
            {
                foreach (var item in expenseStats)
                {
                    Console.WriteLine($"    - {item.Key,-20} : {item.Value:N0} VND");
                }
            }

            // 4. Tìm ra "Thủ phạm" gây tốn kém nhất
            Console.WriteLine($"\n[4] 🚨 HẠNG MỤC TỐN KÉM NHẤT:");
            Console.WriteLine($"    => {_statisticsService.GetTopSpendingCategory()}");

            Console.WriteLine("=======================================================\n");
        }
    }
}