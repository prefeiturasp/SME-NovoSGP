using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoTurma : IRepositorioBase<AcompanhamentoTurma>
    {
        Task<AcompanhamentoTurma> ObterApanhadoGeralPorTurmaIdESemestre(long turmaId, int semestre);
    }
}
