using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repositories
{
    public class AssetCategoryRepository : BaseRepository<AssetCategory>, IAssetCategoryRepository
    {
        #region Constructor
        public AssetCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy loại tài sản theo tên loại tài sản
        /// (Phục vụ việc thêm dữ liệu từ file excel)
        /// </summary>
        /// <param name="name">Tên loại tài sản muốn tìm kiếm theo</param>
        /// <returns>Loại tài sản tương ứng</returns>
        /// Created by: VDDong (27/06/2022)
        public AssetCategory GetByName(string name)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //using store procedure
                var tableName = "AssetCategory";
                var sqlCommand = $"Proc_GetByName";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("tableName", tableName);
                dynamicParameters.Add("entityName", name);
                var entity = sqlConnection.QueryFirstOrDefault<AssetCategory>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return entity;
            }
        }

        #endregion
    }
}
