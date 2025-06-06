﻿using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroGraficoEncaminhamentoPorSituacaoDto
    {
        public long? UeId { get; set; }
        public long? DreId { get; set; }
        public int AnoLetivo { get; set; }
        public Modalidade? Modalidade { get; set; }
    }
}