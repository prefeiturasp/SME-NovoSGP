using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaConsulta : IRepositorioBase<CompensacaoAusencia>
    {
        Task<PaginacaoResultadoDto<CompensacaoAusencia>> Listar(Paginacao paginacao, string turmaId, string disciplinaId, int bimestre, string nomeAtividade);
        Task<CompensacaoAusencia> ObterPorAnoTurmaENome(int anoLetivo, long turmaId, string nome, long idIgnorar);
        Task<IEnumerable<Infra.TotalCompensacaoAusenciaDto>> ObterCompesacoesAusenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre);
        Task<Infra.Dtos.TotalCompensacaoAusenciaDto> ObterTotalPorAno(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<Infra.TotalCompensacaoAusenciaDto>> ObterAtividadesCompensacaoConsolidadasPorTurmaEAno(int anoletivo, long dreId, long ueId, int modalidadeCodigo, int bimestre, int semestre);
    }
}
