﻿using System;

namespace SME.SGP.Infra
{
    public class TotalFrequenciaEAulasPorPeriodoDto
    {
        public string DescricaoAnoTurma { get; set; }
        public long TotalAulas { get; set; }
        public long TotalFrequencias { get; set; }
        public string TotalFrequenciaFormatado
        {
            get => $"{TotalFrequencias} frequências registradas ({PercentualFrequencia}% das aulas)";
        }
        public double PercentualFrequencia
        {
            get
            {
                if (TotalAulas == 0)
                    return 0;

                var porcentagem = ((double)TotalFrequencias / TotalAulas) * 100;

                return Math.Round(porcentagem > 100 ? 100 : porcentagem, 2);
            }
        }
    }
}
