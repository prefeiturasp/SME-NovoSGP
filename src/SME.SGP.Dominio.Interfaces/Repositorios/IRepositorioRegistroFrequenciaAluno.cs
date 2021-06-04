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
        Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string turmaId, IEnumerable<string> alunos);
        Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId);
        Task<IEnumerable<FiltroMigracaoFrequenciaAulasDto>> ObterTurmasIdFrequenciasExistentesPorAnoAsync(int[] anosLetivos);
        Task<bool> ExisteRegistroFrequenciaAlunoAsync(long registroFrequenciaId, string codigoAluno, int numeroAula);
    }
}
