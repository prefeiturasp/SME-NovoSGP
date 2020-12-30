using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualUseCase : AbstractUseCase, IInserirRegistroIndividualUseCase
    {
        public InserirRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirRegistroIndividualDto dto)
        {
            var auditoria = await mediator.Send(new InserirRegistroIndividualCommand(dto.TurmaId, dto.AlunoCodigo, dto.ComponenteCurricularId, dto.Data, dto.Registro));
            return auditoria;
        }
    }
}
