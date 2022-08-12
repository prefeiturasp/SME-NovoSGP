using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverResponsavelEncaminhamentoAEECommandHandler : IRequestHandler<RemoverResponsavelEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public RemoverResponsavelEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(RemoverResponsavelEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel;
            encaminhamentoAEE.ResponsavelId = null;

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            var pendenciaEncaminhamentoAEE = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(request.EncaminhamentoId));
            if (pendenciaEncaminhamentoAEE != null)
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoAEE.PendenciaId));

            return idEntidadeEncaminhamento != 0;
        }
    }
}
