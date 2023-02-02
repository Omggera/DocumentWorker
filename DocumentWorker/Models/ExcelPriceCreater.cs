using ClosedXML.Excel;
using System;
using System.IO;

namespace DocumentWorker.Models
{
    public class ExcelPriceCreater
    {
        /// <summary>
        /// Набирает прайс-лист. Удаляет ненужные столбцы,
        /// вычисляет итоговые цены по формуле, после чего заменяет формулы 
        /// их значениями
        /// </summary>
        /// <param name="file">Абсолютный путь к файлу прайс-листа</param>
        /// <param name="path">Абсолютный путь к папке, куда сохранится созданный файл </param>
        /// <param name="progress">Прогресс выполнения задачи</param>
        public void CreatePrice(string? file, string? path, IProgress<int> progress)
        {
            progress.Report(10);

            //Загружаем файл
            byte[] bin = File.ReadAllBytes(file);
            using (MemoryStream stream = new MemoryStream(bin))
            {
                progress.Report(20);

                //Открываем файл
                using (var workbook = new XLWorkbook(stream))
                {
                    //Работаем с первым листом
                    var ws = workbook.Worksheet(1);

                    progress.Report(40);

                    //Удалаяем ненужные столбцы
                    for (int a = 17; a > 9; a--)
                    {
                        ws.Column(a).Delete();
                    }

                    progress.Report(60);

                    //Получаем последнюю используемую строку, чтобы узнать
                    //общее количество строк
                    int maxRow = ws.LastRowUsed().RowNumber();

                    //Проходим по всем ячейкам в столбце с итоговой ценой
                    //и устанавливаем туда формулу для расчета
                    for (int b = 2; b < (maxRow + 1); b++)
                    {
                        if (GetCellInt(ws, "H", b) < 200)
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 20);

                        else if ((GetCellInt(ws, "H", b) > 200) & (GetCellInt(ws, "H", b) < 2000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, "1.1");

                        else if ((GetCellInt(ws, "H", b) > 2000) & (GetCellInt(ws, "H", b) < 5000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 250);

                        else if ((GetCellInt(ws, "H", b) > 5000) & (GetCellInt(ws, "H", b) < 20000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 600);

                        else if ((GetCellInt(ws, "H", b) > 20000) & (GetCellInt(ws, "H", b) < 30000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 1100);

                        else if ((GetCellInt(ws, "H", b) > 30000) & (GetCellInt(ws, "H", b) < 40000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 1600);

                        else if ((GetCellInt(ws, "H", b) > 40000) & (GetCellInt(ws, "H", b) < 50000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 2100);

                        else if ((GetCellInt(ws, "H", b) > 50000) & (GetCellInt(ws, "H", b) < 70000))
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 2600);

                        else if (GetCellInt(ws, "H", b) > 70000)
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, 3100);

                        if ((ws.Cell($"C{b}").GetString()).Contains("Мебель") == true)
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, "1.1");

                        else if((ws.Cell($"C{b}").GetString()).Contains("Отделка") == true)
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, "1.2");

                        else if ((ws.Cell($"C{b}").GetString()).Contains("БХИГ") == true)
                            ws.Cell($"I{b}").FormulaA1 = ExcelFormula(b, "1.15");

                        //Заменяем формулу её значением
                        ws.Cell($"I{b}").Value = ws.Cell($"I{b}").Value;
                    }
                    
                    progress.Report(80);

                    //Получаем сегодняшнюю дату
                    DateTime dateTime = DateTime.Now;
                    string date = dateTime.ToString("d");

                    //Сохраняем получившийся файл
                    FileInfo fi = new FileInfo($"{path}/Прайс Владимир на {date}.xlsx");
                    workbook.SaveAs(fi.ToString());

                    progress.Report(100);
                }

                //Хотя я и использовал using, но без этих инструкций
                //программа не освобождала используемые ресурсы оперативной памяти
                stream.Close();
                stream.Dispose();
            }
        }

        /// <summary>
        /// Получает Int32 значение из ячейки
        /// </summary>
        /// <param name="worksheet">Рабочая книга</param>
        /// <param name="col">Столбец</param>
        /// <param name="row">Строка</param>
        /// <returns></returns>
        private int GetCellInt(IXLWorksheet worksheet, string col, int row)
        {
            var value = worksheet.Cell($"{col}{row}").GetValue<int>();
            return value;
        }

        /// <summary>
        /// Увеличивает наценку путем сложения 
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="tradeMargin">Наценка</param>
        /// <returns></returns>
        private string ExcelFormula(int row, int tradeMargin)
        {
            var formula = $"=CEILING(IF(G{row}>((H{row})+{tradeMargin}),G{row},H{row}+{tradeMargin}),10)";
            return formula;
        }

        /// <summary>
        /// Реализует процентное увеличение наценки - 
        /// умножает на определенный процент. Для корректного отображения
        /// в формуле, процент надо вводить в виде строки
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="tradeMargin">Наценка</param>
        /// <returns></returns>
        private string ExcelFormula(int row, string tradeMargin)
        {
            var formula = $"=CEILING(IF(G{row}>((H{row})*{tradeMargin}),G{row},H{row}*{tradeMargin}),10)";
            return formula;
        }
    }
}
