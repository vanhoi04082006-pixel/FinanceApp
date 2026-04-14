using FinanceApp.Core.Enums;
using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Monitoring
{
    public class GoalTracker : IObserver
    {
        private Wallet _wallet;
        private string _targetName;
        private decimal _targetAmount;

        public GoalTracker(Wallet wallet, string targetName, decimal targetAmount)
        {
            _wallet = wallet;
            _targetName = targetName;
            _targetAmount = targetAmount;
        }

        public void Update(Transaction trans)
        {
            // 1. Thám tử này chỉ quan tâm khi có TIỀN VÀO (Income)
            if (trans.Type == TransactionType.Income)
            {
                // 2. Tính toán phần trăm tiến độ
                // Lưu ý: Ép kiểu hoặc đảm bảo phép chia không bị mất số thập phân
                decimal progress = (_wallet.Balance / _targetAmount) * 100;

                Console.WriteLine($"\n[MỤC TIÊU] 🎯 {_targetName}");
                Console.WriteLine($"Tiến độ: {progress:F2}% ({_wallet.Balance:N0} / {_targetAmount:N0} VNĐ)");

                // 3. Kiểm tra xem đã về đích chưa
                if (_wallet.Balance >= _targetAmount)
                {
                    Console.WriteLine($"🥳 Tuyệt vời! Bạn đã tích lũy đủ tiền để thực hiện mục tiêu '{_targetName}'!");
                }
            }
        }
    }
}
