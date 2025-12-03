using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAUseCase : IObterAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAResumoDto>> Executar(FiltroAtendimentoNAAPADto filtro)
        {
            if (!string.IsNullOrEmpty(filtro.CodigoUe) && filtro.CodigoUe.Equals("-99"))
                filtro.CodigoUe = string.Empty;

            return await mediator.Send(new ObterEncaminhamentosNAAPAQuery(filtro));
        }
    }
}
