using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalReclassificacao
    {
        Task ExcluirReclassificacaoAnual(int anoLetivo);
        Task BulkInsertAsync(IEnumerable<PainelEducacionalReclassificacao> registros);
    }
}
