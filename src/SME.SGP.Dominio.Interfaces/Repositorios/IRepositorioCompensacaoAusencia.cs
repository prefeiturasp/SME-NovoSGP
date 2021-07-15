using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusencia : IRepositorioBase<CompensacaoAusencia>
    {
        Task<PaginacaoResultadoDto<CompensacaoAusencia>> Listar(Paginacao paginacao, string turmaId, string disciplinaId, int bimestre, string nomeAtividade);
        Task<CompensacaoAusencia> ObterPorAnoTurmaENome(int anoLetivo, long turmaId, string nome, long idIgnorar);
        Task<IEnumerable<TotalAusenciasCompensadasDto>> ObterCompesacoesAusenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre);
    }
}
