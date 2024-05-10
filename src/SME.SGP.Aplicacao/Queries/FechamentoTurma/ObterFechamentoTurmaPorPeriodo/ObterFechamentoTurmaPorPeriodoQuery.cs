using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorPeriodoQuery : IRequest<FechamentoTurma>
    {
        public ObterFechamentoTurmaPorPeriodoQuery(long turmaId, long periodoId)
        {
            TurmaId = turmaId;
            PeriodoId = periodoId;
        }

        public long TurmaId { get; set; }
        public long PeriodoId { get; set; }
    }
}
