using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDresUseCase : AbstractUseCase, IObterDresUseCase
    {
        public ObterDresUseCase(IMediator mediator) : base(mediator)
        {}

        public Task<IEnumerable<Dre>> Executar()
        {
            return mediator.Send(ObterTodasDresQuery.Instance);
        }
    }
}
