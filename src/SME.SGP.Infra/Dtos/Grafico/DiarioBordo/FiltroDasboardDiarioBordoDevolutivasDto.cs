using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroDasboardDiarioBordoDevolutivasDto
    {
        [Required]
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long DreId { get; set; }
    }
}
