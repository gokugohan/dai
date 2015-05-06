using DaiWebRole.Azure;
using DaiWebRole.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace DaiWebRole.Controllers
{
    [Authorize]
    public class MembroController : Controller
    {
        private AzureBlobStorage blobStorage = new AzureBlobStorage();
        private AzureTableStorage tableStorage = new AzureTableStorage();

        private string[] validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/png"
            };

        // GET: Membro
        public ActionResult Index()
        {
            using (var db = new EntityDai())
            {
                return View(db.Membros.ToList());
            }
        }

        // GET: Membro/Details/5
        public ActionResult Details(int id)
        {
            using (var db = new EntityDai())
            {
                Membro membro = db.Membros.Where(m => m.Id_membro == id).FirstOrDefault();
                return View(membro);
            }
        }

        // GET: Membro/Create
        public ActionResult Novo()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            SelectListItem item = new SelectListItem();

            item.Text = "Masculino";
            item.Value = "Masculino";
            selectList.Add(item);
            item = new SelectListItem();
            item.Text = "Feminino";
            item.Value = "Feminino";
            selectList.Add(item);
            item = new SelectListItem();
            item.Text = "Híbrido";
            item.Value = "Híbrido";
            selectList.Add(item);


            ViewBag.DropdownResult = selectList;
            return View(new MembroViewModel());
        }

        // POST: Membro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Novo(MembroViewModel model)
        {
            if (model.Foto == null || model.Foto.ContentLength == 0)
            {
                ModelState.AddModelError("Foto", "Campo obrigatório");
            }
            else if (!validImageTypes.Contains(model.Foto.ContentType))
            {
                ModelState.AddModelError("Foto", "Inserir apenas imagem do tipo GIF, JPG or PNG se faz favor!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CloudBlobContainer blobContainer = blobStorage.GetCloudBlobContainer(AzureBlobStorage.CONTAINER_FOTO);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(model.Foto.FileName);
                    blob.UploadFromStream(model.Foto.InputStream);

                    using (var db = new EntityDai())
                    {

                        Membro membro = new Membro
                        {
                            PrimeirNome = model.PrimeiroNome,
                            UltimoNome = model.UltimoNome,
                            Genero = model.Genero,
                            Contacto = model.Contacto,
                            Endereco = model.Endereco,
                            foto = blob.Uri.ToString()
                        };
                        db.Membros.Add(membro);

                        Guid partitionKey = Guid.NewGuid();
                        string rowKey = membro.Id_membro.ToString(CultureInfo.InvariantCulture);

                        Log log = new Log(partitionKey.ToString(), rowKey);
                        string descricao = membro.PrimeirNome + " " + membro.UltimoNome + " criado com sucesso (" + DateTime.Now + ")";
                        log.descricao = descricao;

                        tableStorage = new AzureTableStorage();
                        tableStorage.InsertToTable(log);
                        db.SaveChanges();

                        Utilizador u = new Utilizador
                        {
                            UserName = membro.PrimeirNome,
                            Password = "123"
                        };
                        db.Utilizadores.Add(u);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(model);
        }

        // GET: Membro/Edit/5
        public ActionResult Editar(int id)
        {
            return View(new MembroViewModel());
        }

        // POST: Membro/Edit/5
        [HttpPost]
        public ActionResult Editar(int id, MembroViewModel model)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    CloudBlobContainer blobContainer = blobStorage.GetCloudBlobContainer(AzureBlobStorage.CONTAINER_FOTO);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(model.Foto.FileName);
                    blob.UploadFromStream(model.Foto.InputStream);

                    using (var db = new EntityDai())
                    {
                        var membro = (from m in db.Membros
                                      where m.Id_membro == id
                                      select m).FirstOrDefault();

                        if (membro != null)
                        {
                            membro.PrimeirNome = model.PrimeiroNome;
                            membro.UltimoNome = model.UltimoNome;
                            membro.Genero = model.Genero;
                            membro.Contacto = model.Contacto;
                            membro.Endereco = model.Endereco;
                            membro.foto = blob.Uri.ToString();
                        };

                        db.Entry(membro).State = EntityState.Modified;
                        
                        Guid partitionKey = Guid.NewGuid();
                        string rowKey = membro.Id_membro.ToString(CultureInfo.InvariantCulture);

                        Log log = new Log(partitionKey.ToString(), rowKey);
                        string descricao = membro.PrimeirNome + " " + membro.UltimoNome + " criado com sucesso (" + DateTime.Now + ")";
                        log.descricao = descricao;

                        tableStorage = new AzureTableStorage();
                        tableStorage.InsertToTable(log);
                        

                        Utilizador u = new Utilizador
                        {
                            UserName = membro.PrimeirNome,
                            Password = "123"
                        };

                        db.Entry(u).State = EntityState.Modified;

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: Membro/Delete/5
        public ActionResult Eliminar(int id)
        {
            using (var db = new EntityDai())
            {
                Membro membro = (from m in db.Membros
                                where m.Id_membro == id
                                select m).FirstOrDefault();

                Utilizador utilizador = (from u in db.Utilizadores
                                         where u.Id == membro.Id_membro
                                         select u).FirstOrDefault();

                db.Utilizadores.Remove(utilizador);
                db.Membros.Remove(membro);

                Guid partitionKey = Guid.NewGuid();
                string rowKey = membro.Id_membro.ToString(CultureInfo.InvariantCulture);

                Log log = new Log(partitionKey.ToString(), rowKey);
                string descricao = membro.PrimeirNome + " " + membro.UltimoNome + " eliminado com sucesso (" + DateTime.Now + ")";
                log.descricao = descricao;

                tableStorage = new AzureTableStorage();
                tableStorage.InsertToTable(log);

                db.SaveChanges();
                return RedirectToAction("Index");
            }            
        }
    }
}
