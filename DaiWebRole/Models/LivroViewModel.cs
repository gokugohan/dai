using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DaiWebRole.Models
{
    public class LivroViewModel
    {
        [Required]
        [Display(Name="Título do livro")]
        public string Titulo { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name="Capa do livro")]
        public HttpPostedFileBase CapaDoLivro { get; set; }
        
        public int Categoria { get; set; }
        

        [Required]
        [Display(Name="Autor")]
        public string Autor { get; set; }

        [Required]
        [Display(Name="Editora")]
        public string Editora { get; set; }

        [Required]
        [Display(Name="Isbn")]
        public string Isbn { get; set; }

        public List<SelectListItem> CategoriaItem { get; set; }

    }
}
