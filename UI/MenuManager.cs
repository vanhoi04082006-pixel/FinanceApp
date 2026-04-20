/*
 * Tên file : MenuManager.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 12/04/2026
 * Mục đích : Quản lý giao diện Console với cấu trúc Sub-menus & Clear UX.
 * Version   : 2.0 (Phiên bản hoàn chỉnh cuối cùng 🏆)
 */
using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using FinanceApp.Data;
using FinanceApp.Services;
using FinanceApp.Monitoring;
using FinanceApp.Utils;
using System;

namespace FinanceApp.UI
{
    public class MenuManager
    {
        private WalletService _walletService;
        private TransactionService _transactionService;
        private CategoryService _categoryService;
        private TransferService _transferService;
        private StatisticsService _statisticsService;
        private DatabaseContext _data;

        public MenuManager(WalletService walletService,
                           TransactionService transactionService,
                           CategoryService categoryService,
                           TransferService transferService,
                           StatisticsService statisticsService,
                           DatabaseContext data)
        {
            _walletService = walletService;
            _transactionService = transactionService;
            _categoryService = categoryService;
            _transferService = transferService;
            _statisticsService = statisticsService;
            _data = data;
        }

        // ==========================================
        // 1. MENU CHÍNH (MAIN MENU)
        // ==========================================
        public void Start()
        {
            bool running = true;

            while (running)
            {
                Console.Clear(); // 🧹 Xóa sạch màn hình trước khi in Menu chính
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                 FINANCE APP MANAGEMENT                       ║");
                Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║  1. Quản lý Ví (Wallet)                                      ║");
                Console.WriteLine("║  2. Quản lý Hạng mục (Category)                              ║");
                Console.WriteLine("║  3. Quản lý Giao dịch (Transaction)                          ║");
                Console.WriteLine("║  4. Theo dõi và Cảnh báo (Monitoring)                        ║");
                Console.WriteLine("║  5. Báo cáo Thống kê (Statistics)                            ║");
                Console.WriteLine("║  0. Thoát chương trình                                       ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");

                string choice = InputHelper.GetString("👉 Chọn danh mục chính: ");

                switch (choice)
                {
                    case "1": WalletMenu(); break;
                    case "2": CategoryMenu(); break;
                    case "3": TransactionMenu(); break;
                    case "4": MonitoringMenu(); break;

                    case "5":
                        Console.Clear();
                        ShowStatistics();
                        WaitUser();
                        break;

                    case "0":
                        running = false;
                        Console.Clear();
                        Console.WriteLine("👋 Thoát chương trình. Hẹn gặp lại!");
                        break;

                    default:
                        Console.WriteLine("❌ Lựa chọn không hợp lệ!");
                        WaitUser();
                        break;
                }
            }
        }

        // ==========================================
        // 2. CÁC MENU CON (SUB-MENUS)
        // ==========================================

        private void WalletMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n--- 👛 QUẢN LÝ VÍ ---");
                Console.WriteLine("1. Tạo ví mới");
                Console.WriteLine("2. Xem danh sách ví");
                Console.WriteLine("3. Sửa tên ví");
                Console.WriteLine("4. Xóa ví");
                Console.WriteLine("0. Quay lại Menu chính");

                string choice = InputHelper.GetString("👉 Chọn chức năng: ");

                if (choice == "0")
                {
                    back = true;
                    continue;
                }

