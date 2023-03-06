using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaDiariaAlunoUseCase : AbstractUseCase, IObterFrequenciaDiariaAlunoUseCase
    {
        public ObterFrequenciaDiariaAlunoUseCase(IMediator mediator) : base(mediator){}

        public async Task<PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>> Executar(FiltroFrequenciaDiariaAlunoDto dto)
        {
            return await mediator.Send(new ObterFrequenciaDiariaAlunoQuery(dto.TurmaId,dto.ComponenteCurricularId,dto.AlunoCodigo,dto.Bimestre, dto.Semestre));
        }
    }
}
