using System;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto
    {
        public string Dre { get; set; }
        public string RealizouProcedimentoTrabalho { get; set; }
        public int Quantidade { get; set; }        
    }
}