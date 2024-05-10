using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualUseCase : ISalvarPlanejamentoAnualUseCase
    {
        private readonly IMediator mediator;

        public SalvarPlanejamentoAnualUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }
        public async Task<PlanejamentoAnualAuditoriaDto> Executar(long turmaId, long componenteCurricularId, SalvarPlanejamentoAnualDto dto)
        {           
           

            var auditoria = await mediator.Send(new SalvarPlanejamentoAnualCommand()
            {
                TurmaId = turmaId,
                ComponenteCurricularId = componenteCurricularId,
                PeriodosEscolares = dto.PeriodosEscolares
            });

            return auditoria;
        }
    }
}
