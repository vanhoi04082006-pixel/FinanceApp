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
using System.Text.Json.Serialization; // Thêm thư viện này

namespace FinanceApp.Core.Models
{
    // Gắn nhãn để JSON biết cách phân biệt các loại ví con
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(CashWallet), typeDiscriminator: "cash")]
    [JsonDerivedType(typeof(CardWallet), typeDiscriminator: "card")]
    public abstract class Wallet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }

        public abstract string GetWalletType();

        public Wallet()
        {
            Transactions = new List<Transaction>();
        }
    }
}
