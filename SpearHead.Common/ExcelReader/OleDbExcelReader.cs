using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace SpearHead.Common.ExcelReader
{
    public sealed class OleDbExcelReader : IExcelReader
    {
        private string _filePath
        {
            get; set;
        }

        private string _connectionString
        {
            get
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + _filePath +
                    "';Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

            }
        }

        public async Task<DataTable> ReadFromExcel(string fullFileName)
        {
            _filePath = fullFileName;
            try
            {
                using (OleDbConnection connection = new OleDbConnection(_connectionString))
                {
                    if (connection.State == 0)
                    {
                        connection.Open();
                    }

                    DataTable dataTableSheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (dataTableSheets.Rows.Count == 0)
                    {
                        throw new IndexOutOfRangeException("There are no sheets on  the workbook");
                    }
                    string tableName = dataTableSheets.Rows[0]["TABLE_NAME"].ToString();
                    string query = $"Select  * from [{tableName}]";
                    DataTable dt = new DataTable();
                    using (OleDbDataAdapter ada = new OleDbDataAdapter(query, connection))
                    {
                        ada.Fill(dt);
                    }
                    connection.Close();
                    return await Task.FromResult(dt).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
