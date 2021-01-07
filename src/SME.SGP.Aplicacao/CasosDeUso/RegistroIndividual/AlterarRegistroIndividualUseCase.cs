using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroIndividualUseCase : AbstractUseCase, IAlterarRegistroIndividualUseCase
    {
        public AlterarRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(AlterarRegistroIndividualDto dto)
        {
            var auditoria = await mediator.Send(new AlterarRegistroIndividualCommand(dto.Id, dto.TurmaId, dto.AlunoCodigo, dto.ComponenteCurricularId, dto.Data, dto.Registro));

            return auditoria;
        }
    }
}
