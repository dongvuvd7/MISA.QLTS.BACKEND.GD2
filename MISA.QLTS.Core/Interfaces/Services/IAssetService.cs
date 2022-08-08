using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Services
{
    public interface IAssetService : IBaseService<Asset>
    {
        #region Methods definition
        /// <summary>
        /// Xuất file excel danh sách tài sản
        /// </summary>
        /// <returns>File excel DSTS</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public Stream ExportExcel();

        #endregion
    }
}
