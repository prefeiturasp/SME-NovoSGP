using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaUseCase : AbstractUseCase, IInserirOcorrenciaUseCase
    {
        public InserirOcorrenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(InserirOcorrenciaDto dto) 
            => await mediator.Send(new InserirOcorrenciaCommand(dto.DataOcorrencia, dto.HoraOcorrencia, dto.Titulo, dto.Descricao, dto.OcorrenciaTipoId, dto.TurmaId, dto.CodigosAlunos));
    }
}
