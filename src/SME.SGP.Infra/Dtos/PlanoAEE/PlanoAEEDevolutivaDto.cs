using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PlanoAEEDevolutivaDto
    {
        public string ParecerCoordenacao { get; set; }
        public string ParecerPAAI { get; set; }
        public string ResponsavelRF { get; set; }
        public string ResponsavelNome { get; set; }
        public bool PodeEditarParecerCoordenacao { get; set; }
        public bool PodeEditarParecerPAAI { get; set; }
        public bool PodeAtribuirResponsavel { get; set; }
    }
}
