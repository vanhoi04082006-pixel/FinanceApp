/*
 * Tên file : Wallet.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Đây là nơi quản lý thông tin mà 1 chiếc ví phải có
              (Lớp cha quy định các đặc điểm chung của mọi loại ví.)
 * Version : 1.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Models
{
    public abstract class Wallet
    {
        public string Id { get; set; }      // Mã ví 
        public string Name { get; set; }    // Tên ví 
        public decimal Balance { get; set; } // Số dư 
        public List<Transaction> Transactions { get; set; }

        // Quy tắc bắt buộc: Mọi loại ví con đều phải tự khai báo danh tính
        public abstract string GetWalletType();
        public Wallet()
        {
            Transactions = new List<Transaction>();
        }
    }
}
