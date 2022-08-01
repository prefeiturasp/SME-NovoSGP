using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoConselhoClasse
    {
        Task<ConselhoClasseNotaRetornoDto> SalvarConselhoClasseAlunoNotaAsync(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId, string codigoTurma, int bimestre);
        Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId, bool? historico = false);
    }

    public struct RetornoConsolidado
    {
        public Dictionary<string, int> TotaldeAlterados;
        public List<objConsolidacaoConselhoAluno> listaObjErros;
    }
}