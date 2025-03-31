
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public abstract class AuthenticatedPageModel : Microsoft.AspNetCore.Mvc.RazorPages.PageModel
{
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        // Lấy token từ cookie
        var token = Request.Cookies["token"];

        // Nếu token không tồn tại hoặc rỗng, chuyển hướng về trang Login
        if (string.IsNullOrEmpty(token))
        {
            context.Result = new RedirectToPageResult("/Login");
            return; // Dừng thực hiện các xử lý tiếp theo
        }

        base.OnPageHandlerExecuting(context);
    }
}
