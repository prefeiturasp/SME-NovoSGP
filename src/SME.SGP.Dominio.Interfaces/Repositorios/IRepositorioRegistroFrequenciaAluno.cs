using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioRegistroFrequenciaAluno : IRepositorioBase<RegistroFrequenciaAluno>
    {
        Task RemoverPorRegistroFrequenciaId(long registroFrequenciaId);
        Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId);

        Task<bool> InserirVarios(IEnumerable<RegistroFrequenciaAluno> registros);
    }
}
