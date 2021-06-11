using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoPorTurmaPeriodoQuery : IRequest<FechamentoTurma>
    {
        public long PeriodoEscolarId { get; set; }
        public long TurmaId { get; set; }

    }
}
