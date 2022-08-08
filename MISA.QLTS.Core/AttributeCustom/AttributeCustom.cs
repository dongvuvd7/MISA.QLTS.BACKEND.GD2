using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.AttributeCustom
{
    #region [Attributes custom]
    /// <summary>
    /// Thuộc tính là khóa chính
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    { 
    }

    /// <summary>
    /// Thuộc tính không được phép trống
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty : Attribute
    {
    }

    /// <summary>
    /// Thuộc tính không được phép trùng
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotDuplicate : Attribute
    {
    }

    /// <summary>
    /// Đánh dấu tên thuộc tính
    /// Dùng khi muốn lấy tên tiếng Việt của thuộc tính (để validate chẳng hạn)
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyName : Attribute
    {
        public string Name = String.Empty;
        public PropertyName(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Trường kiểm tra xem property có quá số lượng ký tự cho phép không
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLength : Attribute
    {
        public int Length = 0;
        public MaxLength(int length)
        {
            Length = length;
        }
    }

    /// <summary>
    /// Trường kiểm tra email có hợp lệ không
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailField : Attribute
    {
    }

    /// <summary>
    /// Trường kiểm tra số điện thoại có hợp lệ không
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumberField : Attribute
    {
    }

    /// <summary>
    /// Trường kiểm tra định dạng website
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WebsiteField : Attribute
    {
    }

    /// <summary>
    /// Thuộc tính không dùng để build câu truy vấn
    /// Created by: VDDong (16/06/2022)
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class NotMap : Attribute
    {
    }

    #endregion
}
