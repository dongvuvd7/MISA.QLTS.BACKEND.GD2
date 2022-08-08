using MISA.QLTS.Core.AttributeCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Entities
{
    public class CostSource
    {
        /// <summary>
        /// Id nguồn nguyên giá
        /// </summary>
        [PrimaryKey]
        public Guid CostSourceId { get; set; }

        /// <summary>
        /// Mã nguồn nguyên giá
        /// </summary>
        public string CostSourceCode { get; set; }

        /// <summary>
        /// Tên nguồn nguyên giá
        /// </summary>
        public string CostSourceName { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string CostSourceNote { get; set; }

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
    }
}
