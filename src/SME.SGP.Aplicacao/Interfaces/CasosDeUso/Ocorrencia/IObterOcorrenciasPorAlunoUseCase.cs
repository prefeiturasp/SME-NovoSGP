using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterOcorrenciasPorAlunoUseCase : IUseCase<FiltroTurmaAlunoSemestreDto, PaginacaoResultadoDto<OcorrenciasPorAlunoDto>>
    {
    }
}
