using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAula
    {
        Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades);
        Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa();

        Task<long[]> ListarPendenciasPorAulaId(long aulaId);
        Task<long[]> ListarPendenciasPorAulasId(long[] aulaId);

        Task Salvar(long aulaId, string motivo, long pendenciaId);
        Task Excluir(TipoPendencia tipoPendenciaAula, long aulaId);

        Task SalvarVarias(long pendenciaId, IEnumerable<long> aulas);        
        Task<Turma> ObterNomeTurmaPorPendencia(long pendenciaId);
        Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId);
        Task<long> ObterPendenciaAulaPorTurmaIdDisciplinaId(string turmaId, string disciplinaId);
        Task<long> ObterPendenciaAulaIdPorAulaId(long aulaId, TipoPendencia tipoPendencia);
        Task<long> ObterPendenciaIdPorAula(long aulaId, TipoPendencia tipoPendencia);

    }
}