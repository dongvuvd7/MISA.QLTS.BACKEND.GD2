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
    public class AssetRepository : BaseRepository<Asset>, IAssetRepository
    {
        #region Constructor
        public AssetRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region Methods
        // <summary>
        /// Lấy danh sách tài sản theo điều kiện tìm kiếm, lọc, phân trang
        /// Kết hợp cả nhóm theo loại tài sản và bộ phận
        /// </summary>
        /// <param name="searchText">Từ khoá tìm kiếm</param>
        /// <param name="assetCategory">Tên loại tài sản</param>
        /// <param name="department">Tên bộ phận sử dụng</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số thứ tự trang</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public object Filter(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber)
        {
            using(sqlConnection = new MySqlConnection(connectionString))
            {
                searchText = "%" + searchText + "%";
                //Build câu truy vấn theo các điều kiện
                var sqlCommand = "FROM Asset WHERE (AssetCode LIKE @searchText OR AssetName LIKE @searchText) ";
                if (assetCategory != null)
                    sqlCommand += "AND AssetCategoryName = @assetCategory ";
                if (department != null)
                    sqlCommand += "AND DepartmentName = @department ";

                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@searchText", searchText);
                dynamicParameters.Add("@assetCategory", assetCategory);
                dynamicParameters.Add("@department", department);

                //Đếm tổng số bản ghi
                var countCommand = $"SELECT COUNT(*) " + sqlCommand;
                var totalRecords = sqlConnection.QueryFirstOrDefault<int>(countCommand, param: dynamicParameters);

                //Lấy ra các bản ghi theo các điều kiện
                sqlCommand = $"SELECT * " + sqlCommand + $"ORDER BY AssetCode DESC LIMIT @start,@pageSize";
                dynamicParameters.Add("@start", (pageNumber - 1) * pageSize);
                dynamicParameters.Add("@pageSize", pageSize);

                var assets = sqlConnection.Query<Asset>(sqlCommand, param: dynamicParameters);
                return new
                {
                    totalRecords = totalRecords,
                    data = assets
                };
            }
        }

        /// <summary>
        /// Lấy danh sách tài sản theo điều kiện tìm kiếm, lọc, phân trang
        /// Kết hợp nhóm theo loại tài sản, bộ phận sử dụng
        /// </summary>
        /// <param name="searchText">Từ khoá tìm kiếm</param>
        /// <param name="assetCategory">Tên loại tài sản</param>
        /// <param name="department">Tên bộ phận sử dụng</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số thứ tự trang</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// CreatedBy: VDDong (13/07/2022)
        public object Filters(string? searchText, string? assetCategory, string? department, int? pageSize, int? pageNumber)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {

                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();

                if (assetCategory == null) assetCategory = "";
                if (department == null) department = "";
                var sqlCountCommand = "Proc_GetTotalAssetFilters";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@searchText", searchText);
                dynamicParameters.Add("@departmentId", department);
                dynamicParameters.Add("@assetCategoryId", assetCategory);
                //Total records
                var totalRecords = sqlConnection.QueryFirstOrDefault<int>(sqlCountCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                //get records by condition filters
                var sqlCommand = "Proc_GetAssetFilters";
                dynamicParameters.Add("@pageNumber", pageNumber);
                dynamicParameters.Add("@pageSize", pageSize);
                var assets = sqlConnection.Query<Asset>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return new
                {
                    totalRecords = totalRecords,
                    data = assets
                };

            }

        }

        /// <summary>
        /// Lấy ra danh sách tài sản theo chứng từ (API để click bảng trên show bảng dưới)
        /// Ý tưởng:
        /// + Lấy danh sách các assetId theo licenseId từ bảng LicenseDetail
        /// + Sau đó SELECT tất cả các bản ghi từ bảng Asset theo danh sách các AssetId vừa rồi
        /// </summary>
        /// <param name="licenseId">Id chứng từ liên kết đến tài sản (danh sách tài sản)</param>
        /// <returns>Danh sách tài sản theo licenseId tương ứng</returns>
        /// Created by: VDDong (02/08/2022)
        public IEnumerable<Asset> GetByLicenseId(Guid licenseId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //Lấy danh sách các assetId theo licenseId từ bảng LicenseDetail
                var sqlCommand = "SELECT AssetId FROM LicenseDetail WHERE LicenseId = @licenseId";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@licenseId", licenseId);
                var assetIds = sqlConnection.Query<Guid>(sqlCommand, param: dynamicParameters);
                //SELECT tất cả các bản ghi theo danh sách các assetId vừa rồi
                sqlCommand = "SELECT * FROM Asset WHERE AssetId IN (";
                for (int i = 0; i < assetIds.Count(); i++)
                {
                    sqlCommand += $"@AssetId{i},";
                    dynamicParameters.Add($"@AssetId{i}", assetIds.ElementAt<Guid>(i));
                }
                sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1); //Bỏ dấu , ở cuối chuỗi
                sqlCommand += ") ORDER BY AssetId";
                var assets = sqlConnection.Query<Asset>(sqlCommand, param: dynamicParameters);

                return assets;
            }
        }

        /// <summary>
        /// Lấy ra các bản ghi để xuất khẩu ra file Excel
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        /// CreatedBy: VDDong (23/07/2022)
        public IEnumerable<Asset> GetExport()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                ////Build query
                var sqlCommand = $"SELECT * FROM Asset ORDER BY AssetCode DESC";
                //Run query
                var entities = sqlConnection.Query<Asset>(sqlCommand);
                return entities;

            }
        }


        /// <summary>
        /// Lấy mã tài sản mới nhất
        /// </summary>
        /// <returns>Mã tài sản mới nhất</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public string GetNewCode()
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //build query
                var sqlCommand = "SELECT a.AssetCode FROM Asset a ORDER BY a.CreatedDate DESC LIMIT 1";
                var newCode = sqlConnection.QueryFirstOrDefault<string>(sqlCommand);
                if (newCode != null)
                {
                    //Mã mới  = phần tử cuối cùng của mã cũ + 1
                    string numberString = string.Empty;
                    for (int i = newCode.Length - 1; i >= 0; i--)
                    {
                        if (Char.IsDigit(newCode[i]))
                            numberString += newCode[i];
                        else break;
                    }
                    if (numberString == null || string.IsNullOrEmpty(numberString))
                    {
                        return newCode + "1";
                    }
                    else
                    {
                        //reverse numberString
                        char[] charArray = numberString.ToCharArray();
                        Array.Reverse(charArray);
                        numberString = new string(charArray);
                        //lấy phần số + 1
                        int number = Int32.Parse(numberString);
                        number++;
                        string resNumberString = number.ToString();
                        return newCode.Substring(0, newCode.Length - numberString.Length) + resNumberString;
                    }

                }
                else
                {
                    return "";
                }
            }
        }
        
        #endregion
    }
}
