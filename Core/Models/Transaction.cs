/*
 * Tên file : Transaction.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Đây là nơi chứa các thông tin trên tờ hóa đơn của bạn 
              ("Biên lai" lưu trữ thông tin chi tiết của một lần tiêu tiền.)
 * Version : 1.0
 */
using FinanceApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }           // Mã giao dịch
        public decimal Amount { get; set; }   // Số tiền giao dịch
        public DateTime Date { get; set; }     // Ngày thực hiện
        public string Note { get; set; }      // Ghi chú

        // Đây là nơi ta dùng hai "nguyên liệu" đã tạo:
        public TransactionType Type { get; set; }
        public Category Category { get; set; }
        public Transaction(int id, decimal amount, DateTime date, string note, TransactionType type, Category category)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Note = note;
            Type = type;
            Category = category;
        }
    }
}
