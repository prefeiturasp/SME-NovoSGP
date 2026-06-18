using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidado : IRepositorioBase<ConselhoClasseConsolidadoTurmaAluno>
    {
        Task<bool> ExcluirLogicamenteConsolidacaoConselhoClasseAlunoTurmaPorIdConsolidacao(long[] idsConsolidacoes);
    }
}
