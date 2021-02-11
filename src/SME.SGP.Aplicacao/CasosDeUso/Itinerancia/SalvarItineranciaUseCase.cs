using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaUseCase : AbstractUseCase, ISalvarItineranciaUseCase
    {
        public SalvarItineranciaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public Task<AuditoriaDto> Executar(ItineranciaDto param)
        {
            throw new NotImplementedException();
        }
    }
}
