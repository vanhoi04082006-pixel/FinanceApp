/*
 * Tên file : CategoryService.cs
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 08/04/2026
 * Mục đích : Quản lý danh sách các hạng mục chi tiêu (Ăn uống, Lương, v.v...)
 * Version   : 1.2 (Tích hợp Auto-Save JSON & Fix ID Generation)
 */
using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceApp.Services
{
    public class CategoryService
    {
        // Biến đóng vai trò là "kết nối nội bộ" đến nhà kho dữ liệu 
        private DatabaseContext _data;

        public CategoryService()
        {
            // Lấy thực thể duy nhất của DatabaseContext
            _data = DatabaseContext.Instance;
        }

        public bool AddCategory(string name)
        {
            // 1. Kiểm tra trùng tên (không phân biệt hoa thường)
            if (_data.Categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Thất bại vì đã tồn tại
            }

            // 2. Tạo ID gợi nhớ bằng cách bỏ dấu và thay khoảng trắng bằng gạch dưới
            // Ví dụ: "Ăn uống" -> "AN_UONG"
            string newId = RemoveSign(name).ToUpper().Replace(" ", "_");

            // 3. Tạo đối tượng và thêm vào danh sách
            Category newCat = new Category { Id = newId, Name = name };
            _data.Categories.Add(newCat);

            // BƯỚC 4: LƯU THAY ĐỔI VÀO FILE JSON 💾
            _data.SaveChanges();

            return true; // Thành công!
        }

        public bool DeleteCategory(string idToDelete)
        {
            // Tìm hạng mục theo ID
            var categoryToDelete = _data.Categories.FirstOrDefault(c => c.Id == idToDelete);

            if (categoryToDelete != null)
            {
                // Xóa khỏi danh sách
                _data.Categories.Remove(categoryToDelete);

                // LƯU THAY ĐỔI: Để danh mục này biến mất vĩnh viễn trong file JSON
                _data.SaveChanges();
                return true;
            }

            return false; // Không tìm thấy để xóa
        }

        public bool UpdateCategory(string id, string newName)
        {
            // 1. Tìm hạng mục cần sửa
            var category = _data.Categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                // 2. Cập nhật tên mới
                category.Name = newName;

                // 3. Cập nhật lại ID dựa trên tên mới (Để dữ liệu đồng bộ)
                category.Id = RemoveSign(newName).ToUpper().Replace(" ", "_");

                // LƯU THAY ĐỔI 💾
                _data.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Category> GetAllCategories()
        {
            return _data.Categories;
        }

        // --- Hàm hỗ trợ: Bỏ dấu tiếng Việt để tạo mã ID sạch ---
        public string RemoveSign(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            string[] signs = new string[] {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ", "ÍÌỊỈĨ",
                "đ", "Đ",
                "ýỳỵỷỹ", "ÝỲỴỶỸ"
            };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
            }
            return str;
        }
    }
}