/*
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 15/04/2026
 * Mục đích : 
 * Tên file : CashFactory.cs
 * Thư mục  : Core/Factories
 */
using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Factories
{
    public class CashFactory : IFinanceFactory
    {
        public Wallet CreateWallet(string id, string name, decimal balance)
        {
            // Chuyên sản xuất CashWallet
            return new CashWallet { Id = id, Name = name, Balance = balance };
        }
    }
}
