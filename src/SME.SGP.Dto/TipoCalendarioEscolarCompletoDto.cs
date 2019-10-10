using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class TipoCalendarioEscolarCompletoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int anoLetivo { get; set; }
        public Periodo Periodo { get; set; }
        public Modalidade Modalidade { get; set; }
        public bool Situacao { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string CriadoRF { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
    }
}
