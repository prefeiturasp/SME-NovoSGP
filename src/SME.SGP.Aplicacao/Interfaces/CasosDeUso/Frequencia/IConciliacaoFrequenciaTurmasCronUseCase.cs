using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConciliacaoFrequenciaTurmasCronUseCase
    {
        Task Executar();
        Task ProcessarNaData(DateTime dataPeriodo, string turmaCodigo);
    }
}
