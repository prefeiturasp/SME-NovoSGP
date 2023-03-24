using System;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoBimestreDto
    {
        public string Bimestre { get; set; }
        public int AulasPrevistas { get; set; }
        public int AulasRealizadas { get; set; }
        public int Ausencias { get; set; }
        public int Compensacoes { get; set; }
        public double? Frequencia { get; set; }
        public bool PossuiJustificativas { get; set; }
        public int Semestre { get; set; }

        public double PercentualFrequencia(int? TotalAulas = null, int? TotalFaltasNaoCompensadas = null)
        {
            if (!TotalAulas.HasValue)
                TotalAulas = AulasRealizadas;

            if (!TotalFaltasNaoCompensadas.HasValue)
                TotalFaltasNaoCompensadas = Ausencias - Compensacoes;

            if (TotalAulas == 0)
                return 0;

            var porcentagem = 100 - (((double)TotalFaltasNaoCompensadas / (double)TotalAulas) * 100);

            return Math.Round(porcentagem > 100 ? 100 : porcentagem, 2);
            
        }
    }
}