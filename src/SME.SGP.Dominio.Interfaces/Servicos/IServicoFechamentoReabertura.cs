using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoReabertura
    {
        Task<string> AlterarAsync(FechamentoReabertura fechamentoReabertura, int[] bimestresPropostos);

        Task<string> ExcluirAsync(FechamentoReabertura fechamento);

        Task<string> SalvarAsync(FechamentoReabertura fechamentoReabertura);
    }
}