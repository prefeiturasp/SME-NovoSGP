using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioGrade : IRepositorioBase<Grade>
    {
        Task<Grade> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao);

        Task<int> ObterHorasComponente(long gradeId, long componenteCurricular, int ano);
    }
}