using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseNota : IRepositorioBase<ConselhoClasseNota>
    {
        Task<ConselhoClasseNota> ObterPorConselhoClasseAlunoComponenteCurricular(long conselhoClasseAlunoId, long componenteCurricularCodigo);
    }
}