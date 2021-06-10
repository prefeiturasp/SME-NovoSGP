using System;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoDto
    {
        public string AlunoCodigo { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }

        public int NumeroFaltasNaoCompensadas { get => TotalAusencias - TotalCompensacoes; }
        public double PercentualFrequencia 
        {
            get
            {
                if (TotalAulas == 0)
                    return 0;

                var porcentagem = 100 - ((double)NumeroFaltasNaoCompensadas / TotalAulas) * 100;

                return Math.Round(porcentagem > 100 ? 100 : porcentagem, 2);
            }
        }
    }
}
