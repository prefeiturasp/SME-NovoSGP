using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAluno : IRepositorioBase<RegistroAusenciaAluno>
    {
        bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId);
        
        AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoETurma(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId);
        
        Task<AusenciaPorDisciplinaDto> ObterTotalAusenciasPorAlunoETurmaAsync(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId);

        Task<IEnumerable<RegistroAusenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId);
        Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterTotalAusenciasPorAlunosETurmaAsync(DateTime dataAula, IEnumerable<string> alunos,  string turmaId);
        Task SalvarVarios(List<RegistroAusenciaAluno> ausenciasParaAdicionar);
        Task ExcluirVarios(List<long> idsParaExcluir);
    }
}