using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasseAluno
    {
        Task<ConselhoClasseAluno> ObterPorConselhoClasseAsync(long conselhoClasseId, string alunoCodigo);
        Task<bool> ExisteConselhoClasseUltimoBimestreAsync(Turma turma, string alunoCodigo);
        Task<ParecerConclusivoDto> ObterParecerConclusivo(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma);
        Task<IEnumerable<ConselhoDeClasseGrupoMatrizDto>> ObterListagemDeSinteses(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre);
        Task<ConselhoClasseAlunoNotasConceitosRetornoDto> ObterNotasFrequencia(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre, bool exibirHistorico = false);
        Task<List<string>> ObterTurmasComMatriculasValidas(string alunoCodigo, string[] turmasCodigos, DateTime periodoInicio, DateTime periodoFim);
    }
}
