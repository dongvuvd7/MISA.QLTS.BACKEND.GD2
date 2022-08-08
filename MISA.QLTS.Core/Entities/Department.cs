using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class Department
    {
        #region Properties


        /// <summary>
        /// Id bộ phận
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã bộ phận
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên bộ phận
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Có phải cha hay không
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// Id cha
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Id tổ chức
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        #endregion
    }
}
