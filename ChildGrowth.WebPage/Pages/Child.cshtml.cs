using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace ChildGrowth.WebPage.Pages
{
    public class ChildModel : PageModel
    {
        // Danh sách dữ liệu mẫu cho trẻ
        public List<ChildRecord> Children { get; set; }

        public void OnGet()
        {
            // Khởi tạo dữ liệu mẫu cho mục demo
            Children = new List<ChildRecord>
            {
                new ChildRecord { Id = 1, Name = "John Doe", Age = 5, Height = 110, Weight = 18 },
                new ChildRecord { Id = 2, Name = "Jane Doe", Age = 7, Height = 120, Weight = 22 }
            };
        }
    }

    // Lớp mô hình cho thông tin trẻ
    public class ChildRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }  // chiều cao tính theo cm
        public int Weight { get; set; }  // cân nặng tính theo kg
    }
}
