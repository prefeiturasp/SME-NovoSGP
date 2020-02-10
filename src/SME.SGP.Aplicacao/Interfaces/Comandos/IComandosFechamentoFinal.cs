using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoFinal
    {
        Task<string[]> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto);
    }
}