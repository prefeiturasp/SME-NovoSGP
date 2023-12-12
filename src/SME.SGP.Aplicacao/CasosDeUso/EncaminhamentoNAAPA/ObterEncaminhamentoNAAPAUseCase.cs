using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAUseCase : IObterEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAResumoDto>> Executar(FiltroEncaminhamentoNAAPADto filtro)
        {
            var codigoUe = !string.IsNullOrEmpty(filtro.CodigoUe) ? filtro.CodigoUe.Equals("-99") ? string.Empty : filtro.CodigoUe : String.Empty; 
            
            return await mediator.Send(new ObterEncaminhamentosNAAPAQuery(filtro));
        }
    }
}
