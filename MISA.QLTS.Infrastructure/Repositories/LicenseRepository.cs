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
    public class LicenseRepository : BaseRepository<License>, ILicenseRepository
    {
        #region Constructor
        public LicenseRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #endregion

        /// <summary>
        /// Lấy danh sách chứng từ theo điều kiện tìm kiếm và phân trang
        /// </summary>
        /// <param name="searchText">Từ khoá tìm kiếm</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNumber">Số thứ tự trang</param>
        /// <returns>Danh sách các bản ghi thoả mãn điều kiện</returns>
        /// 
        public object Filter(string? searchText, int? pageSize, int? pageNumber)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //Từ khóa tìm kiếm theo mã chứng từ hoặc ghi chú
                searchText = "%" + searchText + "%";
                //Build query string
                var sqlCommand = "FROM License WHERE (LicenseCode LIKE @searchText OR Note LIKE @searchText)";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@searchText", searchText);

                //Thực thi đếm tổng số bản ghi
                var countCommand = "SELECT COUNT(*) " + sqlCommand;
                var totalRecords = sqlConnection.QueryFirstOrDefault<int>(countCommand, param: dynamicParameters);

                //Thực thi phân trang
                sqlCommand = $"SELECT * " + sqlCommand + $"ORDER BY CreatedDate DESC LIMIT @start, @pageSize";
                dynamicParameters.Add("@start", (pageNumber - 1) * pageSize);
                dynamicParameters.Add("@pageSize", pageSize);
                var resultLicenses = sqlConnection.Query<License>(sqlCommand, param: dynamicParameters);

                return new
                {
                    totalRecords = totalRecords,
                    data = resultLicenses
                };

            }
        }

        /// <summary>
        /// Lấy mã chứng từ mới
        /// </summary>
        /// <returns>Mã chứng từ mới</returns>
        /// 
        public string GetNewCode()
        {
            using(sqlConnection = new MySqlConnection(connectionString))
            {
                //build query
                var sqlCommand = "SELECT l.LicenseCode FROM License l ORDER BY l.CreatedDate DESC LIMIT 1";
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
                        string resNumberString = number.ToString().PadLeft(numberString.Length, '0');
                        return newCode.Substring(0, newCode.Length - numberString.Length) + resNumberString;
                    }

                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Thêm các bản ghi vào bảng LicenseDetail
        /// Khi thêm/sửa license thì nhân tiện update luôn cho LicenseDetail thông qua thuộc tính LicenseDetail được thêm ở Entity
        /// Cách update là xóa đi rồi thêm lại:
        /// B1: Xóa hết các bản ghi trong bảng LicenseDetail mà có LicenseId là license đang sửa
        /// B2: Thêm lại các bản ghi vào bảng LicenseDetail
        /// </summary>
        /// <param name="license">Chứng từ</param>
        /// <returns>Số bản ghi thêm được vào bảng LicenseDetail</returns>
        /// 
        public override int InsertDetail(License license)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var result = 0;

                //Xóa hết các bản ghi LicenseDetail theo licenseId trong bảng LicenseDetail
                var sqlCommand = "DELETE FROM LicenseDetail WHERE LicenseId = @LicenseId";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@LicenseId", license.LicenseId);
                sqlConnection.Execute(sqlCommand, param: dynamicParameters);

                //Thêm lại các bản ghi vào bảng LicenseDetail
                sqlCommand = "";
                for (int i = 0; i < license.LicenseDetail.Length; i++)
                {
                    //LicenseId vẫn đang được lưu từ query trước nên không cần add param licenseId
                    //Mã tài sản
                    var assetId = license.LicenseDetail[i].AssetId;
                    //Chi tiết thông tin [{"giá trị": x, "nguồn nguyên giá": y}]
                    var detail = license.LicenseDetail[i].Detail;
                    //Mã bản ghi LicenseDetail
                    var newId = Guid.NewGuid();

                    sqlCommand += $"INSERT INTO LicenseDetail (LicenseDetailId, LicenseId, AssetId, Detail) VALUES (@newId{i}, @LicenseId, @AssetId{i}, @Detail{i});";
                    dynamicParameters.Add($"@AssetId{i}", assetId);
                    dynamicParameters.Add($"@Detail{i}", detail);
                    dynamicParameters.Add($"@newId{i}", newId);
                }
                //Thực thi
                result = sqlConnection.Execute(sqlCommand, param: dynamicParameters);

                return result;

            }
        }


    }
}
