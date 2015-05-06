using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DaiWebRole.Models
{
    public class VideoModel
    {
        [Required]
        public string Titulo { get; set; }       

        [DataType(DataType.Upload)]
        HttpPostedFileBase VideoUpload { get; set; }
    }
}
