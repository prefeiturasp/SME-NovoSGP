using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.AtividadeInfantil
{
    public class ObterListaAtividadeMuralUseCase : IObterListaAtividadeMuralUseCase
    {
        private readonly IMediator mediator;
        public ObterListaAtividadeMuralUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<IEnumerable<object>> BuscarPorAulaId(long aulaId)
        {
            throw new NotImplementedException();
        }
    }
}
