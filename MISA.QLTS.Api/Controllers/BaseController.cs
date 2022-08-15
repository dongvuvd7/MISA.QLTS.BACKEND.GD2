using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;

namespace MISA.QLTS.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController<MISAEntity> : ControllerBase
    {
        #region Constructor
        IBaseRepository<MISAEntity> _baseRepository;
        IBaseService<MISAEntity> _baseService;

        public BaseController(IBaseRepository<MISAEntity> baseRepository, IBaseService<MISAEntity> baseService)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }
        #endregion

        #region Controllers methods
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        /// Created by: VDDong (16/06/2022)
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var entities = _baseRepository.Get();
                return Ok(entities);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Lấy một bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID bản ghi cần lấy</param>
        /// <returns>Bản ghi tương ứng id</returns>
        /// Created by: VDDong (16/06/2022)
        [HttpGet("{entityId}")]
        public IActionResult Get(Guid entityId)
        {
            try
            {
                var entity = _baseRepository.Get(entityId);
                return Ok(entity);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi thêm vào database</param>
        /// <returns></returns>
        /// Created by: VDDong (16/06/2022)
        [HttpPost]
        public IActionResult Post(MISAEntity entity)
        {
            var res = _baseService.InsertService(entity);
            if (res > 0) return StatusCode(201, res);
            else return StatusCode(200, res);
            
        }

        /// <summary>
        /// Sửa bản ghi có id là entityId
        /// </summary>
        /// <param name="entity">Bản ghi cần sửa</param>
        /// <param name="entityId">Id bản ghi cần sửa</param>
        /// <returns>Số bản ghi được sửa thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        [HttpPut("{entityId}")]
        public IActionResult Put(MISAEntity entity, Guid entityId)
        {
            var res = _baseService.UpdateService(entity, entityId);
            return Ok(res);

        }

        /// <summary>
        /// Xoá bản ghi có id là entityId
        /// </summary>
        /// <param name="entityIds">Chuỗi các Id muốn xóa</param>
        /// <returns>Số bản ghi đã xoá thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        [HttpDelete]
        public IActionResult Delete(string? entityIds)
        {
            try
            {
                var res = _baseRepository.Delete(entityIds);
                return Ok(res);
            }
            catch (Exception e)
            {
                throw new MISAValidateException(e);
            }
            
        }

        /// <summary>
        /// Thêm hàng loạt các bản ghi lên database
        /// </summary>
        /// <param name="entities">Danh sách các bản ghi (đã pass validate required và check trùng mã)</param>
        /// <returns>Số bản ghi thêm được và thông báo lỗi (nếu có)</returns>
        /// Created by: VDDong (06/07/2022)
        [HttpPost("MultiplePost")]
        public IActionResult Post(List<MISAEntity> entities)
        {
            var res = _baseService.MultipleInsertService(entities);
            return Ok(res);
            
        }

        #endregion
    }
}
