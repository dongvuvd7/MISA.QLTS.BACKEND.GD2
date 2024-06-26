﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.Api.Controllers
{
    [Authorize]
    public class AssetsController : BaseController<Asset>
    {
        #region Constructor
        IAssetRepository _assetRepository;
        IAssetService _assetService;
        public AssetsController(IAssetRepository assetRepository, IAssetService assetService) : base(assetRepository, assetService)
        {
            _assetRepository = assetRepository;
            _assetService = assetService;
        }
        #endregion

        #region Controllers methods
        /// <summary>
        /// Lấy danh sách tài sản theo điều kiện tìm kiếm, lọc, phân trang
        /// Kết hợp nhóm theo loại tài sản, bộ phận sử dụng
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="assetCategory">Loại tài sản</param>
        /// <param name="department">Bộ phận sử dụng</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (16/06/2022)
        [HttpGet("Filter")]
        public IActionResult Filter(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber)
        {
            try
            {
                var assets = _assetRepository.Filter(searchText, assetCategory, department, pageSize, pageNumber);
                return Ok(assets);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Lấy danh sách tài sản theo điều kiện tìm kiếm, lọc, phân trang
        /// Kết hợp nhóm theo loại tài sản, bộ phận sử dụng
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="assetCategory">Loại tài sản: Id</param>
        /// <param name="department">Bộ phận sử dụng: Id</param>
        /// <param name="pageSize">Số bản ghi trên trang</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (13/07/2022)
        [HttpGet("Filters")]
        public IActionResult Filters(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber)
        {
            try
            {
                var assets = _assetRepository.Filters(searchText, assetCategory, department, pageSize, pageNumber);
                return Ok(assets);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Lấy mã tài sản mới
        /// </summary>
        /// <returns>Mã tài sản mới</returns>
        /// CreatedBy: VDDong (16/06/2022)
        [HttpGet("NewCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var assetCode = _assetRepository.GetNewCode();
                return Ok(assetCode);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Xuất file excel danh sách tài sản
        /// </summary>
        /// <returns>File excel DSTS</returns>
        /// CreatedBy: VDDong (16/06/2022)
        [HttpGet("ExportExcel")]
        public IActionResult Export()
        {
            try
            {
                var stream = _assetService.ExportExcel();
                string excelName = "DanhSachTaiSan_ExportByVDDong.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Lấy danh sách các tài sản theo mã chứng từ
        /// API gọi khi click chứng từ bảng trên, danh sách tài sản tương ứng show bảng dưới
        /// </summary>
        /// <param name="licenseId"></param>
        /// <returns>Danh sách tài sản theo mã chứng từ</returns>
        /// Created by: VDDong (02/08/2022)
        [HttpGet("GetByLicenseId/{licenseId}")]
        public IActionResult GetByLicenseId(Guid licenseId)
        {
            var assets = _assetRepository.GetByLicenseId(licenseId);
            return Ok(assets);
        }

        /// <summary>
        ///  Lấy ra danh sách tài sản có tìm kiếm, phân trang theo id chứng từ
        /// </summary>
        /// <param name="licenseId">Id chứng từ liên kết tài sản</param>
        /// <returns>Danh sách tài sản thuộc chứng từ tương ứng (có tìm kiếm, phân trang)</returns>
        [HttpGet("GetFilterByLicenseId")]
        public IActionResult GetFilterByLicenseId(Guid licenseId, string? searchText, int? pageSize, int? pageNumber)
        {
            var result = _assetRepository.GetFilterByLicenseId(licenseId, searchText, pageSize, pageNumber);
            return Ok(result);
        }

        /// <summary>
        /// Kiểm tra xem tài sản có liên kết với chứng từ nào không
        /// (Dùng check trước khi xóa, nếu có liên kết thì không cho xóa tài sản đó)
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns>Mã tài sản và Mã chứng từ nếu có</returns>
        [HttpGet("AssetReferencedLicense/{assetId}")]
        public IActionResult AssetReferencedLicense(Guid assetId)
        {
            var assetReferencedLicense = _assetRepository.CheckAssetReferencedToLicense(assetId);
            return Ok(assetReferencedLicense);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="assetCategory"></param>
        /// <param name="department"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// <exception cref="MISAValidateException"></exception>
        [HttpGet("FilterListAssetsForLicense")]
        public IActionResult FilterListAssetsForLicense(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber, Guid licenseId)
        {
            try
            {
                var assets = _assetRepository.FiltersListAssetsForLicense(searchText, assetCategory, department, pageSize, pageNumber, licenseId);
                return Ok(assets);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }

        }

        #endregion
    }
}
