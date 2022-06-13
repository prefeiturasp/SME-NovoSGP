﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlteracaoNotaFechamentoAgrupadaUseCase : AbstractUseCase, INotificarAlteracaoNotaFechamentoAgrupadaUseCase
    {
        public NotificarAlteracaoNotaFechamentoAgrupadaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var wfSemAprovacao = await mediator.Send(new ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery());
            var agruparEmTurmaBimestre = wfSemAprovacao.GroupBy(w => new { w.TurmaId, w.Bimestre });

            foreach (var turmas in agruparEmTurmaBimestre)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoAprovacaoFechamentoPorTurma, turmas, Guid.NewGuid(), null));

            return true;
        }
    }
}
