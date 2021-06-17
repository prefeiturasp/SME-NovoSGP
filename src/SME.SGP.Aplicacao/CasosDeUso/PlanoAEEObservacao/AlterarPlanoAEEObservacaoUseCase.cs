using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarPlanoAEEObservacaoUseCase : AbstractUseCase, IAlterarPlanoAEEObservacaoUseCase
    {
        public AlterarPlanoAEEObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(PersistenciaPlanoAEEObservacaoDto dto)
        {
            return await mediator.Send(new AlterarPlanoAEEObservacaoCommand(dto.Id, dto.PlanoAEEId, dto.Observacao, dto.Usuarios));
        }
    }
}
