/*
 * Tên file : CashWallet.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Kế thừa các thông tin cần có cho 1 chiếc ví từ lớp cha (Wallet) và định nghĩa
 * là loại ví tiền mặt (Ví tiền mặt cụ thể).
 * Version : 1.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Models
{
    public class CashWallet : Wallet
    {
        public override string GetWalletType()
        {
            return "Ví tiền mặt";
        }
    }
}
