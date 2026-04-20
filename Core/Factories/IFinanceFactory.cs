/*
 * Tên file : IFinanceFactory.cs
 * Thư mục  : Core/Factories
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 15/02/2026
 * Mục đích : Giao diện chuẩn mực bắt buộc các nhà máy sản xuất ví phải tuân thủ.
 */
using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Factories
{
    public interface IFinanceFactory
    {
        // Nhận vào nguyên liệu (id, name, balance) và trả ra 1 chiếc ví (Wallet)
        Wallet CreateWallet(string id, string name, decimal balance);
    }
}
