using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurmaConsulta
    {
        Task<Turma> ObterPorCodigo(string turmaCodigo);
        Task<string> ObterTurmaCodigoPorConselhoClasseId(long conselhoClasseId);
    }
}