using System.ComponentModel.DataAnnotations;

namespace PQKTHQuanLyDSSV.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Họ tên phải từ 4 đến 100 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có đuôi gmail.com")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$", ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngành học")]
        public Branch? Branch { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giới tính")]
        public Gender? Gender { get; set; }

        public bool IsRegular { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string? Address { get; set; }

        [Range(typeof(DateTime), "1/1/1963", "12/31/2005", ErrorMessage = "Ngày sinh không hợp lệ")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm")]
        [Range(0.0, 10.0, ErrorMessage = "Điểm phải từ 0.0 đến 10.0")]
        public double? Mark { get; set; }
    }
}