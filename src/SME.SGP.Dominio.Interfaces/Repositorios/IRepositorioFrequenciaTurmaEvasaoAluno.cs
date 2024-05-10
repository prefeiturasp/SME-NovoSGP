using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaTurmaEvasaoAluno
    {
        Task<long> Inserir(FrequenciaTurmaEvasaoAluno frequenciaTurmaEvasaoAluno);
        Task LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMeses(long[] turmasIds, int[] meses);
    }
}
