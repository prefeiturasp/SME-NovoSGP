using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoEscolaAquiUseCase : IObterComunicadoEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public ObterComunicadoEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<ComunicadoCompletoDto> Executar(long id)
        {
            return await mediator.Send(new ObterComunicadoSimplesPorIdQuery(id));
        }
    }
}
