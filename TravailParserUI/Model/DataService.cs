using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace TravailParserUI.Model
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataService : IDataService
    {
        private static readonly Regex _toReplace = new Regex(@"(?: (?:[\w]{1,2}|)[\d]{3} {1,2}[\d]{1,2}(?: {1,3}\d{1,2} |))|(?:\d{1,2}[Y]\d{2}\D\d{2})");
        private readonly string[] _newLines = new string[]{"0   0   500 720 501 32  ", "0   0   500 712 501 32  " };

        public void GetData(Action<DataTable, Exception> callback, string filePath)
        {
            // Use this to connect to the actual data service
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                string line;

                using (var streamReader = new StreamReader(fileStream))
                {
                    line = streamReader.ReadToEnd();
                }

                foreach (var newLine in _newLines)
                {
                    line = line.Replace(newLine, Environment.NewLine);
                }

                line = _toReplace.Replace(line, "");

                var index = line.IndexOf(">", StringComparison.Ordinal);

                if (index > 0)
                {
                    line = line.Remove(0, index);
                }

                var split = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var dataFromTrv = string.Join(" ", split);

                var lines = dataFromTrv.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var table = new DataTable();

                table.Columns.Add("Nazwa");
                table.Columns.Add("Work start");
                table.Columns.Add("Work end");
                table.Columns.Add("Duration");
                table.Columns.Add("Preparaton duartion");
                table.Columns.Add("Stop duration");
                table.Columns.Add("Work duration");
                table.Columns.Add("Width");
                table.Columns.Add("Length");

                foreach (var s in lines)
                {
                    var fields = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var newFields = fields.Take(9).ToArray();

                    var newRow = table.Rows.Add();
                    newRow.ItemArray = newFields;
                }
                callback(table, null);
            }
            catch (Exception e)
            {
                callback(null, e);
            }
        }

        public void SaveData(DataTable trvs, string savePath)
        {
            var saveFile = new FileInfo(savePath);

            using (var package = new ExcelPackage(saveFile))
            {
                var worksheet = package.Workbook.Worksheets.Add("Work time");

                worksheet.Cells[1, 1].LoadFromDataTable(trvs, true);
                package.Save();
            }
        }
    }
}