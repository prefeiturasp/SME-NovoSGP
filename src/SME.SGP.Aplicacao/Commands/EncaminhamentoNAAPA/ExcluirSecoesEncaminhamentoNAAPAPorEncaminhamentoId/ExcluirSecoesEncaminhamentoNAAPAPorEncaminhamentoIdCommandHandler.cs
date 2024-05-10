using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommandHandler : IRequestHandler<ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommand, bool>
    {
        public IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao { get; }
        public IMediator mediator { get; }

        public ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirSecoesEncaminhamentoNAAPAPorEncaminhamentoIdCommand request, CancellationToken cancellationToken)
        {
            var secoesIds = await repositorioEncaminhamentoNAAPASecao.ObterIdsSecoesPorEncaminhamentoNAAPAId(request.EncaminhamentoNAAPAId);

            foreach(var secaoId in secoesIds)
                await mediator.Send(new ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand(secaoId));
            
            await repositorioEncaminhamentoNAAPASecao.RemoverLogico(request.EncaminhamentoNAAPAId, "encaminhamento_naapa_id");

            return true;
        }
    }
}
