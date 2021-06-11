using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterPlanosAEEUseCase : AbstractUseCase, IObterPlanosAEEUseCase
    {
        public ObterPlanosAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<PlanoAEEResumoDto>> Executar(FiltroPlanosAEEDto filtro)
        {
            return await mediator.Send(new ObterPlanosAEEQuery(filtro.DreId, filtro.UeId, filtro.TurmaId, filtro.AlunoCodigo, filtro.Situacao));
        }
    }
}
