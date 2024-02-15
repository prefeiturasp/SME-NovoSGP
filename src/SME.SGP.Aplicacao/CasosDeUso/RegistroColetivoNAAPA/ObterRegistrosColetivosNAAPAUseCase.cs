using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosColetivosNAAPAUseCase : IObterRegistrosColetivosNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterRegistrosColetivosNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<RegistroColetivoListagemDto>> Executar(FiltroRegistroColetivoDto filtro)
        {
            if (filtro.UeId.HasValue && filtro.UeId.Equals(-99))
                filtro.UeId = null;

            return await mediator.Send(new ObterRegistrosColetivosNAAPAQuery(filtro));
        }
    }
}
