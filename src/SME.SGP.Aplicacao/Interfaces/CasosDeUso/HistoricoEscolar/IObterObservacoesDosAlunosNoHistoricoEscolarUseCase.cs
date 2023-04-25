using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterObservacoesDosAlunosNoHistoricoEscolarUseCase : IUseCase<string, PaginacaoResultadoDto<AlunoComObservacaoDoHistoricoEscolarDto>>
    {
    }
}
