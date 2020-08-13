using System;
using System.IO;
using System.Text.RegularExpressions;
using FileHelpers;

namespace TravailParser
{
    public class Program
    {
        private static readonly Regex ToReplace = new Regex(@" (?:[\w]{1,2}|)[\d]{3} {1,2}[\d]{1,2}(?: {1,3}\d{1,2} |)");
        private const string NewLine = "0   0   500 720 501 32  ";

        public static TrvCsv[] GFetDataFromTrv(string filePath)
        {
            filePath = filePath?.Trim('"');

            var fileStream = new FileStream(filePath, FileMode.Open);
            string line;

            using (var streamReader = new StreamReader(fileStream))
            {
                line = streamReader.ReadToEnd();
            }

            line = line.Replace(NewLine, Environment.NewLine);
            line = ToReplace.Replace(line, "");
            line = line.Remove(0, line.IndexOf(">", StringComparison.Ordinal));

            
            var split = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var dataFromTrv = string.Join(" ", split);

            var engine = new FileHelperEngine<TrvCsv>();
            var record = engine.ReadString(dataFromTrv);
            return record;
        }
    }
}
