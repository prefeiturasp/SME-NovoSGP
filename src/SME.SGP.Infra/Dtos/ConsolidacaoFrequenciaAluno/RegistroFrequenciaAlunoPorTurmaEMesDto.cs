using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoPorTurmaEMesDto
    {
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public decimal Percentual
        {
            get
            {
                if (QuantidadeAulas == 0)
                    return 0;

                var faltasNaoCompensadas = QuantidadeAusencias - QuantidadeCompensacoes;
                var porcentagem = 100 - (Convert.ToDecimal(faltasNaoCompensadas) / QuantidadeAulas * 100);

                return Math.Round(porcentagem > 100 ? 100 : porcentagem, 2);
            }
        }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
