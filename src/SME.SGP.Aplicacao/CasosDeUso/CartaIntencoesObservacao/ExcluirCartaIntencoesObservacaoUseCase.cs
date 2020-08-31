using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCartaIntencoesObservacaoUseCase : IExcluirCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;

        public ExcluirCartaIntencoesObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long observacaoId)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new ExcluirCartaIntencoesObservacaoCommand(observacaoId, usuarioId));
        }
    }
}
