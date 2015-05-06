﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole.Models
{
    public class UtilizadorViewModel
    {
        [Required]
        [Display(Name="Nome utilizador")]
        public string UserName { get; set; }
        [Required]
        [Display(Name="Senha")]
        public string Password { get; set; }

    }
}
