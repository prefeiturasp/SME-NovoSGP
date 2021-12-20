using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequencia : IRepositorioBase<RegistroFrequencia>
    {
        Task ExcluirFrequenciaAula(long aulaId);
        Task SalvarConciliacaoTurma(string turmaId, string disciplinaId, DateTime dataReferencia, string alunos);
    }
}