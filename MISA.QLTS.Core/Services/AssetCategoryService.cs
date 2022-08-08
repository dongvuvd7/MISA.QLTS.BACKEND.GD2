using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Services
{
    public class AssetCategoryService : BaseService<AssetCategory>, IAssetCategoryService
    {
        #region Constructor
        IAssetCategoryRepository _assetCategoryRepository;
        public AssetCategoryService(IAssetCategoryRepository assetCategoryRepository) : base(assetCategoryRepository)
        {
            _assetCategoryRepository = assetCategoryRepository;
        }
        #endregion
    }
}
