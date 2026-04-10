/*
 * Tên file : Category.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Đây là "bản thiết kế" cho các hạng mục như Ăn uống, Học tập...
              (Đại diện cho các hạng mục (Ăn uống, Học tập...).)
 * Version : 1.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Core.Models
{
    public class Category
    {
        public string Id { get; set; } // Mã số định danh
        public string Name { get; set; } // Tên hạng mục hay nói cách khác là loại ấy

        public Category (string id , string name)
        {
            Id = id;
            Name = name;
        }

        public Category ()
        {

        }
    }
}