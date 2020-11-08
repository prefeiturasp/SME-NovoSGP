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

        Task Salvar(PendenciaAula pendenciaAula);
        Task Excluir(TipoPendencia tipoPendenciaAula, long aulaId);

        Task SalvarVarias(long pendenciaId, IEnumerable<long> aulas);
    }
}