using System;
using System.Data;

namespace TravailParserUI.Model
{
    public interface IDataService
    {
        void GetData(Action<DataTable, Exception> callback, string filePath);
        void SaveData(DataTable trvs, string savePath);
    }
}
