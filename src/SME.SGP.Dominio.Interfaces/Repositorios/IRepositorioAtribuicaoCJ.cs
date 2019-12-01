using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAtribuicaoCJ : IRepositorioBase<AtribuicaoCJ>
    {
        Task<AtribuicaoCJ> ObterPorComponenteTurmaModalidadeUe(Modalidade modalidade, string turmaId, string ueId, long componenteCurricularId);
    }
}