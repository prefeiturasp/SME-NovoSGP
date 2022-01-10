using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQueryHandler : IRequestHandler<ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQuery, PeriodoFechamentoBimestre>
    {
        private readonly IRepositorioPeriodoFechamento repositorioPeriodoFechamento;

        public ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQueryHandler(IRepositorioPeriodoFechamento repositorioPeriodoFechamento)
        {
            this.repositorioPeriodoFechamento = repositorioPeriodoFechamento ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoFechamento));
        }

        public async Task<PeriodoFechamentoBimestre> Handle(ObterPeriodoFechamentoPorTurmaBimestrePeriodoEscolarQuery request, CancellationToken cancellationToken)
            => await repositorioPeriodoFechamento.ObterPeriodoFechamentoTurma(request.Turma.AnoLetivo, request.Bimestre, request.PeriodoEscolarId);
    }
}
