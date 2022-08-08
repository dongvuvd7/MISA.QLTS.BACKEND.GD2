using MISA.QLTS.Core.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class License
    {
        #region Properties of License
        /// <summary>
        /// Id chứng từ
        /// Khóa chính
        /// </summary>
        [PrimaryKey]
        public Guid LicenseId { get; set; }

        /// <summary>
        /// Mã chứng từ
        /// </summary>
        [NotEmpty]
        [NotDuplicate]
        [PropertyName("Mã chứng từ")]
        public string LicenseCode { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        [NotEmpty]
        public DateTime LicenseDate { get; set; }

        /// <summary>
        /// Ngày ghi tăng
        /// </summary>
        [NotEmpty]
        public DateTime WriteDate { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// Một chứng từ phải chứa ít nhất 1 tài sản
        /// </summary>
        [NotEmpty]
        public float TotalCost { get; set; }

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

        /// <summary>
        /// Danh sách chi tiết chứng từ
        /// Viết ghép ở đây để khi vào LicenseRepository sẽ tách ra để insert trường này vào bảng LicenseDetail
        /// Notmap để khi build câu truy vấn của License sẽ không đưa trường này vào câu truy vấn
        /// </summary>
        [NotMap]
        public LicenseDetail[] LicenseDetail { get; set; }

    }
}
