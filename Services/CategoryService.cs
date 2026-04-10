using FinanceApp.Core.Models;
using FinanceApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinanceApp.Services
{
    public class CategoryService
    {
        // Biến này đóng vai trò là "kết nối nội bộ" đến nhà kho dữ liệu 
        private DatabaseContext _data;

        public CategoryService()
        {
            // Lấy chìa khóa duy nhất và cất vào biến _data
            _data = DatabaseContext.Instance;
        }
        public bool AddCategory(string name)
        {
            // 1. Kiểm tra trùng tên (không phân biệt hoa thường)
            if (_data.Categories.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return false; // Thất bại vì đã tồn tại
            }

            // 2. Tạo ID gợi nhớ (Ví dụ: "Ăn uống" -> "AN_UONG")
            // Gợi ý: Dùng .ToUpper() và .Replace(" ", "_")
            string newId = name.ToUpper().Replace(" " , "");

            // 3. Tạo đối tượng và thêm vào "nhà kho"
            Category newCat = new Category { Id = newId, Name = name };
            _data.Categories.Add(newCat);

            return true; // Thành công!
        }

        // Hàm hỗ trợ bỏ dấu tiếng Việt
        public string RemoveSign(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            string[] signs = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };
            for (int i = 1; i < signs.Length; i++)
            {
                for (int j = 0; j < signs[i].Length; j++)
                    str = str.Replace(signs[i][j], signs[0][i - 1]);
            }
            return str;
        }

        public bool DeleteCategory(string idToDelete)
        {
            // Tìm hạng mục
            var categoryToDelete = _data.Categories.FirstOrDefault(c => c.Id == idToDelete);

            if (categoryToDelete != null)
            {
                // Nếu tìm thấy, chúng ta dùng lệnh Remove để xóa khỏi danh sách
                _data.Categories.Remove(categoryToDelete);
                return true; // Báo về là đã xóa thành công
            }
            else
            {
                // Nếu không tìm thấy thì làm gì ở đây nhỉ?
                return false;
            }
        }

        public bool UpdateCategory(string id, string newName)
        {
            // 1. Tìm hạng mục cần sửa
            var category = _data.Categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                // 2. Cập nhật tên mới
                category.Name = newName;

                // 3. Cập nhật ID mới tương ứng với tên mới
                category.Id = RemoveSign(newName).ToUpper().Replace(" ", "_");

                return true; // Thành công
            }
            return false; // Thất bại vì không tìm thấy ID
        }

        public List<Category> GetAllCategories()
        {
            return _data.Categories;
        }
    }
}