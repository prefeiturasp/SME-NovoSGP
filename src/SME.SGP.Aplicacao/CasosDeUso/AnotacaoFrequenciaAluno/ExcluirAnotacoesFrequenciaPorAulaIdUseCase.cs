﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacoesFrequenciaPorAulaIdUseCase : AbstractUseCase, IExcluirAnotacoesFrequenciaPorAulaIdUseCase
    {
        public ExcluirAnotacoesFrequenciaPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroIdDto>();

            await mediator.Send(new ExcluirAnotacoesFrequencciaDaAulaCommand(filtro.Id));
            return true;
        }
    }
}
