using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamentoTurmaDisciplina
    {
        Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId);
        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int bimestre);
        Task<FechamentoTurmaDisciplinaBimestreDto> ObterNotasFechamentoTurmaDisciplina(string turmaId, long disciplinaId, int? bimestre, int semestre);
        Task<IEnumerable<AlunoDadosBasicosDto>> ObterDadosAlunos(string turmaCodigo, int anoLetivo);
    }
}
