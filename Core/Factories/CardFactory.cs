/*
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 15/04/2026
 * Mục đích :
 * Tên file : CardFactory.cs
 * Thư mục  : Core/Factories
 * Version : 1.0
 */
using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Factories
{
    public class CardFactory : IFinanceFactory
    {
        public Wallet CreateWallet(string id, string name, decimal balance)
        {
            // Chuyên sản xuất CardWallet
            return new CardWallet { Id = id, Name = name, Balance = balance };
        }
    }
}
