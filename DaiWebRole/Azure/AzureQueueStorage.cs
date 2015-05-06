using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole
{
    public class AzureQueueStorage
    {
        public const string QUEUE = "queue";
        private const string CONNECTION_STRING = "StorageConString";
        public CloudQueue getCloudQueue(string NomeDaQueue)
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(CONNECTION_STRING));
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference(NomeDaQueue);

            // Create the queue if it doesn't already exist
            if (queue.CreateIfNotExists())
            {
                // Escrever no log
                // Log vai ser uma TableStorage
                /*
                 * Texto para escrever na tableStorage é;
                 * Queue "Nome da queue" criado com sucesso -  com data e hora
                 * 
                 */
            }
            else
            {
                /*
                    Escrever na tableStorage com a mensagem; 
                    O queue "Nome da queue" já existe - com data e hora
                 */
            }
            return queue;
        }


        public CloudQueueClient getCloudQueueClient()
        {
            // Retrieve storage account from connection string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(CONNECTION_STRING));
            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            return queueClient;
        }



        public CloudQueueMessage peek()
        {
            CloudQueue queue = getCloudQueue(QUEUE);
            CloudQueueMessage cloudQueueMessage = queue.PeekMessage();
            
            /*
             * Escrever a hora e a data da consulta do queue e conteúdo para tableStorage
             */

            return cloudQueueMessage;
        }

        public CloudQueueMessage pop()
        {
            CloudQueue queue = getCloudQueue(QUEUE);
            CloudQueueMessage cloudQueueMessage = queue.GetMessage();
            queue.DeleteMessage(cloudQueueMessage);
            /*
             * Escrever a hora e a data da eliminação do queue
             */
            return cloudQueueMessage;
        }
    }
}
