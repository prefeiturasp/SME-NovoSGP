using SME.SGP.Infra;


namespace SME.SGP.Aplicacao
{
    public interface IObterFrequenciaDiariaAlunoUseCase : IUseCase<FiltroFrequenciaDiariaAlunoDto, PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>>
    {
    }
}
