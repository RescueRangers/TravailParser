using System;
using System.Data;
using TravailParserUI.Model;

namespace TravailParserUI.Design
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataTable, Exception> callback, string filePath)
        {
            // Use this to create design time data

            var item = new DataTable();
            callback(item, null);
        }

        public void SaveData(DataTable trvs, string savePath)
        {
            throw new NotImplementedException();
        }
    }
}