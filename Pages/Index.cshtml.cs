using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VisitTracker.Pages
{
    public class IndexModel : PageModel
    {
        public int VisitCount { get; set; }
        public string? ClientIP { get; set; }
        public string? ClientTimeZone { get; set; }

        public void OnGet()
        {
            // Get client IP
            ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Handle visit count cookie
            var visitCookie = Request.Cookies["VisitCount"];
            VisitCount = 1;

            if (visitCookie != null && int.TryParse(visitCookie, out int count))
            {
                VisitCount = count + 1;
            }

            Response.Cookies.Append("VisitCount",
                VisitCount.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1), // Persistent cookie
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });

            // Get timezone from cookie if exists
            ClientTimeZone = Request.Cookies["ClientTimeZone"];
        }

        public IActionResult OnGetSaveTimeZone(string timeZone)
        {
            if (!string.IsNullOrEmpty(timeZone))
            {
                Response.Cookies.Append("ClientTimeZone",
                    timeZone,
                    new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(1),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Lax
                    });
            }
            return Content("");
        }
    }
}