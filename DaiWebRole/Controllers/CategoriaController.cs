using DaiWebRole.Azure;
using DaiWebRole.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DaiWebRole.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        EntityDai db = new EntityDai();
        AzureTableStorage tableStorage;
        // GET: Categoria
        public async Task<ActionResult> Index()
        {
            return View(await db.Categorias.ToListAsync());
        }
        
        public ActionResult Novo()
        {
            return View(new CategoriaViewModel());
        }

        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Novo(CategoriaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Categoria categoria = new Categoria
                    {
                        Nome = model.Nome
                    };
                    this.db.Categorias.Add(categoria);

                    Guid partitionKey = Guid.NewGuid();
                    string rowKey = categoria.Id_categoria.ToString(CultureInfo.InvariantCulture);

                    Log log = new Log(partitionKey.ToString(),rowKey);
                    string descricao = categoria.Nome + " criado com sucesso (" + DateTime.Now + ")";
                    log.descricao = descricao;

                    tableStorage = new AzureTableStorage();
                    tableStorage.InsertToTable(log);

                    await this.db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Categoria/Edit/5
        public async Task<ActionResult> Editar(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = await db.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            CategoriaViewModel model = new CategoriaViewModel
            {
                Nome = categoria.Nome
            };
            return View(model);
        }

        // POST: Categoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(int id, CategoriaViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Categoria categoria = await this.db.Categorias.FindAsync(id);
                categoria.Nome = model.Nome;
                this.db.Entry(categoria).State = EntityState.Modified;

                Guid partitionKey = Guid.NewGuid();
                string rowKey = categoria.Id_categoria.ToString(CultureInfo.InvariantCulture);

                Log log = new Log(partitionKey.ToString(), rowKey);
                log.descricao = "Categoria editado com sucesso (" + DateTime.Now+")";
                tableStorage = new AzureTableStorage();
                tableStorage.InsertToTable(log);

                await this.db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
            
            
        }

        
        [HttpGet]
        public async Task<ActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = await db.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            Categoria tmp = this.db.Categorias.Remove(categoria);

            Guid partitionKey = Guid.NewGuid();
            string rowKey = categoria.Id_categoria.ToString(CultureInfo.InvariantCulture);

            Log log = new Log(partitionKey.ToString(), rowKey);
            log.descricao = "Categoria: "+ tmp.Nome + " eliminado com sucesso (" + DateTime.Now + ")";
            tableStorage = new AzureTableStorage();
            tableStorage.InsertToTable(log);
            
            await this.db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
