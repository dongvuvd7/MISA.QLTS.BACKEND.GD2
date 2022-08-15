using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.Api.Controllers
{
    [Authorize]
    public class LicenseController : BaseController<License>
    {
        ILicenseRepository _licenseRepository;

        public LicenseController(ILicenseRepository licenseRepository, ILicenseService licenseService) : base(licenseRepository, licenseService)
        {
            _licenseRepository = licenseRepository;
        }

        /// <summary>
        /// Lấy mã chứng từ mới
        /// </summary>
        /// <returns>Mã chứng từ mới</returns>
        /// Created by: VDDong (06/07/2022)
        [HttpGet("NewCode")]
        public IActionResult GetNewCode()
        {
            var newCode = _licenseRepository.GetNewCode();
            return Ok(newCode);
        }

        /// <summary>
        /// Lấy danh sách chứng từ theo điều kiện tìm kiếm và phân trang
        /// </summary>
        /// <param name="searchText">Tìm kiếm theo mã chứng từ hoặc ghi chú</param>
        /// <param name="pageSize">số bản ghi / 1 trang</param>
        /// <param name="pageNumber">trang hiện tại</param>
        /// <returns>Danh sách các bản ghi 
        /// Created by: VDDong (06/07/2022)
        [HttpGet("Filter")]
        public IActionResult Filter(string? searchText, int? pageSize, int? pageNumber)
        {
            var licenses = _licenseRepository.Filter(searchText, pageSize, pageNumber);
            return Ok(licenses);
        }

    }
}
