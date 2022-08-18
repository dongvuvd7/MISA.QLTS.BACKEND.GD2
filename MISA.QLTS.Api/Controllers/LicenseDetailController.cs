using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Interfaces.Repositories;

namespace MISA.QLTS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class LicenseDetailController : ControllerBase
    {
        ILicenseDetailRepository _licenseDetailRepository;

        public LicenseDetailController(ILicenseDetailRepository licenseDetailRepository)
        {
            _licenseDetailRepository = licenseDetailRepository;
        }

        /// <summary>
        /// Lấy ra các bản ghi theo id chứng từ
        /// Để lấy ra detail (chi tiết nguyên giá) chứa các cặp {nguồn nguyên giá, giá trị}
        /// </summary>
        /// <param name="licenseId">id chứng từ muốn lấy</param>
        /// <returns>Các bản ghi LicenseDetail tương ứng</returns>
        /// Created by: VDDong (18/08/2022)
        [HttpGet]
        public IActionResult GetByLicenseId(Guid licenseId)
        {
            var licenseDetails = _licenseDetailRepository.GetByLicenseId(licenseId);
            return Ok(licenseDetails);
        }
    }
}
