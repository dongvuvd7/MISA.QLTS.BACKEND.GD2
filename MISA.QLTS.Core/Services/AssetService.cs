using MISA.QLTS.Core.Entities;
using MISA.QLTS.Core.Interfaces.Repositories;
using MISA.QLTS.Core.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Services
{
    public class AssetService : BaseService<Asset>, IAssetService
    {
        #region Constructor
        IAssetRepository _assetRepository;
        public AssetService(IAssetRepository assetRepository) : base(assetRepository)
        {
            _assetRepository = assetRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Xuất file excel danh sách tài sản
        /// </summary>
        /// <returns>File excel DSTS</returns>
        /// CreatedBy: VDDong (16/06/2022)
        public Stream ExportExcel()
        {
            //Lấy tất cả danh sách tài sản để export ra file excel
            //var list = _assetRepository.Get().ToList<Asset>();
            var list = _assetRepository.GetExport().ToList<Asset>();

            //Khai báo khởi tạo tiêu đề sheet
            var stream = new MemoryStream();
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets.Add("DS Tài Sản");

            //Set độ rộng từng column
            workSheet.Column(1).Width = 5; //STT
            workSheet.Column(2).Width = 15; //Mã
            workSheet.Column(3).Width = 40; //Tên
            workSheet.Column(4).Width = 40; //Tên Loại
            workSheet.Column(5).Width = 40; //Tên Bộ phận
            workSheet.Column(6).Width = 15; //Số lượng
            workSheet.Column(7).Width = 25; //Giá
            workSheet.Column(8).Width = 10; //Số năm sử dụng
            workSheet.Column(9).Width = 15; //Tỉ lệ khấu hao
            workSheet.Column(10).Width = 20; //Ngày mua
            workSheet.Column(11).Width = 20; //Ngày sử dụng

            //dòng đầu tiên - tiêu đề
            using (var range = workSheet.Cells["A1:K1"]) //độ rộng tiêu đề từ cột A1 đến cột K1
            {
                range.Merge = true; //gộp các cột lại (bỏ border ngăn giữa đi)
                range.Value = "DANH SÁCH TÀI SẢN"; //giá trị dòng đó
                range.Style.Font.Bold = true; //set font đậm
                range.Style.Font.Size = 16; //set font size
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dòng
            }

            // style cho excel.
            //gán giá trị cho từng ô[hàng, cột]
            workSheet.Cells[3, 1].Value = Properties.Resources.NumericalOrder;
            workSheet.Cells[3, 2].Value = Properties.Resources.AssetCode;
            workSheet.Cells[3, 3].Value = Properties.Resources.AssetName;
            workSheet.Cells[3, 4].Value = Properties.Resources.AssetCategory;
            workSheet.Cells[3, 5].Value = Properties.Resources.AssetDepartment;
            workSheet.Cells[3, 6].Value = Properties.Resources.AssetQuantity;
            workSheet.Cells[3, 7].Value = Properties.Resources.AssetCost;
            workSheet.Cells[3, 8].Value = "Số năm SD";
            workSheet.Cells[3, 9].Value = Properties.Resources.AssetDepreciationRate;
            workSheet.Cells[3, 10].Value = "Ngày mua";
            workSheet.Cells[3, 11].Value = "Ngày sử dụng";

            //style cho các ô từ A3 đến K3
            using (var range = workSheet.Cells["A3:K3"])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray); //background color
                range.Style.Font.Bold = true; //set font đậm
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin); //border xung quanh
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; //căn giữa dòng
            }

            //Đổ dữ liệu từ list vào
            for (int i = 0; i < list.Count; i++)
            {
                //bắt đầu từ dòng 4 nên (i+4)
                workSheet.Cells[i + 4, 1].Value = i + 1; //STT
                workSheet.Cells[i + 4, 2].Value = list[i].AssetCode;
                workSheet.Cells[i + 4, 3].Value = list[i].AssetName;
                workSheet.Cells[i + 4, 4].Value = list[i].AssetCategoryName;
                workSheet.Cells[i + 4, 5].Value = list[i].DepartmentName;
                workSheet.Cells[i + 4, 6].Value = list[i].Quantity;
                workSheet.Cells[i + 4, 7].Value = list[i].Cost;
                workSheet.Cells[i + 4, 8].Value = list[i].LifeTime;
                workSheet.Cells[i + 4, 9].Value = list[i].DepreciationRate;
                workSheet.Cells[i + 4, 10].Value = list[i].PurchaseDate.ToString("dd/MM/yyyy");
                workSheet.Cells[i + 4, 11].Value = list[i].UseDate.ToString("dd/MM/yyyy");

            }

            //căn giữa cho cột A (STT)
            using (var range = workSheet.Cells["A3:A" + (list.Count + 3)])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            //căn giữa cho cột J và K (ngày tháng)
            using (var range = workSheet.Cells["J4:K" + (list.Count + 3)])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            package.Save();
            stream.Position = 0;
            return package.Stream;
        }

        #endregion
    }
}
