using MISA.QLTS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Interfaces.Repositories
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        #region Methods definition
        /// <summary>
        /// Lấy bộ phận sử dụng theo tên
        /// </summary>
        /// <param name="name">Tên bộ phận sử dụng muốn lấy</param>
        /// <returns>Bộ phận sử dụng tương ứng</returns>
        /// Created by: VDDong (27/06/2022)
        Department GetByName(string name);

        #endregion
    }
}
