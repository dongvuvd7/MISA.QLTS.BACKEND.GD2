using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Services
{
    public interface IBaseService<MISAEntity>
    {
        #region Methods definition
        /// <summary>
        /// Validate dữ liệu khi thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi muốn thêm</param>
        /// <returns>Số bản ghi đã thêm được</returns>
        /// CreatedBy: VDDong (16/06/2022)
        int InsertService(MISAEntity entity);

        /// <summary>
        /// Validate dữ liệu khi sửa bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi muốn sửa</param>
        /// <param name="entityId">Id bản ghi muốn sửa</param>
        /// <returns>Số bản ghi đã sửa được</returns>
        /// CreatedBy: VDDong (16/06/2022)
        int UpdateService(MISAEntity entity, Guid entityId);

        /// <summary>
        /// Thêm hàng loạt các bản ghi lên database
        /// </summary>
        /// <param name="entities">Danh sách các bản ghi (đã pass validate required và check trùng mã)</param>
        /// <returns>Số bản ghi thêm được</returns>
        /// Created by: VDDong (06/07/2022)
        object MultipleInsertService(List<MISAEntity> entities);

        #endregion
    }
}
