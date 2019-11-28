using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtribuicaoCJ : IRepositorioBase<AtribuicaoCJ>
    {
        Task<IEnumerable<AtribuicaoCJ>> ObterPorComponenteTurmaModalidadeUe(Modalidade? modalidade, string turmaId, string ueId, long componenteCurricularId);
    }
}