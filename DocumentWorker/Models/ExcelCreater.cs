using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentWorker.Models
{
    public class ExcelCreater
    {
        public void Excel(
            string savePath, 
            string fileFromList,
            string cityName,
            string legalEntity,
            string legalName,
            string phoneNumberSalesDepartment,
            string phoneNumberDeliveryService,
            string sellersRepresentative)
        {
            var uri = new Uri("pack://application:,,,/TemplateXlsxFile/Base.xlsx");
            var resourceStream = Application.GetResourceStream(uri).Stream;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(resourceStream))
            {
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets["Sheet"];

                byte[] bin = File.ReadAllBytes($"{fileFromList}");
                using (MemoryStream stream = new MemoryStream(bin))
                using (ExcelPackage newExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws2 = newExcelPackage.Workbook.Worksheets[0];

                    //Первый экземпляр
                    //Юридическое лицо
                    ws.Cells["I2"].Value = $"{legalEntity} - {legalName}";

                    //Номер и дата заказа
                    ws.Cells["F4"].Value = $"{ws2.Cells["H4"].Value} г.{cityName}";

                    //Тел. Отдел продаж
                    ws.Cells["Z3"].Value = $"{phoneNumberSalesDepartment}";

                    //Тел. Служба доставки
                    ws.Cells["Z6"].Value = $"{phoneNumberDeliveryService}";

                    //Адрес:
                    ws.Cells["F8"].Value = $"{ws2.Cells["F8"].Value}";

                    //Заказчик:
                    ws.Cells["F10"].Value = $"{ws2.Cells["F11"].Value}";

                    //Телефоны:
                    ws.Cells["Q9"].Value = $"{ws2.Cells["Q10"].Value}";

                    //Доставка производится:     дата:
                    ws.Cells["K12"].Value = $"{ws2.Cells["L13"].Value}";

                    //время:
                    ws.Cells["W12"].Value = $"{ws2.Cells["X13"].Value}";

                    //Примечания:
                    ws.Cells["F14"].Value = $"{ws2.Cells["F15"].Value}";

                    //
                    //
                    //
                    //
                    //
                    //
                    //
                    //


                    Regex regex = new Regex(@"\d{5}");
                    Match match = regex.Match($"{ws2.Cells["H4"].Value}");

                    FileInfo fi = new FileInfo($"{savePath}/Base{match.Value}.xlsx");
                    excelPackage.SaveAs(fi);
                }
            }
        }
    }
}
