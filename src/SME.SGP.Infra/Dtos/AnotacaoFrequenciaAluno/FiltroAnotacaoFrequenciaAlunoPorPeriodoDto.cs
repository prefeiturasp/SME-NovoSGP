using System;

namespace SME.SGP.Infra
{
    public class FiltroAnotacaoFrequenciaAlunoPorPeriodoDto
    {
        public FiltroAnotacaoFrequenciaAlunoPorPeriodoDto(string codigoAluno, DateTime dataInicio, DateTime dataFim)
        {
            CodigoAluno = codigoAluno;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public string CodigoAluno { get; }
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }
    }
}
