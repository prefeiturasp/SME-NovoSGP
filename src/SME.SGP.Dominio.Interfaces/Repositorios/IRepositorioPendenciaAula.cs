using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaAula
    {
        Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendenciaAula tipoPendenciaAula, string tabelaReferencia);
        Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa();

        Task Salvar(PendenciaAula pendenciaAula);
        Task Excluir(TipoPendenciaAula tipoPendenciaAula, long aulaId);

        

        Task<PendenciaAula> ObterPendenciaPorAulaIdETipo(TipoPendenciaAula tipoPendenciaAula, long aulaId);

        void SalvarVarias(IEnumerable<Aula> aulas, TipoPendenciaAula tipoPendenciaAula);
    }
}