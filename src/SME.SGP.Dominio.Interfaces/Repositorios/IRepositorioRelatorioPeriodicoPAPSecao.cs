using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRelatorioPeriodicoPAPSecao : IRepositorioBase<RelatorioPeriodicoPAPSecao>
    {
        Task<RelatorioPeriodicoPAPSecao> ObterSecoesComQuestoes(long id);
        Task<long?> ObterIdSecaoAtiva(long relatorioAlunoId, long secaoRelatorioPeriodicoId);
    }
}
