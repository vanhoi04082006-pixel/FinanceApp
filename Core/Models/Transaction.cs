using FinanceApp.Core.Enums;
using System;

namespace FinanceApp.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public TransactionType Type { get; set; }
        public Category Category { get; set; }

        // Constructor có tham số của bạn giữ nguyên
        public Transaction(int id, decimal amount, DateTime date, string note, TransactionType type, Category category)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Note = note;
            Type = type;
            Category = category;
        }

        // THÊM: Constructor không tham số để trình xử lý JSON làm việc
        public Transaction() { }
    }
}