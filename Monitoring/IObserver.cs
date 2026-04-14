/*
 * Tên file : IObserver.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 20/04/2026
 * Mục đích : Quy định cách các bộ phận giám sát nhận dữ liệu từ giao dịch.
 * Version : 1.0
 */
using FinanceApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Monitoring
{
    public interface IObserver
    {
        // Hàm này sẽ được gọi tự động mỗi khi có giao dịch mới thành công
        void Update(Transaction transaction);
    }
}
