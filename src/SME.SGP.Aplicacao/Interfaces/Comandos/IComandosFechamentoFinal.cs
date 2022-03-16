using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoFinal
    {
        Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto);
    }
}