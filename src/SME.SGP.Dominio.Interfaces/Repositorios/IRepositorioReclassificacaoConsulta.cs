using SME.SGP.Dominio.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioReclassificacaoConsulta
    {
        Task<IEnumerable<ReclassificacaoRawDto>> ObterReclassificacao(string codigoDre, string codigoUe, int anoLetivo, int anoTurma);
    }
}
