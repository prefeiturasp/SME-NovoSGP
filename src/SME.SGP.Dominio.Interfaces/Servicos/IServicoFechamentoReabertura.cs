using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoReabertura
    {
        Task<string> Alterar(FechamentoReabertura fechamentoReabertura, DateTime dataInicialAnterior, DateTime dataFimAnterior);

        Task<string> Salvar(FechamentoReabertura fechamentoReabertura);
    }
}