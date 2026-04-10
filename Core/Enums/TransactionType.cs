/*
 * Tên file : TransactionType.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Đây là một danh sách các lựa chọn có sẵn để máy tính hiểu được ở có 2 lựa
              chọn là thu nhập và chi tiêu.
              (Định nghĩa kiểu Thu nhập hay Chi tiêu.)
 * Version : 1.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Enums
{
    public enum TransactionType
    {
        Income, // Thu nhập 
        Expense // Chi tiêu 
    }
}
