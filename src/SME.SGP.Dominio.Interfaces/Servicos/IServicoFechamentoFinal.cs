using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoFinal
    {
        Task<List<string>> SalvarAsync(FechamentoTurmaDisciplina fechamentoFinal, Turma turma);

        Task VerificaPersistenciaGeral(Turma turma);
    }
}