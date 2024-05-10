using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioConselhoClasseConsolidado : IRepositorioBase<ConselhoClasseConsolidadoTurmaAluno>
    {
        Task<bool> ExcluirLogicamenteConsolidacaoConselhoClasseAlunoTurmaPorIdConsolidacao(long[] idsConsolidacoes);
    }
}
