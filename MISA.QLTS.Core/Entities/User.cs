using MISA.QLTS.Core.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class User
    {
        /// <summary>
        /// Tên tài khoản đăng nhập
        /// </summary>
        [NotEmpty]
        [NotDuplicate]
        [PropertyName("Tài khoản đăng nhập")]
        public string UserName { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        [NotEmpty]
        [PropertyName("Mật khẩu")]
        public string Password { get; set; }
    }
}
