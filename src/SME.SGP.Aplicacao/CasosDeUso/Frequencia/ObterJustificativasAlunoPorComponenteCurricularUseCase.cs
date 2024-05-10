using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterJustificativasAlunoPorComponenteCurricularUseCase : AbstractUseCase, IObterJustificativasAlunoPorComponenteCurricularUseCase
    {
        public ObterJustificativasAlunoPorComponenteCurricularUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<PaginacaoResultadoDto<JustificativaAlunoDto>> Executar(FiltroJustificativasAlunoPorComponenteCurricular dto)
        {
            return await mediator.Send(new ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery(dto.TurmaId, dto.ComponenteCurricularCodigo, dto.AlunoCodigo, dto.Bimestre, dto.Semestre));
        }
    }
}
