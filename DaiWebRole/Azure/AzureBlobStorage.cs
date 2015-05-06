using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole
{
    public class AzureBlobStorage
    {

        public const string CONTAINER_FOTO = "fotos";
        private const string CONNECTION_STRING = "StorageConString";
        
        public CloudBlobContainer GetCloudBlobContainer(string container)
        {
            // Conectar a conta do storage
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(CONNECTION_STRING));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            
            // Obter a referência do container "fotos"
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);
            
            // Cria se o container "fotos" ainda não existe
            if (blobContainer.CreateIfNotExists())
            {
                // Adicionar permissão para a leitura/acesso público
                blobContainer.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }

            return blobContainer;
        }

        

    }
}
