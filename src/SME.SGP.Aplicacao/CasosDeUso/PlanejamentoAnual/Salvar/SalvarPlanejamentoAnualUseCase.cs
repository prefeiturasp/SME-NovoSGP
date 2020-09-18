using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PlanejamentoAnual.Salvar
{
    public class SalvarPlanejamentoAnualUseCase
    {
        private readonly IMediator mediator;

        public SalvarPlanejamentoAnualUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<AuditoriaDto> Executar(long turmaId, long componenteCurricularId, SalvarPlanejamentoAnualDto dto)
        {
            return await mediator.Send(new SalvarPlanejamentoAnualCommand() { 
            Id=dto.Id,
            PeriodoEscolarId=dto.PeriodoEscolarId,
            Componentes=new List<Dominio.PlanejamentoAnualComponente>
            {
                new Dominio.PlanejamentoAnualComponente
                {
                    
                }
            }
            });
        }
    }
}
