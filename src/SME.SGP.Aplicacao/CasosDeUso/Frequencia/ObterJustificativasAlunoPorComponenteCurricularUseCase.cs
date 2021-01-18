using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterJustificativasAlunoPorComponenteCurricularUseCase : AbstractUseCase, IObterJustificativasAlunoPorComponenteCurricularUseCase
    {
        public ObterJustificativasAlunoPorComponenteCurricularUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<IEnumerable<JustificativaAlunoDto>> Executar(FiltroJustificativasAlunoPorComponenteCurricular dto)
        {
            var justificativas = await mediator.Send(new ObterMotivoPorTurmaAlunoComponenteCurricularQuery(dto.TurmaId, dto.ComponenteCurricularCodigo, dto.AlunoCodigo));
            return justificativas;
        }
    }
}
