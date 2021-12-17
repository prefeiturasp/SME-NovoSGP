using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaDisciplina : IRepositorioBase<FechamentoTurmaDisciplina>
    {
       Task<bool> AtualizarSituacaoFechamento(long fechamentoTurmaDisciplinaId, int situacaoFechamento);
       Task<bool> ExcluirLogicamenteFechamentosTurmaDisciplina(long[] idsFechamentoTurmaDisciplina);
    }
}