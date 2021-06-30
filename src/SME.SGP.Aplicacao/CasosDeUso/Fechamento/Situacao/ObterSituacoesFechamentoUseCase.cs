using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacoesFechamentoUseCase : IObterSituacoesFechamentoUseCase
    {
        private readonly IMediator mediator;

        public ObterSituacoesFechamentoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<SituacaoDto>> Executar(bool unificarNaoIniciado)
        {
            var retorno = await mediator.Send(new ObterSituacoesFechamentoQuery(unificarNaoIniciado));
            return retorno;
        }

    }
}

