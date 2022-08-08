using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.CORE.Enums
{
    /// <summary>
    /// Phân biệt trạng thái thêm hay sửa
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    public enum HttpType 
    {
        POST = 0, //Đăng mới một bản ghi
        PUT = 1 //Chỉnh sửa một bản ghi
    }
}
