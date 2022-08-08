using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using System.Security.Claims;

namespace MISA.QLTS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Xác thực đăng nhập
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns>
        /// -1: Đăng nhập thất bại
        /// token ngẫu nhiên ở cookie: Đăng nhập thành công
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> OnPostAsync([FromBody] User userLogin)
        {
            var user = AuthenticateUser(userLogin.UserName, userLogin.Password);
            if (user == null)
            {
                return StatusCode(200, -1);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "NormalUser"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                //Làm mới phiên xác thực phải được cho phép.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                //Thời gian mà phiếu xác thực hết hạn. Giá trị được đặt ở đây sẽ ghi đè tùy chọn ExpireTimeSpan của CookieAuthenticationOptions được đặt với AddCookie.

                //IsPersistent = true,
                //Liệu phiên xác thực có tồn tại qua nhiều yêu cầu hay không. Khi được sử dụng với cookie, kiểm soát xem thời gian tồn tại của cookie là tuyệt đối (khớp với thời gian tồn tại của vé xác thực) hay dựa trên phiên.

                //IssuedUtc = <DateTimeOffset>,
                //Thời gian mà phiếu xác thực được phát hành.

                //RedirectUri = <string>
                //Đường dẫn đầy đủ hoặc URI tuyệt đối sẽ được sử dụng làm giá trị phản hồi chuyển hướng http.
            };

            //SignInAsync tạo một cookie được mã hóa và thêm nó vào phản hồi
            //ClaimsPrincipal tạo một cookie nắm giữ thông tin người dùng
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), null);


            return Ok();

        }

        /// <summary>
        /// Check username, password có đúng không
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private User AuthenticateUser(string username, string password)
        {
            if (username == "vddong" && password == "123456")
            {
                return new User()
                {
                    UserName = "vddong"
                };
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Hàm logout, xóa cookie
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut
            //Xóa cookie đã tồn tại
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page
            return Ok();
        }
        
    }
}
