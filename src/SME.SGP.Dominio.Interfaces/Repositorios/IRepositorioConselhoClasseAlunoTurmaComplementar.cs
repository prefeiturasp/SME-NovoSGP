using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConselhoClasseAlunoTurmaComplementar
    {
        Task Inserir(long conselhoClasseAlunoId, long turmaId);

        Task<bool> VerificarSeExisteRegistro(long conselhoClasseAlunoId, long turmaId);
    }
}
