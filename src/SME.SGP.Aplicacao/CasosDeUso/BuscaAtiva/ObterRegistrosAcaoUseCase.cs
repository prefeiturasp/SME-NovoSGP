using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosAcaoUseCase : IObterRegistrosAcaoUseCase
    {
        private readonly IMediator mediator;

        public ObterRegistrosAcaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaListagemDto>> Executar(FiltroRegistrosAcaoDto filtro)
        {
            return await mediator.Send(new ObterRegistrosAcaoQuery(filtro));
        }
    }
}
