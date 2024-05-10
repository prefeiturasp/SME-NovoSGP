﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioEncaminhamentoNAAPAUseCase : IObterQuestionarioEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestionarioEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<QuestaoDto>> Executar(long questionarioId, long? encaminhamentoId, string codigoAluno, string codigoTurma)
        {
            return
                await mediator
                .Send(new ObterQuestionarioEncaminhamentoNAAPAQuery(questionarioId, encaminhamentoId, codigoAluno, codigoTurma));
        }
    }
}
