using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaPaginaInicialListao
    {
        public PendenciaPaginaInicialListao()
        {
            PendenciaFrequencia = false;
            PendenciaPlanoAula = false;
            PendenciaAvaliacoes = false;
            PendenciaDiarioBordo = false;
            PendenciaFechamento = false;
        }
        public bool PendenciaFrequencia { get; set; }
        public bool PendenciaPlanoAula { get; set; }
        public bool PendenciaAvaliacoes { get; set; }
        public bool PendenciaDiarioBordo { get; set; }
        public bool PendenciaFechamento { get; set; }
    }
}
