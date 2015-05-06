using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole.Models
{
    public class EmprestimoViewModel
    {
        [Required]
        public int Membro { get; set; }
        [Required]
        public int Livro { get; set; }
        [Required]
        [Display(Name="Data empréstimo")]
        public DateTime DataEmprestimo { get; set; }        
        [Display(Name="Data devolução")]
        public DateTime DataDevolucao { get; set; }
    }
}
