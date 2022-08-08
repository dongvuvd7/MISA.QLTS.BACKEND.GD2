using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class AssetCategory
    {
        #region Properties
        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid AssetCategoryId { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string AssetCategoryCode { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string AssetCategoryName { get; set; }

        /// <summary>
        /// Id tổ chức
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// Tỉ lệ hao mòn
        /// </summary>
        public float DepreciationRate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

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
