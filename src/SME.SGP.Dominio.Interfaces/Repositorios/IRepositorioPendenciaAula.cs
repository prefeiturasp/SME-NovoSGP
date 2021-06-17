using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAula
    {
        Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades, int anoLetivo);
        Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa(int anoLetivo);

        Task<long[]> ListarPendenciasPorAulaId(long aulaId);
        Task<long[]> ListarPendenciasPorAulasId(long[] aulasId);

        Task<PendenciaAulaDto> PossuiPendenciasPorAulaId(long aulaId, bool ehInfantil);
        Task<bool> PossuiPendenciasPorAulasId(long[] aulasId, bool ehInfantil);

        Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulaId(long aulaId);
        Task<bool> PossuiPendenciasAtividadeAvaliativaPorAulasId(long[] aulasId);
        Task<bool> PossuiAtividadeAvaliativaSemNotaPorAulasId(long[] aulasId);

        Task Salvar(long aulaId, string motivo, long pendenciaId);
        Task Excluir(long pendenciaId, long aulaId);

        void SalvarVarias(long pendenciaId, IEnumerable<long> aulas);        
        Task<Turma> ObterTurmaPorPendencia(long pendenciaId);
        Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId);
        Task<long> ObterPendenciaAulaPorTurmaIdDisciplinaId(string turmaId, string disciplinaId, string professorRf, TipoPendencia tipoPendencia);
        Task<long> ObterPendenciaAulaIdPorAulaId(long aulaId, TipoPendencia tipoPendencia);
        Task<IEnumerable<long>> ObterPendenciaIdPorAula(long aulaId, TipoPendencia tipoPendencia);

    }
}