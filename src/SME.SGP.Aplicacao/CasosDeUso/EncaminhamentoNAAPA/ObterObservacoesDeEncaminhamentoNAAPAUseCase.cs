﻿using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesDeEncaminhamentoNAAPAUseCase : AbstractUseCase, IObterObservacoesDeEncaminhamentoNAAPAUseCase
    {

        public ObterObservacoesDeEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>> Executar(long encaminhamentoNAAPAId)
        {
            var usuarioLohadoRf = await mediator.Send(ObterUsuarioLogadoRFQuery.Instance);
            return await mediator.Send(new ObterObservacaoEncaminhamentosNAAPAQuery(encaminhamentoNAAPAId, usuarioLohadoRf));
        }
    }
}
