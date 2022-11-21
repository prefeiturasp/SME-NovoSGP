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
            return await mediator.Send(new ObterEncaminhamentosNAAPAQuery(filtro.ExibirHistorico, filtro.AnoLetivo,
                filtro.DreId, filtro.CodigoUe, filtro.TurmaId, filtro.NomeAluno, filtro.DataAberturaQueixaInicio, 
                filtro.DataAberturaQueixaFim, filtro.Situacao, filtro.Prioridade));
        }
    }
}
