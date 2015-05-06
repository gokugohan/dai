using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole.Azure
{
    public class AzureTableStorage
    {
        public const string TABLE_REFERENCE_NAME = "dai";
        private const string CONNECTION_STRING = "StorageConString";
        private static CloudTable table;
        private static TableBatchOperation batchOperation;        
        private CloudTableClient tableClient;

        public AzureTableStorage()
        {            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(CONNECTION_STRING));            
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(TABLE_REFERENCE_NAME);
            table.CreateIfNotExists();
            batchOperation = new TableBatchOperation();            
        }

       

        public void InsertToTable(Log log)
        {
            if(batchOperation!=null)batchOperation.Insert(log);
            if (table != null) table.ExecuteBatch(batchOperation);
            //tableOperation = TableOperation.Insert(log);
            //table.Execute(tableOperation);
        }

        public List<Log> GetTableContents()
        {
            List<Log> logs = new List<Log>();
            TableQuery<Log> query = new TableQuery<Log>();            
            foreach (Log log in table.ExecuteQuery(query))
            {
                logs.Add(log);
            }
            return logs;
        }

    }
}
