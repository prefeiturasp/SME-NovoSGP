using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPeriodoFechamentoVigenteUseCase : AbstractUseCase, IObterPeriodoFechamentoVigenteUseCase
    {
        public ObterPeriodoFechamentoVigenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PeriodoFechamentoVigenteDto> Executar(FiltroPeriodoFechamentoVigenteDto filtro)
        {
            return await mediator.Send(new ObterPeriodoFechamentoVigentePorAnoModalidadeQuery(filtro.AnoLetivo, filtro.ModalidadeTipoCalendario));
        }
    }
}
