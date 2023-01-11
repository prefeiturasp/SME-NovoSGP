﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoEncaminhamentoNAAPAPorIdCommandHandler : IRequestHandler<ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA { get; }

        public ExcluirQuestaoEncaminhamentoNAAPAPorIdCommandHandler(IMediator mediator, IRepositorioQuestaoEncaminhamentoNAAPA repositorioQuestaoEncaminhamentoNAAPA)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioQuestaoEncaminhamentoNAAPA = repositorioQuestaoEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioQuestaoEncaminhamentoNAAPA));
        }

        public async Task<bool> Handle(ExcluirQuestaoEncaminhamentoNAAPAPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoEncaminhamentoNAAPA.RemoverLogico(request.QuestaoId);

            var questoesIds = await repositorioQuestaoEncaminhamentoNAAPA.ObterQuestoesPorSecaoId(request.QuestaoId);

            foreach (var questaoId in questoesIds)
                await mediator.Send(new ExcluirRespostaEncaminhamentoAEEPorQuestaoIdCommand(questaoId));

            return true;
        }
    }
}
