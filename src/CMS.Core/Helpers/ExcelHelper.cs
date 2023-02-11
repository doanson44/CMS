using CMS.Core.Constants;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace CMS.Core.Helpers
{
    public static class ExcelHelper
    {
        public static Dictionary<string, int> GetHeader(ExcelWorksheet workSheet, int rowIndex)
        {
            var header = new Dictionary<string, int>();

            if (workSheet != null)
            {
                for (var columnIndex = workSheet.Dimension.Start.Column; columnIndex <= workSheet.Dimension.End.Column; columnIndex++)
                {
                    if (workSheet.Cells[rowIndex, columnIndex].Value != null)
                    {
                        var columnName = workSheet.Cells[rowIndex, columnIndex].Value.ToString();
                        if (!header.ContainsKey(columnName) && !string.IsNullOrEmpty(columnName))
                        {
                            header.Add(columnName, columnIndex);
                        }
                    }
                }
            }
            return header;
        }

        public static string GetColumn(ExcelWorksheet workSheet, Dictionary<string, int> header, int rowIndex, string columnName, string type = "")
        {
            var value = string.Empty;

            var columnIndex = header.ContainsKey(columnName) ? header[columnName] : (int?)null;

            if (workSheet != null && columnIndex != null && workSheet.Cells[rowIndex, columnIndex.Value].Value != null)
            {
                value = workSheet.Cells[rowIndex, columnIndex.Value].Value.ToString();

                if (type == "datetime")
                {
                    var oaDate = double.Parse(value);
                    value = DateTime.FromOADate(oaDate).ToString("yyyy/MM/dd");
                }

                if (type == "timespan")
                {
                    value = workSheet.Cells[rowIndex, columnIndex.Value].Text;
                }
            }

            return value;
        }

        public static ExcelWorksheet SetColumnHeaders(ExcelWorksheet workSheet, string[] columnHeaders)
        {
            for (var i = 0; i < columnHeaders.Length; i++)
            {
                workSheet.Cells[1, i + 1].Value = columnHeaders[i];
            }

            // workSheet.Cells.AutoFitColumns();
            return workSheet;
        }

        public static ExcelWorksheet SetColumnHeaders(ExcelWorksheet workSheet, List<string> columnHeaders)
        {
            for (var i = 0; i < columnHeaders.Count; i++)
            {
                workSheet.Cells[1, i + 1].Value = columnHeaders[i];
            }

            // workSheet.Cells.AutoFitColumns();
            return workSheet;
        }

        public static ExcelWorksheet WriteWorkSheets(ExcelWorksheet workSheet, List<string> columnHeaders, List<object[]> items, int row = 0, bool isHyperLink = false)
        {
            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < columnHeaders.Count; j++)
                {
                    try
                    {
                        var cell = workSheet.Cells[i + row + 2, j + 1];
                        cell.Value = items[i][j] ?? "";

                        var date = new DateTime();
                        var isDate = DateTime.TryParseExact(cell.Value as string, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                        if (isDate)
                        {
                            cell.Value = date;
                            cell.Style.Numberformat.Format = "dd/MM/yyyy";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            continue;
                        }

                        var isDateTime = DateTime.TryParseExact(cell.Value as string, ApplicationFormat.DateAndHourMinuteFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                        if (isDateTime)
                        {
                            cell.Value = date;
                            cell.Style.Numberformat.Format = ApplicationFormat.DateAndHourMinuteFormat;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }

                        if (isHyperLink && cell.Value.ToString().StartsWith("https"))
                        {
                            cell.Style.Font.UnderLine = true;
                            cell.Style.Font.Color.SetColor(Color.Blue);
                            cell.Hyperlink = new Uri(@cell.Value.ToString());
                            cell.Value = "View Photo";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("WriteWorkSheets at i={0}; at j={1}; error: {2}", i, j, ex.Message);
                    }
                }
            }

            return workSheet;
        }
    }
}
