using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterListaAlunosFiltroHistoricoEscolarUseCase : IUseCase<FiltroBuscaAlunosDto, PaginacaoResultadoDto<AlunoSimplesDto>>
    {
    }
}
