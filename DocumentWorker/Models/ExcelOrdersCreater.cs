using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace DocumentWorker.Models
{
    public class ExcelOrdersCreater
    {
        /// <summary>
        /// Создает новый Excel документ нужного формата 
        /// с данными загуженного excel файла
        /// </summary>
        /// <param name="savePath">Путь к папке для сохранения созданного файла(ов)</param>
        /// <param name="filesList">Список с абсолютным путем к файлам</param>
        /// <param name="cityName">Название города</param>
        /// <param name="legalEntity">Юридическое лицо</param>
        /// <param name="legalName">Название организации на латинице</param>
        /// <param name="phoneNumberSalesDepartment">Телефон отдела продаж</param>
        /// <param name="phoneNumberDeliveryService">Телефон службы доставки</param>
        /// <param name="sellersRepresentative">Представитель продавца</param>
        /// <param name="progress">Прогресс выполнения задачи</param>
        public void CreateNewExcelDocument(
            string savePath,
            ObservableCollection<string>? filesList,
            string cityName,
            string legalEntity,
            string legalName,
            string phoneNumberSalesDepartment,
            string phoneNumberDeliveryService,
            string sellersRepresentative,
            IProgress<int> progress)
        {
            //Путь к шаблонному excel фалу в ресрсах приложения
            var uri = new Uri("pack://application:,,,/TemplateExcelFile/Base.xlsx");
            var resourceStream = Application.GetResourceStream(uri).Stream;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            for (int i = 0; i < filesList.Count; i++)
            {
                //Первый файл "зашитый" в программу, как ресурс
                using (ExcelPackage excelPackage = new ExcelPackage(resourceStream))
                {
                    ExcelWorksheet ws = excelPackage.Workbook.Worksheets["Sheet"];

                    byte[] bin = File.ReadAllBytes($"{filesList[i]}");
                    using (MemoryStream stream = new MemoryStream(bin))

                    //Файл, добавляемый для копирования
                    using (ExcelPackage newExcelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorksheet ws2 = newExcelPackage.Workbook.Worksheets[0];

                        //Первый экземпляр
                        //Юридическое лицо
                        ws.Cells["I2"].Value = $"{legalEntity} - {legalName}";
                        ws.Cells["I56"].Value = $"{legalEntity} - {legalName}";

                        //Номер и дата заказа
                        ws.Cells["F4"].Value = $"{ws2.Cells["H4"].Value} г.{cityName}";
                        ws.Cells["F58"].Value = $"{ws2.Cells["H4"].Value} г.{cityName}";

                        //Тел. Отдел продаж
                        ws.Cells["Z3"].Value = $"{phoneNumberSalesDepartment}";
                        ws.Cells["Z57"].Value = $"{phoneNumberSalesDepartment}";

                        //Тел. Служба доставки
                        ws.Cells["Z6"].Value = $"{phoneNumberDeliveryService}";
                        ws.Cells["Z60"].Value = $"{phoneNumberDeliveryService}";

                        //Адрес:
                        string adress = $"{ws2.Cells["F8"].Value}";

                        //Если строка с адресом не пуста, то ставим адрес,
                        //а если пустая, то просто название города
                        if (adress != string.Empty)
                        {
                            ws.Cells["F8"].Value = $"{ws2.Cells["F8"].Value}";
                            ws.Cells["F62"].Value = $"{ws2.Cells["F8"].Value}";
                        }
                        else
                        {
                            ws.Cells["F8"].Value = $"г.{cityName}";
                            ws.Cells["F62"].Value = $"г.{cityName}";
                        }
                        
                        //Заказчик:
                        ws.Cells["F10"].Value = $"{ws2.Cells["F11"].Value}";
                        ws.Cells["F64"].Value = $"{ws2.Cells["F11"].Value}";

                        //Телефоны:
                        ws.Cells["Q9"].Value = $"{ws2.Cells["Q10"].Value}";
                        ws.Cells["Q63"].Value = $"{ws2.Cells["Q10"].Value}";

                        //Доставка производится:     дата:
                        string dateDelivery = $"{ws2.Cells["L13"].Value}";
                        ws.Cells["K12"].Value = dateDelivery;
                        ws.Cells["K66"].Value = dateDelivery;

                        //время:
                        ws.Cells["W12"].Value = $"{ws2.Cells["X13"].Value}";
                        ws.Cells["W66"].Value = $"{ws2.Cells["X13"].Value}";

                        //Примечания:
                        string notes = $"{ws2.Cells["F15"].Value}";
                        ws.Cells["F14"].Value = notes;
                        ws.Cells["F68"].Value = notes;

                        //Первая позиция в файле из которого мы копируем это C18,
                        //в файле в который мы вставляем значение это C17

                        //Последняя ячейка в файле в который мы вставляем это C30
                        //Всего допустимо 14 различных позиций

                        int nextCell = 0;

                        //Копируем позиции в заказе исходя из их количества
                        for (int x = 18; x < 30; x++)
                        {
                            string xPosition = $"{ws2.Cells[$"C{x}"].Value}";
                            string xNumberPosition = $"{ws2.Cells[$"B{x}"].Value}";

                            if (
                                (xNumberPosition != "") && 
                                (xPosition != null) | (xPosition != "")
                                )
                            {
                                CopyPosition(ws, ws2, x - 1, x);
                                nextCell = x;
                            }
                            else
                            {
                                nextCell = x-1;
                                break;
                            } 
                        }

                        //Вычисляем, если ли слово "дост" в примечаниях
                        Regex deliveryRegex = new Regex(@"дост");
                        Match deliveryMatch = deliveryRegex.Match(notes);

                        //Если есть, то добавляем слово доставка после последней
                        //позиции и ставим стоимость доставки
                        string deliv = "";
                        if (deliveryMatch.Value == "дост")
                        {
                            Regex onlyDeliveryRegex = new Regex(@"дост\s\d\d\d");
                            Match onlyDeliveryMatch = onlyDeliveryRegex.Match(notes);

                            string onlyDeliveryString = onlyDeliveryMatch.Value;

                            Regex deliveryAmountRegex = new Regex(@"\d\d\d+");
                            Match deliveryAmountMatch = deliveryAmountRegex.Match(onlyDeliveryString);

                            ws.Cells[$"C{nextCell}"].Value = $"Доставка";
                            ws.Cells[$"R{nextCell}"].Value = Convert.ToInt32(deliveryAmountMatch.Value);
                            ws.Cells[$"V{nextCell}"].Value = Convert.ToInt32(deliveryAmountMatch.Value);
                            deliv = "д";
                        }

                        //Копируем позиции в заказе на второй экземпляр
                        ws.Cells[17, 3, 30, 26].Copy(ws.Cells[71, 3, 84, 26]);

                        //Представитель продавца
                        ws.Cells["X49"].Value = sellersRepresentative;
                        ws.Cells["X103"].Value = sellersRepresentative;

                        Regex saveRegex = new Regex(@"\d{5}");
                        Match saveMatch = saveRegex.Match($"{ws2.Cells["H4"].Value}");

                        //Если заказ с доставкой, то добавляем к имени 
                        //сохраненного файла букву "д"
                        if (deliv == "д")
                        {
                            FileInfo fi = new FileInfo($"{savePath}/{dateDelivery} {saveMatch.Value}{deliv}.xlsx");
                            excelPackage.SaveAs(fi);
                        }
                        else 
                        {
                            FileInfo fi = new FileInfo($"{savePath}/{dateDelivery} {saveMatch.Value}.xlsx");
                            excelPackage.SaveAs(fi);
                        }

                        //Вычисляем прогресс выполнения
                        var percentComplete = ((i + 1) * 100) / filesList.Count;
                        progress.Report(percentComplete);
                    }
                } 
            }
        }

        /// <summary>
        /// Копирует позиции в заказе
        /// </summary>
        /// <param name="ws">Куда мы копируем</param>
        /// <param name="ws2">Откуда мы копируем</param>
        /// <param name="numWS">Номер ячейки файла, куда копируем</param>
        /// <param name="numWS2">Номер ячейки, откуда копируем</param>
        private void CopyPosition(ExcelWorksheet ws, ExcelWorksheet ws2, int numWS, int numWS2)
        {
            ws.Cells[$"C{numWS}"].Value = $"{ws2.Cells[$"C{numWS2}"].Value}";
            ws.Cells[$"N{numWS}"].Value = Convert.ToInt32(ws2.Cells[$"N{numWS2}"].Value);
            ws.Cells[$"O{numWS}"].Value = Convert.ToInt32(ws2.Cells[$"O{numWS2}"].Value);
            ws.Cells[$"R{numWS}"].Value = Convert.ToInt32(ws2.Cells[$"R{numWS2}"].Value);
            ws.Cells[$"V{numWS}"].Value = Convert.ToInt32(ws2.Cells[$"V{numWS2}"].Value);
            ws.Cells[$"Z{numWS}"].Value = $"{ws2.Cells[$"Z{numWS2}"].Value}";
        }
    }
}
