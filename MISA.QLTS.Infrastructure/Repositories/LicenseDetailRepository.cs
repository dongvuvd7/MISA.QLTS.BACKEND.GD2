using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Infrastructure.Repositories
{
    public class LicenseDetailRepository : ILicenseDetailRepository
    {
        #region Constructor
        /// <summary>
        /// Do LicenseDetailRepository không kế thừa BaseRepository nên cần phải khởi tạo chuỗi kết nối tới database
        /// Created by: VDDong (06/07/2022)
        /// </summary>
        protected string connectionString;
        protected MySqlConnection sqlConnection;
        
        public LicenseDetailRepository(IConfiguration configuration)
        {
            //connectionString = configuration.GetConnectionString("MISA_QLTS_DBFORCE");
            connectionString = configuration.GetConnectionString("MISA_QLTS_LOCAL");
        }

        #endregion

        #region Methods
        /// <summary>
        /// Lấy ra các bản ghi licensedetail theo id chứng từ
        /// Để lấy ra detail (chi tiết nguyên giá) chứa các cặp {nguồn nguyên giá, giá trị}
        /// </summary>
        /// <param name="licenseId">id chứng từ muốn lấy</param>
        /// <returns>Các bản ghi LicenseDetail tương ứng idLicenseDetail đầu vào</returns>
        /// Created by: VDDong (18/08/2022)
        public IEnumerable<LicenseDetail> GetByLicenseId(Guid licenseId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //Build câu truy vấn
                var sqlCommand = $"SELECT * FROM LicenseDetail WHERE LicenseId = @LicenseId ORDER BY AssetId";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add($"@LicenseId", licenseId);
                //Thực thi câu truy vấn
                var result = sqlConnection.Query<LicenseDetail>(sqlCommand, param: dynamicParameters);
                return result;
            }
        }

        #endregion
    }
}
