using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public class FinalizarSolicitacaoRelatorioUseCase : AbstractUseCase, IFinalizarSolicitacaoRelatorioUseCase
    {
        public FinalizarSolicitacaoRelatorioUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(long solicitacaoRelatorioId)
        {
            var solicitacaoRelatorio = await mediator.Send(new ObterSolicitacaoRelatorioPorIdQuery(solicitacaoRelatorioId));

            if (solicitacaoRelatorio != null)
                await mediator.Send(new FinalizarSolicitacaoRelatorioCommand(solicitacaoRelatorio));
        }
    }
}


