using FinanceApp.Data;
using FinanceApp.Services;
using FinanceApp.UI;
using System;
using System.Text;

namespace FinanceApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Thiết lập tiếng Việt cho Console
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Khởi tạo các thành phần cốt lõi (Services & Database)
            WalletService walletService = new WalletService();
            TransactionService transactionService = new TransactionService();
            CategoryService categoryService = new CategoryService();
            TransferService transferService = new TransferService();
            StatisticsService statisticsService = new StatisticsService();
            var data = DatabaseContext.Instance;

            // 2. Khởi tạo Trình quản lý Menu và truyền các service vào
            MenuManager menu = new MenuManager(walletService, transactionService, categoryService, transferService, statisticsService, data);

            // 3. Bắt đầu chạy giao diện
            Console.WriteLine("🚀 Ứng dụng Quản lý Tài chính đang khởi động...");
            menu.Start();

        }
    }
}