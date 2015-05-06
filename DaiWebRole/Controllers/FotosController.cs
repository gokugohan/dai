using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DaiWebRole.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Diagnostics;
using System.IO;

namespace DaiWebRole.Controllers
{
    public class FotosController : Controller
    {
        private EntityDai db = new EntityDai();

        private AzureBlobStorage storage = new AzureBlobStorage();

        private string[] validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/png"
            };


        // GET: Fotos
        public ActionResult Index()
        {
       
            return View(db.Fotos.ToList());
        }

        [HttpGet]
        public ActionResult Upload()
        {            
            return View(new FotoModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(FotoModel model)
        {
            

            if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (ModelState.IsValid)
            {
               
                try
                {
                    
                    
                    
                    CloudBlobContainer blobContainer = storage.GetCloudBlobContainer(AzureBlobStorage.CONTAINER_FOTO);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(model.ImageUpload.FileName);
                    blob.UploadFromStream(model.ImageUpload.InputStream);

                    Foto foto = new Foto
                    {
                        Nome = model.ImageUpload.FileName,
                        Titulo = model.Titulo,
                        Url = blob.Uri.ToString(),
                        DataCriacao = DateTime.Now
                    };

                    db.Fotos.Add(foto);
                    db.SaveChanges();
                    
                }
                catch (Exception e)
                {
                    Trace.TraceInformation("Error: " + e);
                }
            }
            
            return RedirectToAction("Upload");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var foto = db.Fotos.Find(id);
            if (foto == null)
            {
                return new HttpNotFoundResult();
            }
            var model = new FotoModel
            {
                Titulo = foto.Titulo,
                
            };

            CloudBlobContainer blobContainer = storage.GetCloudBlobContainer(AzureBlobStorage.CONTAINER_FOTO);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(foto.Nome);



            return View(model);
        }


        [HttpPost]        
        public string DeleteFoto(string name)
        {
            Uri uri = new Uri(name);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            CloudBlobContainer blobContainer = storage.GetCloudBlobContainer(AzureBlobStorage.CONTAINER_FOTO);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);
            blob.Delete();
            return filename + " deleted.";
        }

    }
}
