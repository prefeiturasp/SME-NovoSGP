using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoFinal
    {
        Task SalvarAsync(FechamentoFinal fechamentoFinal);

        Task VerificaPersistenciaGeral(Turma turma);
    }
}