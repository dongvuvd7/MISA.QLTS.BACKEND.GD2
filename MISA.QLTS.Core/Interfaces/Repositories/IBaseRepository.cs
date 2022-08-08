using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface IBaseRepository<MISAEntity>
    {
        #region Methods definition
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        /// CreatedBy: VDDong (16/06/2022)
        IEnumerable<MISAEntity> Get();

        /// <summary>
        /// Lấy ra 1 bản ghi theo id
        /// </summary>
        /// <param name="entityId">Id của bản ghi muốn tìm</param>
        /// <returns>Bản ghi theo id đã nhập</returns>
        /// CreatedBy: VDDong (16/06/2022)
        MISAEntity Get(Guid entityId);

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi muốn thêm</param>
        /// <returns>Số bản ghi đã thêm thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        int Insert(MISAEntity entity);

        /// <summary>
        /// Sửa bản ghi có id là entityId
        /// </summary>
        /// <param name="entity">Bản ghi muốn sửa</param>
        /// <param name="entityId">Id bản ghi muốn sửa</param>
        /// <returns>Số bản ghi được sửa thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        int Update(MISAEntity entity, Guid entityId);

        /// <summary>
        /// Xoá bản ghi có id là entityId
        /// </summary>
        /// <param name="entityIds">Chuỗi các id muốn xóa</param>
        /// <returns>Số bản ghi đã xoá thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        int Delete(string? entityIds);

        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="propName">Tên trường kiểm tra trùng</param>
        /// <param name="primaryKey">Khóa chính</param>
        /// <param name="propValue">Giá trị trường kiểm tra trùng</param>
        /// <param name="keyValue">Giá trị khóa chính</param>
        /// <returns>true - Mã đã tồn tại; false - Mã chưa tồn tại</returns>
        /// CreatedBy: VDDong (16/06/2022)
        bool CheckDuplicate(string propName, string primaryKey, object propValue, object keyValue);

        /// <summary>
        /// Thêm hàng loạt các bản ghi lên database
        /// </summary>
        /// <param name="entities">Danh sách các bản ghi (đã pass validate required và check trùng mã)</param>
        /// <returns>Số bản ghi thêm được</returns>
        /// Created by: VDDong (06/07/2022)
        int MultipleInsert(List<MISAEntity> entities);

        #endregion
    }
}
