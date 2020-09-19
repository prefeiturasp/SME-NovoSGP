using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanejamentoAnualUseCase : ISalvarPlanejamentoAnualUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPlanejamentoAnualUseCase(IMediator mediator,
                                              IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<AuditoriaDto> Executar(long turmaId, long componenteCurricularId, SalvarPlanejamentoAnualDto dto)
        {
            AuditoriaDto auditoria;
            unitOfWork.IniciarTransacao();

            if (dto.Id > 0)
            {
                auditoria = await mediator.Send(new AlterarPlanejamentoAnualCommand()
                {
                    Id = dto.Id,
                    PeriodoEscolarId = dto.PeriodoEscolarId,
                    TurmaId = turmaId,
                    Componentes = dto.Componentes,
                    ComponenteCurricularId = componenteCurricularId
                });
            }
            else
                auditoria = await mediator.Send(new SalvarPlanejamentoAnualCommand()
                {
                    PeriodoEscolarId = dto.PeriodoEscolarId,
                    TurmaId = turmaId,
                    Componentes = dto.Componentes,
                    ComponenteCurricularId = componenteCurricularId
                });

            unitOfWork.PersistirTransacao();

            return auditoria;
        }
    }
}
