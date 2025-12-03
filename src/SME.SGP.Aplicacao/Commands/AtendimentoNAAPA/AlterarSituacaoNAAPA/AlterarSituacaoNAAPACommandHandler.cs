using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarSituacaoNAAPACommandHandler : IRequestHandler<AlterarSituacaoNAAPACommand, bool>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;
        private readonly IMediator mediator;

        public AlterarSituacaoNAAPACommandHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA, IMediator mediator)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(AlterarSituacaoNAAPACommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new RegistrarHistoricoDeAlteracaoDaSituacaoDoAtendimentoNAAPACommand(request.Encaminhamento, request.Situacao));
            request.Encaminhamento.Situacao = request.Situacao;
            await repositorioEncaminhamentoNAAPA.SalvarAsync(request.Encaminhamento);

            return true;
        }
    }
}
