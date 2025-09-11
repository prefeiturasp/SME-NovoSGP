using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaOuPlanoNaRecorrenciaUseCase: AbstractUseCase, IObterFrequenciaOuPlanoNaRecorrenciaUseCase
    {
        public ObterFrequenciaOuPlanoNaRecorrenciaUseCase(IMediator mediator): base(mediator) { }

        public async Task<bool> Executar(long aulaId)
            => await mediator.Send(new ObterFrequenciaOuPlanoNaRecorrenciaQuery(aulaId));
    }
}
