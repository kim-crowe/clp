using System;
using System.Linq;
using System.Data;
using System.IO;

namespace GorgleDevs.Mvc
{
    public static class DataTableExtensions
    {
        public static DataTable PopulateFromTsv(this DataTable dataTable, StreamReader streamReader)
        {
            var headers = streamReader.ReadLine().Split("\t");
            dataTable.Columns.AddRange(headers.Select(h => new DataColumn(h)).ToArray());

            string line = streamReader.ReadLine();
            while (String.IsNullOrEmpty(line) == false)
            {
                dataTable.Rows.Add(line.Split("\t"));
                line = streamReader.ReadLine();
            }

            return dataTable;
        }
    }
}