using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface IAssetCategoryRepository : IBaseRepository<AssetCategory>
    {
        #region Methods definition
        /// <summary>
        /// Lấy loại tài sản theo tên loại tài sản
        /// (Phục vụ việc thêm dữ liệu từ file excel)
        /// </summary>
        /// <param name="name">Tên loại tài sản muốn tìm kiếm theo</param>
        /// <returns>Loại tài sản tương ứng</returns>
        /// Created by: VDDong (27/06/2022)
        AssetCategory GetByName(string name);

        #endregion
    }
}
