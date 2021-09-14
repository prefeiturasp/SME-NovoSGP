using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterListaAlunosDaTurmaUseCase: IUseCase<string, PaginacaoResultadoDto<AlunoSimplesDto>>
    {
    }
}
