﻿using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface IAssetRepository : IBaseRepository<Asset>
    {
        #region Methods definition
        /// <summary>
        /// Lấy mã tài sản mới
        /// </summary>
        /// <returns>Mã tài sản mới</returns>
        /// CreatedBy: VDDong (16/06/2022)
        string GetNewCode();

        /// <summary>
        /// Lấy danh sách tài sản theo điều kiện tìm kiếm, lọc, phân trang
        /// </summary>
        /// <param name="searchText">Từ khoá tìm kiếm</param>
        /// <param name="assetCategory">Tên loại tài sản</param>
        /// <param name="department">Tên bộ phận sử dụng</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số thứ tự trang</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (16/06/2022)
        object Filter(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber);

        /// <summary>
        /// lấy danh sách tài sản theo điều kiện tìm kiếm, lọc, phân trang
        /// category và department ở đây là id
        /// </summary>
        /// <param name="searchText">Từ khoá tìm kiếm</param>
        /// <param name="assetCategory">Tên loại tài sản</param>
        /// <param name="department">Tên bộ phận sử dụng</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số thứ tự trang</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (13/07/2022)
        object Filters(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber);

        /// <summary>
        /// Lấy toàn bộ bản ghi theo code giảm dần
        /// Phục vụ việc xuất khẩu
        /// </summary>
        /// <returns>Danh sách các bản ghi theo code giảm dần</returns>
        /// CreatedBy: VDDong (18/07/2022)
        IEnumerable<Asset> GetExport();

        /// <summary>
        /// Lấy ra danh sách tài sản theo id chứng từ
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns>Danh sách tài sản thuộc chứng từ</returns>
        IEnumerable<Asset> GetByLicenseId(Guid licenseId);

        #endregion
    }
}