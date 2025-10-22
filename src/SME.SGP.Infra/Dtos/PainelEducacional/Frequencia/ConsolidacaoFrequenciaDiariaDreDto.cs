﻿using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Frequencia
{
    public class ConsolidacaoFrequenciaDiariaDreDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public NivelFrequenciaEnum NivelFrequencia { get; set; }
        public string Ue { get; set; }
        public int AnoLetivo { get; set; }
        public int TotalEstudantes { get; set; }
        public int TotalPresentes { get; set; }
        public decimal PercentualFrequencia { get; set; }
        public DateTime DataAula { get; set; }
    }
}
