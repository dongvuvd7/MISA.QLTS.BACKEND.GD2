using MISA.QLTS.Core.AttributeCustom;
using MISA.QLTS.Core.Exceptions;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using System.Reflection;

namespace MISA.QLTS.Core.Services
{
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Constructor
        IBaseRepository<MISAEntity> _baseRepository;
        List<string> errorMsgs = new List<string>(); //List chứa các lỗi
        #endregion

        #region Methods
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <summary>
        /// Validate dữ liệu khi thêm mới bản ghi, nếu pass validate thì tiến hành thêm mới
        /// </summary>
        /// <param name="entity">Bản ghi muốn thêm</param>
        /// <returns>Số bản ghi thêm được</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public int InsertService(MISAEntity entity)
        {
            //Tạo mới khóa chính
            var props = typeof(MISAEntity).GetProperties();
            foreach (var prop in props)
            {
                var isPrimaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                var propType = prop.PropertyType;
                if (isPrimaryKey && propType == typeof(Guid))
                {
                    prop.SetValue(entity, Guid.NewGuid());
                }
            }

            //Validate
            var isValid = ValidateObject(entity); //Validate chung
            if (isValid)
            {
                isValid = ValidateCustom(entity); //Nếu pass validate chung thì validate riêng
            }
            //Insert
            if (isValid)
            {
                var res = _baseRepository.Insert(entity);
                return res;
            }
            return 0;
        }

        /// <summary>
        /// Validate dữ liệu khi cập nhật bản ghi, nếu pass validate thì tiến hành cập nhật
        /// </summary>
        /// <param name="entity">Bản ghi muốn sửa</param>
        /// <param name="entityId">Id bản ghi muốn sửa</param>
        /// <returns>Số bản ghi đã sửa</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public int UpdateService(MISAEntity entity, Guid entityId)
        {
            //Validate
            var isValid = ValidateObject(entity); //Validate chung
            if (isValid)
            {
                isValid = ValidateCustom(entity); //Nếu pass validate chung thì validate riêng
            }
            if (isValid)
            {
                var res = _baseRepository.Update(entity, entityId);
                return res;
            }
            return 0;
        }

