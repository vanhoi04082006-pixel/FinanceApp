/*
 * Tên file : InputHelper.cs
 * Thư mục  : Utils
 * Người tạo : Bùi Văn Hội
 * Ngày tạo : 15/04/2026
 * Mục đích : "Tấm khiên" bảo vệ ứng dụng khỏi lỗi nhập liệu ngớ ngẩn (Crash).
 * Version : 1.0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.Utils
{
    public static class InputHelper
    {
        // 🛡️ Kỹ năng 1: Ép người dùng phải nhập chữ, cấm để trống
        public static string GetString(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("❌ Lỗi: Không được để trống. Vui lòng nhập lại!");
                }
            } while (string.IsNullOrEmpty(input));

            return input;
        }

        // 🛡️ Kỹ năng 2: Ép người dùng nhập số tiền hợp lệ (Cấm nhập chữ cái)
        // minValue mặc định là 0 để cấm nhập số âm
        public static decimal GetDecimal(string prompt, decimal minValue = 0)
        {
            decimal result;
            bool isValid;
            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                // TryParse: Thử biến chữ thành số. 
                // Nếu thành công trả về true và đẩy số vào biến result. Nếu thất bại trả về false.
                isValid = decimal.TryParse(input, out result);

                if (!isValid)
                {
                    Console.WriteLine("❌ Lỗi: Vui lòng chỉ nhập số (không chứa chữ cái)!");
                }
                else if (result < minValue)
                {
                    Console.WriteLine($"❌ Lỗi: Số tiền không được nhỏ hơn {minValue:N0}!");
                    isValid = false; // Đánh dấu lỗi để bắt vòng lặp chạy lại
                }
            } while (!isValid);

            return result;
        }

        // 🛡️ Kỹ năng 3: Ép nhập số nguyên để chọn Menu (Cấm chọn ngoài danh sách)
        public static int GetInt(string prompt, int min, int max)
        {
            int result;
            bool isValid;
            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim();

                isValid = int.TryParse(input, out result);

                if (!isValid)
                {
                    Console.WriteLine("❌ Lỗi: Vui lòng nhập một số nguyên!");
                }
                else if (result < min || result > max)
                {
                    Console.WriteLine($"❌ Lỗi: Vui lòng chọn từ {min} đến {max}!");
                    isValid = false;
                }
            } while (!isValid);

            return result;
        }
    }
}
