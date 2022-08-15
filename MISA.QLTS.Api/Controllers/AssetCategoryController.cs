using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.Api.Controllers
{
    [Authorize]
    public class AssetCategoriesController : BaseController<AssetCategory>
    {
        #region Constructor
        IAssetCategoryRepository _assetCategoryRepository;
        public AssetCategoriesController(IAssetCategoryRepository assetCategoryRepository, IAssetCategoryService assetCategoryService) : base(assetCategoryRepository, assetCategoryService)
        {
            _assetCategoryRepository = assetCategoryRepository;
        }
        #endregion

        #region Controllers methods
        /// <summary>
        /// Lấy loại tài sản theo tên loại tài sản
        /// (Phục vụ việc thêm dữ liệu từ file excel)
        /// </summary>
        /// <param name="name">Tên loại tài sản muốn tìm kiếm theo</param>
        /// <returns>Loại tài sản tương ứng</returns>
        /// Created by: VDDong (27/06/2022)
        [HttpGet("AssetCategoryName")]
        public IActionResult Get(string entityName)
        {
            try
            {
                var entity = _assetCategoryRepository.GetByName(entityName);
                return Ok(entity);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        #endregion
    }
}
