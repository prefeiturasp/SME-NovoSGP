using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoReabertura
    {
        Task Alterar(FechamentoReabertura fechamentoReabertura, DateTime dataInicialAnterior, DateTime dataFimAnterior);

        Task Salvar(FechamentoReabertura fechamentoReabertura);
    }
}