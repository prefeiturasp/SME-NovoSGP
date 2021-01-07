using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAusenciaRegistroIndividualUseCase : AbstractUseCase, IGerarPendenciaAusenciaRegistroIndividualUseCase
    {
        public GerarPendenciaAusenciaRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            throw new NotImplementedException("Será implementado em uma task separada.");
        }
    }
}