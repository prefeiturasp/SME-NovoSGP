using MediatR;
using System;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQuery : IRequest<long>
    {
        public ObterTotalAlunosPorAnoModalidadeDreAnoLetivoInicioEFimQuery(string anoEscolar, int modalidadeTurma, int anoLetivo, long codigoDre, DateTime dataInicio, DateTime dataFim)
        {
            AnoLetivo = anoLetivo;
            AnoEscolar = anoEscolar;
            ModalidadeTurma = modalidadeTurma;
            DataInicio = dataInicio;
            DataFim = dataFim;
            CodigoDre = codigoDre;
        }

        public int ModalidadeTurma { get; set; }
        public int AnoLetivo { get; set; }
        public string AnoEscolar { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public long CodigoDre { get; set; }
    }
}
