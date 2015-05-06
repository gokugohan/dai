using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWebRole.Models
{
    public class DaiContext:DbContext
    {
        public DaiContext()
            : base("name=con")
        {

        }
        public DaiContext(string conString)
            : base(conString)
        {

        }

        public System.Data.Entity.DbSet<Foto> Fotos { get; set; }
        public System.Data.Entity.DbSet<Video> Videos { get; set; }
    }
}
