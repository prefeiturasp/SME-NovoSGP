using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoPorTurmaEMesDto
    {
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public double Percentual
        {
            get
            {
                var frequenciaCalculo = new FrequenciaAluno()
                {
                    TotalAulas = QuantidadeAulas,
                    TotalAusencias = QuantidadeAusencias,
                    TotalCompensacoes = QuantidadeCompensacoes
                };

                return frequenciaCalculo.PercentualFrequencia;
            }
        }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
    }
}
