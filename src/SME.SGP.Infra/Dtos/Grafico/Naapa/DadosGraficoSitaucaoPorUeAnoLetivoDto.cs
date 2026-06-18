using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra
{
    public class DadosGraficoSitaucaoPorUeAnoLetivoDto
    {
        public SituacaoNAAPA Situacao { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataUltimaConsolidacao { get; set; }

    }
}