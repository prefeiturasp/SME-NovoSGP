using System;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class DadosGraficoSitaucaoPorUeAnoLetivoDto
    {
        public SituacaoNAAPA Situacao { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataUltimaConsolidacao { get; set; }
        
    }
}