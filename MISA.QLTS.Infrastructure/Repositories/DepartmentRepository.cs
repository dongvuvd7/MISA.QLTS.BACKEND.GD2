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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        #region Constructor
        public DepartmentRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy bộ phận sử dụng theo tên
        /// </summary>
        /// <param name="name">Tên bộ phận sử dụng muốn lấy</param>
        /// <returns>Bộ phận sử dụng tương ứng</returns>
        /// Created by: VDDong (27/06/2022)
        public Department GetByName(string name)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //using store procedure
                var tableName = "Department";
                var sqlCommand = $"Proc_GetByName";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("tableName", tableName);
                dynamicParameters.Add("entityName", name);
                var entity = sqlConnection.QueryFirstOrDefault<Department>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return entity;
            }
        }

        #endregion
    }
}