        /// <summary>
        /// Validate chung cho các đối tượng khi thêm, sửa
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <returns>True: hợp lệ, False: không hợp lệ</returns>
        /// <exception cref="MISAValidateException"></exception>
        /// CreatedBy: VDDong (16/06/2022)
        private bool ValidateObject(MISAEntity entity)
        {
            var isValid = true;

            //Lấy ra khóa chính
            var primaryKey = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKey))).FirstOrDefault();
            var keyValue = primaryKey.GetValue(entity);

            //Check bắt buộc nhập
            var propsNotEmpty = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
            foreach (var prop in propsNotEmpty)
            {
                var name = GetPropNameDisplay(prop);
                var propValue = prop.GetValue(entity);
                if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                {
                    errorMsgs.Add($"{name} {Properties.Resources.RequiredValidate}.");
                }
            }

            //Check trùng lặp
            var propsNotDuplicate = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
            foreach (var prop in propsNotDuplicate)
            {
                var name = GetPropNameDisplay(prop);
                var propValue = prop.GetValue(entity);
                //truy cập db kiểm tra trùng lặp
                var isDuplicate = _baseRepository.CheckDuplicate(prop.Name, primaryKey.Name, propValue, keyValue);
                if (isDuplicate)
                {
                    errorMsgs.Add($"{name} {Properties.Resources.NotDuplicateValidate}.");
                }
            }

            //Kiểm tra ký tự vượt quá độ dài cho phép
            var propsMaxLength = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(MaxLength)));
            foreach (var prop in propsMaxLength)
            {
                var name = GetPropNameDisplay(prop);
                var propValue = prop.GetValue(entity);
                var maxLength = prop.GetCustomAttribute<MaxLength>().Length;
                if (propValue != null && propValue.ToString().Length > maxLength)
                {
                    errorMsgs.Add($"{name} {Properties.Resources.MaxLengthValidate} là {maxLength}");
                }
            }

            //Thông báo lỗi nếu có lỗi
            if (errorMsgs.Count > 0)
            {
                isValid = false;
                var result = new
                {
                    userMsg = Properties.Resources.ValidateErrorMsg,
                    errorMsgs = errorMsgs
                };
                throw new MISAValidateException(result);
            }

            return isValid;
        }

        /// <summary>
        /// Hàm validate riêng cho các loại đối tượng
        /// </summary>
        /// <param name="entity">Bản ghi muốn validate</param>
        /// <returns></returns>
        /// CreatedBy: VDDong (16/06/2022)
        protected virtual bool ValidateCustom(MISAEntity entity)
        {
            return true;
            //Coding in here
        }

        /// <summary>
        /// Lấy tên hiển thị của property
        /// </summary>
        /// <param name="prop">Property</param>
        /// <returns>Tên hiển thị của property</returns>
        /// CreatedBy: VDDong (16/06/2022)
        private string GetPropNameDisplay(PropertyInfo prop)
        {
            var propNameDisplay = prop.GetCustomAttributes(typeof(PropertyName), true);
            var name = prop.Name;
            if (propNameDisplay.Length > 0)
            {
                name = (propNameDisplay[0] as PropertyName).Name;
            }
            return name;
        }

        /// <summary>
        /// Thêm hàng loạt các bản ghi lên database
        /// </summary>
        /// <param name="entities">Danh sách các bản ghi (đã pass validate required và check trùng mã)</param>
        /// <returns>Số bản ghi thêm được</returns>
        /// Created by: VDDong (06/07/2022)
        public object MultipleInsertService(List<MISAEntity> entities)
        {
            //List lưu trữ các bản ghi hợp lệ
            List<MISAEntity> entitiesValid = new List<MISAEntity>();
            //List lưu trữ mã các bản ghi hợp lệ gửi về client
            List<string> successRecordsCode = new List<string>();

            //Duyệt từng bản ghi trong List entities
            foreach (var entity in entities)
            {
                //Tạo mới khóa chính
                var props = typeof(MISAEntity).GetProperties();
                foreach (var prop in props)
                {
                    var isPrimaryKey = Attribute.IsDefined(prop, typeof(PrimaryKey));
                    var propType = prop.PropertyType;
                    if (isPrimaryKey && propType == typeof(Guid))
                    {
                        prop.SetValue(entity, Guid.NewGuid());
                    }
                }
                ///Validate
                var isValid = true;
                //Lấy ra khóa chính
                var primaryKey = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKey))).FirstOrDefault();
                var keyValue = primaryKey.GetValue(entity);

                //Check bắt buộc nhập
                var propsNotEmpty = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotEmpty)));
                foreach (var prop in propsNotEmpty)
                {
                    var name = GetPropNameDisplay(prop);
                    var propValue = prop.GetValue(entity);
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                    {
                        isValid = false;
                        //Lấy mã tài sản
                        var assetCode = entity.GetType().GetProperty("AssetCode").GetValue(entity);
                        errorMsgs.Add($"{assetCode}: {name} {Properties.Resources.RequiredValidate}.");
                    }
                }

                //Check trùng lặp
                var propsNotDuplicate = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(NotDuplicate)));
                foreach (var prop in propsNotDuplicate)
                {
                    var name = GetPropNameDisplay(prop);
                    var propValue = prop.GetValue(entity);
                    //truy cập db kiểm tra trùng lặp
                    var isDuplicate = _baseRepository.CheckDuplicate(prop.Name, primaryKey.Name, propValue, keyValue);
                    if (isDuplicate)
                    {
                        isValid = false;
                        //Lấy mã tài sản
                        var assetCode = entity.GetType().GetProperty("AssetCode").GetValue(entity);
                        errorMsgs.Add($"{assetCode}: {name} {Properties.Resources.NotDuplicateValidate}.");
                    }
                }

                //Kiểm tra ký tự vượt quá độ dài cho phép
                var propsMaxLength = entity.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(MaxLength)));
                foreach (var prop in propsMaxLength)
                {
                    var name = GetPropNameDisplay(prop);
                    var propValue = prop.GetValue(entity);
                    var maxLength = prop.GetCustomAttribute<MaxLength>().Length;
                    if (propValue != null && propValue.ToString().Length > maxLength)
                    {
                        errorMsgs.Add($"{name} {Properties.Resources.MaxLengthValidate} là {maxLength}");
                    }
                }

                //Nếu bản ghi hợp lệ thì thêm vào danh sách bản ghi hợp lệ
                if (isValid)
                {
                    entitiesValid.Add(entity);
                    successRecordsCode.Add(entity.GetType().GetProperty("AssetCode").GetValue(entity).ToString());
                }
            }
            //Thực hiện thêm hàng loạt các bản ghi vào database
            var recordsInserted = _baseRepository.MultipleInsert(entitiesValid);

            return new
            {
                recordsInserted = recordsInserted,
                successRecordsCode = successRecordsCode,
                errorMsgs = errorMsgs
            };
        }

        #endregion
    }
}
