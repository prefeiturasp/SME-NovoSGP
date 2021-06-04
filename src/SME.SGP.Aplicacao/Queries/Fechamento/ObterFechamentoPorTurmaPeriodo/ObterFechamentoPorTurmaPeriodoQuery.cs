using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoQuery : IRequest<FechamentoTurma>
    {
        public long PeriodoEscolarId { get; internal set; }
        public long TurmaId { get; set; }

    }
}
