using MISA.QLTS.Core.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class Asset
    {
        #region Properties
        /// <summary>
        /// Khoá chính
        /// </summary>
        [PrimaryKey]
        public Guid AssetId { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        [NotEmpty]
        [NotDuplicate]
        [MaxLength(50)]
        [PropertyName("Mã tài sản")]
        public string AssetCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        [NotEmpty]
        [MaxLength(255)]
        [PropertyName("Tên tài sản")]
        public string AssetName { get; set; }

        /// <summary>
        /// Id tổ chức
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// Mã tổ chức
        /// </summary>
        public string? OrganizationCode { get; set; }

        /// <summary>
        /// Tên tổ chức
        /// </summary>
        public string? OrganizationName { get; set; }

        /// <summary>
        /// Id bộ phận
        /// </summary>
        [NotEmpty]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã bộ phận
        /// </summary>
        [NotEmpty]
        [PropertyName("Mã bộ phận sử dụng")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên bộ phận
        /// </summary>
        [NotEmpty]
        [PropertyName("Tên bộ phận sử dụng")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Id loại tài sản
        /// </summary>
        [NotEmpty]
        public Guid AssetCategoryId { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        [NotEmpty]
        [PropertyName("Mã loại tài sản")]
        public string AssetCategoryCode { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        [NotEmpty]
        [PropertyName("Tên loại tài sản")]
        public string AssetCategoryName { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>
        [NotEmpty]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Ngày bắt đầu sử dụng
        /// </summary>
        [NotEmpty]
        public DateTime UseDate { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        [NotEmpty]
        public double Cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [NotEmpty]
        public int Quantity { get; set; }

        /// <summary>
        /// Tỉ lệ hao mòn
        /// </summary>
        [NotEmpty]
        public float DepreciationRate { get; set; }

        /// <summary>
        /// Năm theo dõi
        /// </summary>
        public int TrackedYear { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        [NotEmpty]
        public int LifeTime { get; set; }

        /// <summary>
        /// Năm sản xuất
        /// </summary>
        public int ProductionYear { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        #endregion
    }
}
