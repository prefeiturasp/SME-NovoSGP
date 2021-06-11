using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommandHandler : IRequestHandler<ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommand, bool>
    {
        public IRepositorioEncaminhamentoAEESecao repositorioEncaminhamentoAEESecao { get; }
        public IMediator mediator { get; }

        public ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEESecao repositorioEncaminhamentoAEESecao)
        {
            this.repositorioEncaminhamentoAEESecao = repositorioEncaminhamentoAEESecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEESecao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirSecoesEncaminhamentoAEEPorEncaminhamentoIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioEncaminhamentoAEESecao.RemoverLogico(request.EncaminhamentoAEEId, "encaminhamento_aee_id");

            var secoesIds = await repositorioEncaminhamentoAEESecao.ObterIdsSecoesPorEncaminhamentoAEEId(request.EncaminhamentoAEEId);

            foreach(var secaoId in secoesIds)
                await mediator.Send(new ExcluirQuestaoEncaminhamentoAEEPorSecaoIdCommand(secaoId));

            return true;
        }
    }
}
