using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.QLTS.Core.AttributeCustom;
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
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region Constructor
        protected string connectionString;
        protected MySqlConnection sqlConnection;

        public BaseRepository(IConfiguration configuration)
        {
            //connectionString = configuration.GetConnectionString("MISA_QLTS_DBFORCE");
            //connectionString = configuration.GetConnectionString("MISA_QLTS_GD1_DBFORCE");
            //connectionString = configuration.GetConnectionString("MISA_QLTS_GD1_DBFORCE_BACKUP");
            //connectionString = configuration.GetConnectionString("MISA_QLTS_GD1_LOCAL");
            connectionString = configuration.GetConnectionString("MISA_QLTS_LOCAL");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Kiểm tra trùng mã
        /// </summary>
        /// <param name="propName">Trường muốn kiểm tra trùng</param>
        /// <param name="primaryKey">Khóa chính</param>
        /// <param name="propValue">Giá trị trường kiểm tra trùng</param>
        /// <param name="keyValue">Giá trị khóa chính</param>
        /// <returns>true - Mã đã tồn tại; false - Mã chưa tồn tại</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public bool CheckDuplicate(string propName, string primaryKey, object propValue, object keyValue)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                ////Build query
                //var tableName = typeof(MISAEntity).Name;
                //var sqlCommand = $"SELECT {propName} FROM {tableName} WHERE {propName} = @{propName} AND {primaryKey} != @{primaryKey}";
                //DynamicParameters dynamicParameters = new DynamicParameters();
                //dynamicParameters.Add($"@{propName}", propValue);
                //dynamicParameters.Add($"@{primaryKey}", keyValue);

                ////Check if duplicate return true else return false
                //var res = sqlConnection.QueryFirstOrDefault<object>(sqlCommand, param: dynamicParameters);
                //if (res != null) return true;
                //else return false;

                //using procedure
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("propName", propName);
                dynamicParameters.Add("primaryKey", primaryKey);
                dynamicParameters.Add("tableName", typeof(MISAEntity).Name);
                dynamicParameters.Add("propValue", propValue);
                dynamicParameters.Add("keyValue", keyValue);
                var sqlCommand = "Proc_CheckDuplicate";
                var res = sqlConnection.QueryFirstOrDefault<string>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                if (res != null) return true;
                else return false;
            }
        }

        /// <summary>
        /// Xoá bản ghi có id là entityId
        /// </summary>
        /// <param name="entityIds">Chuỗi các id muốn xóa</param>
        /// <returns>Số bản ghi đã xoá thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public int Delete(string? entityIds)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //Build query
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();
                var tableName = typeof(MISAEntity).Name; //Tên bảng
                var listId = entityIds.Split(","); //Tách chuỗi ids client gửi lên thành từng id muốn xóa
                var sqlCommand = $"DELETE FROM {tableName} WHERE {tableName}Id IN (";
                DynamicParameters dynamicParameters = new DynamicParameters();
                for (int i = 0; i < listId.Length; i++) //Có bao nhiêu id muốn xóa tạo thành bấy nhiều cái param
                {
                    sqlCommand += $"@{tableName}Id{i},";
                    dynamicParameters.Add($"@{tableName}Id{i}", Guid.Parse(listId[i])); //Add param vào câu query
                }
                sqlCommand = sqlCommand.Substring(0, sqlCommand.Length - 1); //Xóa dấu , thừ ở cuối câu
                sqlCommand += ")";

                //Execute delete
                var res = sqlConnection.Execute(sqlCommand, param: dynamicParameters, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return res;
                
            }
        }

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public IEnumerable<MISAEntity> Get()
        {
            using(sqlConnection = new MySqlConnection(connectionString))
            {

                //using procedure
                var tableName = typeof(MISAEntity).Name;
                var sqlCommand = $"Proc_GetAll";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("tableName", tableName);
                var entities = sqlConnection.Query<MISAEntity>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return entities;

            }
        }

        /// <summary>
        /// Lấy ra 1 bản ghi theo id
        /// </summary>
        /// <param name="entityId">Id bản ghi muốn lấy</param>
        /// <returns>Bản ghi theo id đã nhập</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public MISAEntity Get(Guid entityId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {

                //using procedure
                var tableName = typeof(MISAEntity).Name;
                var sqlCommand = $"Proc_GetById";
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("tableName", tableName);
                dynamicParameters.Add("entityId", entityId);
                var entity = sqlConnection.QueryFirstOrDefault<MISAEntity>(sqlCommand, param: dynamicParameters, commandType: CommandType.StoredProcedure);
                return entity;
            }
        }

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="entity">Bản ghi muốn thêm mới</param>
        /// <returns>Số bản ghi đã thêm thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public int Insert(MISAEntity entity)
        {
            using(sqlConnection = new MySqlConnection(connectionString))
            {

                ///Build query
                //Khai báo các biến tên bảng, danh sách các trường, danh sách các param
                var tableName = typeof(MISAEntity).Name;
                var listColumnsName = string.Empty;
                var listColumnsParam = string.Empty;

                DynamicParameters dynamicParameters = new DynamicParameters();
                //Duyệt từng thuộc tính của đối tượng
                var props = typeof(MISAEntity).GetProperties();
                foreach (var prop in props)
                {
                    //Nếu thuộc tính là NotMap thì loại không đưa vào câu truy vấn để lên database
                    var notMapProps = prop.GetCustomAttributes(typeof(NotMap), true);
                    if(notMapProps.Length == 0)
                    {
                        var name = prop.Name; //Tên của thuộc tính
                        var value = prop.GetValue(entity); //Giá trị của thuộc tính
                        listColumnsName += $"{name},"; //Chuỗi danh sách trường
                        listColumnsParam += $"@{name},"; //Chuỗi danh sách các giá trị tương ứng danh sách các trường
                        dynamicParameters.Add($"@{name}", value);//Đưa giá trị tương ứng vào param
                    }
                }

                //Gán giá trị CreatedDate có giá trị là DateTime hiện tại
                //listColumnsName += "CreatedDate";
                //listColumnsParam += "@CreatedDate";
                dynamicParameters.Add("@CreatedDate", DateTime.Now);

                //Xóa dấu , thừa ở cuối câu
                listColumnsName = listColumnsName.Substring(0, listColumnsName.Length - 1);
                listColumnsParam = listColumnsParam.Substring(0, listColumnsParam.Length - 1);

                //Build and Run query
                var sqlCommand = $"INSERT INTO {tableName}({listColumnsName}) VALUES ({listColumnsParam})";
                var rowsInserted = sqlConnection.Execute(sqlCommand, param: dynamicParameters);

                InsertDetail(entity);

                return rowsInserted;


                //using procedure
                //sqlConnection.Open();
                //var sqlTransaction = sqlConnection.BeginTransaction();
                //var tableName = typeof(MISAEntity).Name;
                //var sqlCommand = $"Proc_Insert{tableName}";
                //var rowsInserted = sqlConnection.Execute(sqlCommand, entity, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                //sqlTransaction.Commit();
                //return rowsInserted;

            }
        }

        /// <summary>
        /// Cập nhật một bản ghi theo id
        /// </summary>
        /// <param name="entity">Bản ghi muốn sửa</param>
        /// <param name="entityId">Id bản ghi muốn sửa</param>
        /// <returns>Số bản ghi cập nhật thành công</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public int Update(MISAEntity entity, Guid entityId)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                //Khai báo các biến tên bảng, danh sách các trường, danh sách param
                var tableName = typeof(MISAEntity).Name;
                var listUpdate = string.Empty;

                DynamicParameters dynamicParameters = new DynamicParameters();

                var props = typeof(MISAEntity).GetProperties();
                //Duyệt từng thuộc tính của đối tượng
                foreach (var prop in props)
                {
                    //Nếu thuộc tính là NotMap thì loại không đưa vào câu truy vấn để lên database
                    var notMapProps = prop.GetCustomAttributes(typeof(NotMap), true);
                    if (notMapProps.Length == 0)
                    {
                        var name = prop.Name; //Tên của thuộc tính
                        var value = prop.GetValue(entity); //Giá trị của thuộc tính
                        listUpdate += $"{name} = @{name},";
                        dynamicParameters.Add($"@{name}", value); //Đưa giá trị tương ứng vào
                    }
                }

                //Xóa dấu , thừa ở cuối câu
                listUpdate = listUpdate.Substring(0, listUpdate.Length - 1);

                //Build and Run query
                var sqlCommand = $"UPDATE {tableName} SET {listUpdate} WHERE {tableName}Id = @{tableName}Id";
                dynamicParameters.Add($"@{tableName}Id", entityId);
                var rowsUpdated = sqlConnection.Execute(sqlCommand, param: dynamicParameters);

                InsertDetail(entity);

                return rowsUpdated;


                //using procedure
                //sqlConnection.Open();
                //var sqlTransaction = sqlConnection.BeginTransaction();
                //var tableName = typeof(MISAEntity).Name;
                //var sqlCommand = $"Proc_Update{tableName}";
                //var rowsUpdated = sqlConnection.Execute(sqlCommand, entity, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                //sqlTransaction.Commit();
                //return rowsUpdated;

            }
        }

        /// <summary>
        /// Thêm các bản ghi vào bảng detail
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int InsertDetail(MISAEntity entity)
        {
            return 0;
        }

        /// <summary>
        /// Thêm hàng loạt các bản ghi lên database
        /// </summary>
        /// <param name="entities">Danh sách các bản ghi (đã pass validate required và check trùng mã)</param>
        /// <returns>Số bản ghi thêm được</returns>
        /// Created by: VDDong (06/07/2022)
        public int MultipleInsert(List<MISAEntity> entities)
        {
            using (sqlConnection = new MySqlConnection(connectionString))
            {
                var rowsInserted = 0;
                var tableName = typeof(MISAEntity).Name;
                var sqlCommand = $"Proc_Insert{tableName}";
                sqlConnection.Open();
                var sqlTransaction = sqlConnection.BeginTransaction();
                foreach(var entity in entities)
                {
                    rowsInserted += sqlConnection.Execute(sqlCommand, entity, commandType: CommandType.StoredProcedure, transaction: sqlTransaction);
                }
                sqlTransaction.Commit();
                return rowsInserted;
            }
        }

        #endregion
    }
}
