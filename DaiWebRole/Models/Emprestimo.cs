//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DaiWebRole.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emprestimo
    {
        public int Id_emprestimo { get; set; }
        public int Id_membro { get; set; }
        public int Id_livro { get; set; }
        public System.DateTime Data_emprestimo { get; set; }
        public System.DateTime Data_devolucao { get; set; }
    
        public virtual Livro T_livro { get; set; }
        public virtual Membro T_membro { get; set; }
    }
}