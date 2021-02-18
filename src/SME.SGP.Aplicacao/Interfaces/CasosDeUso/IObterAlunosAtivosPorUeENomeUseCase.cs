using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAlunosAtivosPorUeENomeUseCase : IUseCase<FiltroBuscaEstudantesAtivoDto, PaginacaoResultadoDto<AlunoParaAutoCompleteAtivoDto>>
    {
    }
}