                Console.WriteLine("-----------------------------------");
                switch (choice)
                {
                    case "1": CreateWallet(); WaitUser(); break;
                    case "2": ShowWallets(); WaitUser(); break;
                    case "3": UpdateWallet(); WaitUser(); break;
                    case "4": DeleteWallet(); WaitUser(); break;
                    default: Console.WriteLine("❌ Không hợp lệ!"); WaitUser(); break;
                }
            }
        }

        private void CategoryMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n--- 📂 QUẢN LÝ HẠNG MỤC ---");
                Console.WriteLine("1. Thêm hạng mục");
                Console.WriteLine("2. Xem danh sách hạng mục");
                Console.WriteLine("3. Sửa hạng mục");
                Console.WriteLine("4. Xóa hạng mục");
                Console.WriteLine("0. Quay lại Menu chính");

                string choice = InputHelper.GetString("👉 Chọn chức năng: ");

                if (choice == "0") { back = true; continue; }

                Console.WriteLine("-----------------------------------");
                switch (choice)
                {
                    case "1": AddCategory(); WaitUser(); break;
                    case "2": ShowCategories(); WaitUser(); break;
                    case "3": UpdateCategory(); WaitUser(); break;
                    case "4": DeleteCategory(); WaitUser(); break;
                    default: Console.WriteLine("❌ Không hợp lệ!"); WaitUser(); break;
                }
            }
        }

        private void TransactionMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n--- 💸 QUẢN LÝ GIAO DỊCH ---");
                Console.WriteLine("1. Thêm giao dịch (Thu/Chi)");
                Console.WriteLine("2. Xem lịch sử giao dịch");
                Console.WriteLine("3. Chuyển tiền giữa các ví");
                Console.WriteLine("0. Quay lại Menu chính");

                string choice = InputHelper.GetString("👉 Chọn chức năng: ");

                if (choice == "0") { back = true; continue; }

                Console.WriteLine("-----------------------------------");
                switch (choice)
                {
                    case "1": AddTransaction(); WaitUser(); break;
                    case "2": ShowTransactionHistory(); WaitUser(); break;
                    case "3": TransferMoney(); WaitUser(); break;
                    default: Console.WriteLine("❌ Không hợp lệ!"); WaitUser(); break;
                }
            }
        }

        private void MonitoringMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\n--- 🚨 THEO DÕI & CẢNH BÁO ---");
                Console.WriteLine("1. Thiết lập cảnh báo ngân sách");
                Console.WriteLine("2. Thiết lập mục tiêu tiết kiệm");
                Console.WriteLine("0. Quay lại Menu chính");

                string choice = InputHelper.GetString("👉 Chọn chức năng: ");

                if (choice == "0") { back = true; continue; }

                Console.WriteLine("-----------------------------------");
                switch (choice)
                {
                    case "1": SetupBudgetAlert(); WaitUser(); break;
                    case "2": SetupGoalTracker(); WaitUser(); break;
                    default: Console.WriteLine("❌ Không hợp lệ!"); WaitUser(); break;
                }
            }
        }

        // ==========================================
        // 3. CÁC HÀM XỬ LÝ LOGIC (Đã tích hợp Utils)
        // ==========================================

        // --- WALLET ---
        private void CreateWallet()
        {
            string name;
            do
            {
                name = InputHelper.GetString("Tên ví: ");
                if (!Validator.IsValidName(name)) Console.WriteLine("❌ Tên không được chứa ký tự đặc biệt!");
            } while (!Validator.IsValidName(name));

            string type;
            do
            {
                type = InputHelper.GetString("Loại (cash/card): ").ToLower();
            } while (type != "cash" && type != "card");

            decimal balance = InputHelper.GetDecimal("Số dư: ", 0);

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
            string id = GetValidWalletId("ID ví cần sửa: ");
            string name;
            do
            {
                name = InputHelper.GetString("Tên mới: ");
                if (!Validator.IsValidName(name)) Console.WriteLine("❌ Tên không được chứa ký tự đặc biệt!");
            } while (!Validator.IsValidName(name));

            Console.WriteLine(_walletService.UpdateWallet(id, name) ? "✨ Cập nhật thành công!" : "❌ Không tìm thấy!");
        }

        private void DeleteWallet()
        {
            string id = GetValidWalletId("ID ví cần xóa: ");
            var result = _walletService.DeleteWallet(id);
            Console.WriteLine(result.success ? $"✅ {result.message}" : $"❌ {result.message}");
        }

        // --- CATEGORY ---
        private void AddCategory()
        {
            string name;
            do
            {
                name = InputHelper.GetString("Tên hạng mục: ");
                if (!Validator.IsValidName(name)) Console.WriteLine("❌ Tên không được chứa ký tự đặc biệt!");
            } while (!Validator.IsValidName(name));

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
            string id = InputHelper.GetString("ID Hạng mục: ");
            string name;
            do
            {
                name = InputHelper.GetString("Tên mới: ");
                if (!Validator.IsValidName(name)) Console.WriteLine("❌ Tên không được chứa ký tự đặc biệt!");
            } while (!Validator.IsValidName(name));

            Console.WriteLine(_categoryService.UpdateCategory(id, name) ? "✨ Thành công!" : "❌ Không tìm thấy!");
        }

        private void DeleteCategory()
        {
            string id = InputHelper.GetString("ID Hạng mục cần xóa: ");
            Console.WriteLine(_categoryService.DeleteCategory(id) ? "✅ Đã xóa!" : "❌ Không tìm thấy!");
        }

        // --- TRANSACTION ---
        private void AddTransaction()
        {
            if (_data.Categories.Count == 0)
            {
                Console.WriteLine("❌ Cần ít nhất 1 hạng mục để tạo giao dịch. Hãy thêm hạng mục trước!");
                return;
            }

            string wId = GetValidWalletId("Wallet ID (VD: W1): ");

            Console.WriteLine("Chọn hạng mục:");
            for (int i = 0; i < _data.Categories.Count; i++)
                Console.WriteLine($"{i + 1}. {_data.Categories[i].Name}");

            int choice = InputHelper.GetInt("Nhập số thứ tự: ", 1, _data.Categories.Count);
            string cId = _data.Categories[choice - 1].Id;

            decimal amount = InputHelper.GetDecimal("Số tiền: ", 1);
            string note = GetValidNote("Ghi chú: ");

            int typeChoice = InputHelper.GetInt("Loại (1: Thu, 2: Chi): ", 1, 2);
            var type = typeChoice == 1 ? TransactionType.Income : TransactionType.Expense;

            Console.WriteLine(_transactionService.AddTransaction(wId, cId, amount, type, note) ? "🎉 Thành công!" : "❌ Thất bại!");
        }

        private void ShowTransactionHistory()
        {
            string walletId = GetValidWalletId("Nhập mã ví bạn muốn xem (Ví dụ: W1): ");
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

        private void TransferMoney()
        {
            string fromId = GetValidWalletId("Mã ví CHUYỂN đi (VD: W1): ");
            string toId = GetValidWalletId("Mã ví NHẬN tiền (VD: W2): ");

            if (fromId.Equals(toId, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("❌ Lỗi: Không thể chuyển tiền cho chính ví đó!");
                return;
            }

            decimal amount = InputHelper.GetDecimal("Số tiền cần chuyển: ", 1);
            string note = GetValidNote("Ghi chú: ");

            if (_transferService.Transfer(fromId, toId, amount, note))
            {
                Console.WriteLine("✅ Chuyển tiền thành công! Hai biên lai đã được tạo tự động.");
            }
        }

        // --- MONITORING ---
        private void SetupBudgetAlert()
        {
            if (_data.Categories.Count == 0)
            {
                Console.WriteLine("❌ Cần có hạng mục trước khi đặt ngân sách!");
                return;
            }

            ShowWallets();
            string wId = GetValidWalletId("Nhập ID ví muốn theo dõi (VD: W1): ");
            var wallet = _data.Wallets.Find(w => w.Id.Equals(wId, StringComparison.OrdinalIgnoreCase));

            if (wallet == null)
            {
                Console.WriteLine("❌ Không tìm thấy ví!"); return;
            }

            Console.WriteLine("\nChọn hạng mục muốn đặt hạn mức:");
            for (int i = 0; i < _data.Categories.Count; i++)
                Console.WriteLine($"{i + 1}. {_data.Categories[i].Name}");

            int catChoice = InputHelper.GetInt("Nhập số thứ tự: ", 1, _data.Categories.Count);
            string categoryName = _data.Categories[catChoice - 1].Name;

            decimal limit = InputHelper.GetDecimal($"Nhập số tiền tối đa muốn chi cho '{categoryName}': ", 1);

            BudgetAlert alert = new BudgetAlert(wallet, categoryName, limit);
            _transactionService.Attach(alert);

            Console.WriteLine($"✅ Đã kích hoạt cảnh báo: Nếu ví '{wallet.Name}' chi quá {limit:N0} VNĐ cho '{categoryName}', hệ thống sẽ báo động!");
        }

        private void SetupGoalTracker()
        {
            ShowWallets();
            string wId = GetValidWalletId("👉 Nhập ID ví dùng để tiết kiệm (VD: W1): ");
            var wallet = _data.Wallets.Find(w => w.Id.Equals(wId, StringComparison.OrdinalIgnoreCase));

            if (wallet == null)
            {
                Console.WriteLine("❌ Không tìm thấy ví!"); return;
            }

            string goalName;
            do
            {
                goalName = InputHelper.GetString("👉 Nhập tên mục tiêu (VD: Mua Laptop...): ");
                if (!Validator.IsValidName(goalName)) Console.WriteLine("❌ Tên không được chứa ký tự đặc biệt!");
            } while (!Validator.IsValidName(goalName));

            decimal targetAmount = InputHelper.GetDecimal($"👉 Nhập số tiền cần đạt được cho '{goalName}': ", 1000);

            GoalTracker tracker = new GoalTracker(wallet, goalName, targetAmount);
            _transactionService.Attach(tracker);

            Console.WriteLine($"✅ Đã bắt đầu theo dõi mục tiêu '{goalName}'! Hãy chăm chỉ nạp tiền vào ví nhé.");
        }

        // --- STATISTICS ---
        private void ShowStatistics()
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("               📊 BÁO CÁO THỐNG KÊ TÀI CHÍNH             ");
            Console.WriteLine("=======================================================");

            decimal totalIncome = _statisticsService.GetTotalAmount(TransactionType.Income);
            decimal totalExpense = _statisticsService.GetTotalAmount(TransactionType.Expense);

            Console.WriteLine("\n[1] TỔNG QUAN (Từ trước đến nay):");
            Console.WriteLine($"    + Tổng thu nhập đã nhận: {totalIncome:N0} VND");
            Console.WriteLine($"    - Tổng chi tiêu đã xuất: {totalExpense:N0} VND");

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

            Console.WriteLine($"\n[4] 🚨 HẠNG MỤC TỐN KÉM NHẤT:");
            Console.WriteLine($"    => {_statisticsService.GetTopSpendingCategory()}");

            Console.WriteLine("=======================================================\n");
        }

        // ==========================================
        // 4. CÁC HÀM HỖ TRỢ DÙNG CHUNG
        // ==========================================

        private string GetValidWalletId(string prompt)
        {
            string id;
            do
            {
                id = InputHelper.GetString(prompt).ToUpper();
                if (!Validator.IsValidWalletId(id))
                    Console.WriteLine("❌ Lỗi: Mã ví phải bắt đầu bằng 'W' theo sau là số (VD: W1, W2)!");
            } while (!Validator.IsValidWalletId(id));
            return id;
        }

        private string GetValidNote(string prompt)
        {
            string note;
            do
            {
                Console.Write(prompt);
                note = Console.ReadLine()?.Trim();
                if (!Validator.IsValidNoteLength(note))
                    Console.WriteLine("❌ Lỗi: Ghi chú quá dài (tối đa 100 ký tự)!");
            } while (!Validator.IsValidNoteLength(note));
            return note;
        }

        private void WaitUser()
        {
            Console.WriteLine("\n👉 Bấm phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}