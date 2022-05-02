using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaTurmaEvasao
    {
        Task<long> Inserir(FrequenciaTurmaEvasao frequenciaTurmaEvasao);
        Task LimparFrequenciaTurmaEvasaoPorTurmasEMeses(long[] turmasIds, int[] meses);
    }
}
