﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPAPUseCase : IObterQuestionarioPAPUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioPAPUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<QuestaoDto>> Executar(string codigoTurma, string codigoAluno, long periodoIdPAP, long questionarioId, long? papSecaoId)
        {
            return await mediator.Send(new ObterQuestionarioPAPPorPeriodoQuery(codigoTurma,codigoAluno,periodoIdPAP,questionarioId,papSecaoId));
        }
    }
}
