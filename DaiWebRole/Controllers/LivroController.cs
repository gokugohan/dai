using DaiWebRole.Azure;
using DaiWebRole.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace DaiWebRole.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {

        private AzureBlobStorage blobStorage = new AzureBlobStorage();
        private AzureTableStorage tableStorage;

        private EntityDai db = new EntityDai();
        private string[] validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/png"
            };
        

        

        // GET: Livroes
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var livros = db.Livros.Include(l => l.T_categoria).ToListAsync();
            return View(await livros);
        }


        // GET: Livro/Create        
        public ActionResult Novo()
        {
            LivroViewModel model = new LivroViewModel();
            model.CategoriaItem = new List<SelectListItem>();
            var result = from ci in db.Categorias
                         select new
                         {
                             Categoria = ci.Id_categoria,
                             Text = ci.Nome
                         };
            foreach (var item in result)
            {
                SelectListItem temp = new SelectListItem();
                temp.Text = item.Text;
                temp.Value = ""+item.Categoria;                
                model.CategoriaItem.Add(temp);
            }

            ViewBag.DropdownResult = model.CategoriaItem;            
            return View(model);
        }

        // POST: Livro/Create
        [HttpPost]
        public async Task<ActionResult> Novo(LivroViewModel model)
        {
            
            if (model.CapaDoLivro == null || model.CapaDoLivro.ContentLength == 0)
            {
                ModelState.AddModelError("CapaDoLivro", "Campo obrigatório");
            }
            else if (!validImageTypes.Contains(model.CapaDoLivro.ContentType))
            {
                ModelState.AddModelError("CapaDoLivro", "Inserir apenas imagem do tipo GIF, JPG or PNG se faz favor!");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    CloudBlobContainer blobContainer = blobStorage.GetCloudBlobContainer(AzureBlobStorage.CONTAINER_FOTO);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(model.CapaDoLivro.FileName);
                    blob.UploadFromStream(model.CapaDoLivro.InputStream);


                    Categoria categoria = await db.Categorias.FindAsync(model.Categoria);

                    Livro livro = new Livro
                    {
                        Titulo_livro = model.Titulo,
                        Capa_livro = blob.Uri.ToString(),
                        T_categoria = categoria,
                        Autor_livro = model.Autor,
                        Editora_livro = model.Editora,
                        Isbn = model.Isbn
                    };

                    //categoria.T_livro.Add(livro);
                    db.Livros.Add(livro);                    
                    await db.SaveChangesAsync();

                    Guid partitionKey = Guid.NewGuid();
                    string rowKey = livro.Id_livro.ToString(CultureInfo.InvariantCulture);
                    Log log = new Log(partitionKey.ToString(), rowKey);                    
                    log.descricao = livro.Titulo_livro + " inserido com sucesso (" + DateTime.Now + ")";
                    tableStorage = new AzureTableStorage();
                    tableStorage.InsertToTable(log);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    return View(model);
                }
            }
            return View(model);
           
        }

        public string Editar(int? id)
        {
            return "Implementa-se depois, ainda tenho problema com 'Editar imagem'";
        }

        // GET: Categoria/Edit/5
        /*
        public async Task<ActionResult> Editar(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livro livro = await db.Livros.FindAsync(id);
            if (livro == null)
            {
                return HttpNotFound();
            }

            LivroViewModel livroViewModel = new LivroViewModel();
            livroViewModel.CategoriaItem = new List<SelectListItem>();
            var result = from ci in db.Categorias
                         select new
                         {
                             Categoria = ci.Id_categoria,
                             Text = ci.Nome
                         };
            foreach (var item in result)
            {
                SelectListItem temp = new SelectListItem();
                temp.Text = item.Text;
                temp.Value = "" + item.Categoria;
                livroViewModel.CategoriaItem.Add(temp);
            }

            ViewBag.DropdownResult = livroViewModel.CategoriaItem;

            
            

            LivroViewModel model = new LivroViewModel
            {
                Titulo = livro.Titulo_livro,
                CapaDoLivro=null,
                Categoria = livro.Categoria_Id,
                Autor = livro.Autor_livro,
                Editora = livro.Editora_livro,
                Isbn = livro.Isbn
            };
            return View(model);
        }
        */
        // POST: Categoria/Edit/5
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(int id, LivroViewModel model)
        {

            if (ModelState.IsValid)
            {
                Categoria categoria = await db.Categorias.FindAsync(model.Categoria);
                Livro livro = await this.db.Livros.FindAsync(id);
                livro.Titulo_livro = model.Titulo;
                livro.Capa_livro = model.CapaDoLivro.FileName;
                livro.T_categoria = categoria;
                livro.Autor_livro = model.Autor;
                livro.Editora_livro = model.Editora;
                livro.Isbn = model.Isbn;

                this.db.Entry(livro).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
            }
            return RedirectToAction("Index");


        }
        */

        // GET: Livro/Delete/5
        public async Task<ActionResult> Eliminar(int id)
        {
            Livro livro = await db.Livros.FindAsync(id);
            if (livro != null)
            {
                db.Livros.Remove(livro);

                Guid partitionKey = Guid.NewGuid();
                string rowKey = livro.Id_livro.ToString(CultureInfo.InvariantCulture);
                Log log = new Log(partitionKey.ToString(), rowKey);                   
                log.descricao = livro.Titulo_livro + " eliminado com sucesso";
                tableStorage = new AzureTableStorage();
                tableStorage.InsertToTable(log);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // POST: Livro/Delete/5
        /*
        [HttpPost]
        public ActionResult Eliminar(int id, LivroViewModel model)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Requisitar(int id)
        {
            try
            {
                var livro = await (from l in db.Livros
                             where l.Id_livro == id
                             select l).FirstOrDefaultAsync();

                return View(livro);
            }
            catch (Exception e)
            {
                ViewBag.mensagem = "Erro: " + e.Message;
            }
            
            return View();
        }
    }
}
