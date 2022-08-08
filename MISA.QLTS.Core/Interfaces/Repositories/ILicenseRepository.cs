using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface ILicenseRepository : IBaseRepository<License>
    {
        #region Methods definition
        /// <summary>
        /// Lấy số chứng từ mới
        /// </summary>
        /// <returns>Mã chứng từ mới</returns>
        /// CreatedBy: VDDong (06/07/2022)
        string GetNewCode();

        /// <summary>
        /// Lấy danh sách chứng từ theo điều kiện tìm kiếm và phân trang
        /// </summary>
        /// <param name="searchText">Từ khoá tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số thứ tự trang</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (06/07/2022)
        object Filter(string? searchText, int? pageSize, int? pageNumber);

        #endregion
    }
}
