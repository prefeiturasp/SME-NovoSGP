using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterJustificativasAlunoPorComponenteCurricularUseCase : IUseCase<FiltroJustificativasAlunoPorComponenteCurricular, PaginacaoResultadoDto<JustificativaAlunoDto>>
    {
    }
}
