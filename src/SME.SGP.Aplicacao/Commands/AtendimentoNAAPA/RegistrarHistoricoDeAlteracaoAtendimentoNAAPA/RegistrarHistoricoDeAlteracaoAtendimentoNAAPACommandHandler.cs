using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegistrarHistoricoDeAlteracaoAtendimentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoAtendimentoNAAPACommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoAtendimentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorioEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoNAAPAHistoricoAlteracoes = repositorioEncaminhamentoNAAPAHistoricoAlteracoes ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPAHistoricoAlteracoes)); 
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var historicoAlteracao = await mediator.Send(new ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery(request.EncaminhamentoNAAPASecaoAlterado, request.EncaminhamentoNAAPASecaoExistente, request.TipoHistoricoAlteracoes));

            if (historicoAlteracao.NaoEhNulo())
                return await repositorioEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historicoAlteracao);

            return 0;
        }
    }
}
