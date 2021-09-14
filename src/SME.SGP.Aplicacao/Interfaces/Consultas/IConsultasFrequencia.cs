using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFrequencia
    {
        Task<bool> FrequenciaAulaRegistrada(long aulaId);

        Task<IEnumerable<AlunoAusenteDto>> ObterListaAlunosComAusencia(string turmaId, string disciplinaId, int bimestre);

        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual);

        Task<SinteseDto> ObterSinteseAluno(double? percentualFrequencia, DisciplinaDto disciplina);

        Task<double> ObterFrequenciaMedia(DisciplinaDto disciplina);

        Task<double?> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "", int? semestre = null);
        Task<FrequenciaAluno> ObterFrequenciaGeralAlunoPorTurmaEComponente(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "");
        Task<IEnumerable<AusenciaMotivoDto>> ObterAusenciaMotivoPorAlunoTurmaBimestreAno(string codigoAluno, string codigoTurma, short bimestre, short ano);
    }
}