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
        Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string turmaId, IEnumerable<string> alunos);
        Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> ObterFrequenciaAlunosGeralPorAnoQuery(int ano);
        Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId);

        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId);

        Task<bool> InserirVarios(IEnumerable<RegistroFrequenciaAluno> registros);
        Task ExcluirVarios(List<long> idsParaExcluir);
    }
}
