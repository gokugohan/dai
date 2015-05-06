using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DaiWebRole.Models
{
    [Authorize]
    public class MembroViewModel
    {
        [Required]
        [Display(Name="Primeiro nome")]
        public string PrimeiroNome { get; set; }
        [Required]
        [Display(Name="Último nome")]
        public string UltimoNome { get; set; }
        
        [Required]
        [DataType(DataType.Upload)]
        [Display(Name="Fotografia")]
        public HttpPostedFileBase Foto { get; set; }

        [Required]
        [Display(Name="Género")]
        public string Genero { get; set; }
        [Required]
        [Display(Name="Endereço")]
        public string Endereco { get; set; }
        [Required]
        [Display(Name="Contacto")]
        public string Contacto { get; set; }

    }
}
