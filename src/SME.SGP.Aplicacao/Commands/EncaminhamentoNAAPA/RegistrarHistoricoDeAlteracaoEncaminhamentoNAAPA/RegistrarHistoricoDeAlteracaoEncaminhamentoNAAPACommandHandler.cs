using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.mediator = mediator;
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes;
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var historicoAlteracao = await mediator.Send(new ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(request.EncaminhamentoNAAPASecaoAlterado, request.EncaminhamentoNAAPAExistente));

            if (historicoAlteracao != null)
                return await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historicoAlteracao);

            return 0;
        }
    }
}
