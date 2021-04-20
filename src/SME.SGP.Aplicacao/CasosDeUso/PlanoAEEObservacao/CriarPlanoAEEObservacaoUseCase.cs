using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarPlanoAEEObservacaoUseCase : AbstractUseCase, ICriarPlanoAEEObservacaoUseCase
    {
        public CriarPlanoAEEObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(PersistenciaPlanoAEEObservacaoDto dto)
        {
            return await mediator.Send(new CriarPlanoAEEObservacaoCommand(dto.PlanoAEEId, dto.Observacao, dto.Usuarios));
        }
    }
}
