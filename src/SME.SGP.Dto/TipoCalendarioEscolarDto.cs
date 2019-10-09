using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class TipoCalendarioEscolarDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int anoLetivo { get; set; }
        public Periodo Periodo { get; set; }

    }
}
