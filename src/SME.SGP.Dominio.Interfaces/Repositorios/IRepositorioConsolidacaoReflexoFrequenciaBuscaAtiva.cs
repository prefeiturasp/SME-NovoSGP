using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoReflexoFrequenciaBuscaAtiva : IRepositorioBase<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno>
    {
        Task ExcluirConsolidacoes(string ueCodigo, int mes, int anoLetivo);
        Task<ConsolidacaoReflexoFrequenciaBuscaAtivaAluno> ObterIdConsolidacao(string turmaCodigo, string alunoCodigo, int mes, int anoLetivo);
    }
}
