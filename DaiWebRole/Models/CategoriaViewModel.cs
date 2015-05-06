using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole.Models
{
    public class CategoriaViewModel
    {
        [Required]
        [Display(Name="Categoria")]
        public string Nome { get; set; }
    }
}
