using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalAulasSemFrequenciaPorTurmaUseCase
    {
        Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Executar(string disciplinaId, string codigoTurma);
    }
}
