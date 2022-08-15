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
    public class DepartmentsController : BaseController<Department>
    {
        #region Constructor
        IDepartmentRepository _departmentRepository;
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService) : base(departmentRepository, departmentService)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Controllers methods
        /// <summary>
        /// Lấy bộ phận sử dụng theo tên
        /// </summary>
        /// <param name="name">Tên bộ phận sử dụng muốn lấy</param>
        /// <returns>Bộ phận sử dụng tương ứng</returns>
        /// Created by: VDDong (27/06/2022)
        [HttpGet("DepartmentName")]
        public IActionResult Get(string entityName)
        {
            try
            {
                var entity = _departmentRepository.GetByName(entityName);
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
