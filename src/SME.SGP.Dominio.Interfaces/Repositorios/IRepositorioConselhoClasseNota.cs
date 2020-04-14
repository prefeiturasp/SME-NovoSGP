using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseNota : IRepositorioBase<ConselhoClasseNota>
    {
        Task<ConselhoClasseNota> ObterPorFiltrosAsync(string alunoCodigo, int bimestre, string componenteCurricularCodigo, string turmaCodigo);
    }
}