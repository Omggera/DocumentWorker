using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentWorker.Models
{
    public class ExcelPriceCreater
    {
        public void CreatePrice(string? file, string? path, IProgress<int> progress)
        {
            progress.Report(10);

            byte[] bin = File.ReadAllBytes(file);
            using (MemoryStream stream = new MemoryStream(bin))
            {
                progress.Report(20);

                using (var workbook = new XLWorkbook(stream))
                {
                    var ws = workbook.Worksheet(1);

                    progress.Report(40);

                    for (int a = 17; a > 9; a--)
                    {
                        ws.Column(a).Delete();
                    }

                    progress.Report(60);

                    int maxRow = ws.LastRowUsed().RowNumber();

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

                        ws.Cell($"I{b}").Value = ws.Cell($"I{b}").Value;
                    }
                    
                    progress.Report(80);

                    DateTime dateTime = DateTime.Now;
                    string date = dateTime.ToString("d");

                    FileInfo fi = new FileInfo($"{path}/Прайс Владимир на {date}.xlsx");
                    workbook.SaveAs(fi.ToString());

                    progress.Report(100);
                }
                stream.Close();
                stream.Dispose();
            }
        }

        private int GetCellInt(IXLWorksheet worksheet, string col, int row)
        {
            var value = worksheet.Cell($"{col}{row}").GetValue<int>();
            return value;
        }

        /// <summary>
        /// Увеличивает наценку путем сложения 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="tradeMargin"></param>
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
        /// <param name="row"></param>
        /// <param name="tradeMargin"></param>
        /// <returns></returns>
        private string ExcelFormula(int row, string tradeMargin)
        {
            var formula = $"=CEILING(IF(G{row}>((H{row})*{tradeMargin}),G{row},H{row}*{tradeMargin}),10)";
            return formula;
        }
    }
}
