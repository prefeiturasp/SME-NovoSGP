using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase
    {
        Task<IEnumerable<int>> Executar(string disciplinaId, string codigoTurma, int bimestre, DateTime dataMatricula);
    }
}
