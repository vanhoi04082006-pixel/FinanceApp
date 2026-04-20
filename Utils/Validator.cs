using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinanceApp.Utils
{
    public static class Validator
    {
        // 🛡️ Thanh tra 1: Kiểm tra định dạng ID Ví 
        // Luật: Phải bắt đầu bằng chữ 'W' (hoặc 'w') và theo sau là các con số (VD: W1, W20)
        public static bool IsValidWalletId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            // Dùng Regex (Biểu thức chính quy) để kiểm tra mẫu chuỗi
            // ^ : Bắt đầu chuỗi
            // [Ww] : Ký tự đầu tiên là W hoặc w
            // \d+ : Có ít nhất 1 chữ số
            // $ : Kết thúc chuỗi
            return Regex.IsMatch(id, @"^[Ww]\d+$");
        }

        // 🛡️ Thanh tra 2: Kiểm tra tên Ví hoặc Hạng mục
        // Luật: Không được chứa ký tự đặc biệt như @, #, $, %, v.v. (Chỉ cho phép chữ, số và dấu cách)
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            // \p{L} hỗ trợ nhận diện các chữ cái có dấu tiếng Việt
            return Regex.IsMatch(name, @"^[\p{L}0-9\s]+$");
        }

        // 🛡️ Thanh tra 3: Kiểm tra tính hợp lệ của Ghi chú (Note)
        // Luật: Không được quá dài (VD: Tối đa 100 ký tự)
        public static bool IsValidNoteLength(string note, int maxLength = 100)
        {
            if (string.IsNullOrEmpty(note)) return true; // Ghi chú rỗng vẫn hợp lệ
            return note.Length <= maxLength;
        }
    }
}
