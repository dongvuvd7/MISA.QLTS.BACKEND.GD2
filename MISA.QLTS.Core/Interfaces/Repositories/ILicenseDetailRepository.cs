using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface ILicenseDetailRepository
    {
        #region Methods definition
        /// <summary>
        /// Lấy ra các bản ghi licensedetail theo id chứng từ
        /// Để lấy ra detail (chi tiết nguyên giá) chứa các cặp {nguồn nguyên giá, giá trị}
        /// </summary>
        /// <param name="licenseId">id chứng từ muốn lấy</param>
        /// <returns>Các bản ghi LicenseDetail tương ứng idLicenseDetail đầu vào</returns>
        /// Created by: VDDong (18/08/2022)
        IEnumerable<LicenseDetail> GetByLicenseId(Guid licenseId);

        #endregion
    }
}
