using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class LicenseDetail
    {
        /// <summary>
        /// Id tài sản
        /// Khóa ngoại liên kết đến khóa chính AssetId ở bảng Asset
        /// </summary>
        public Guid? AssetId { get; set; }

        /// <summary>
        /// Id chứng từ
        /// Khóa ngoại liên kết đến khóa chính LicenseId ở bảng License
        /// </summary>
        public Guid? LicenseId { get; set; }
        
        /// <summary>
        /// Chi tiết nguyên giá
        /// Chuỗi gồm các cặp {source, value}
        /// </summary>
        public string? Detail { get; set; }
    }
}
