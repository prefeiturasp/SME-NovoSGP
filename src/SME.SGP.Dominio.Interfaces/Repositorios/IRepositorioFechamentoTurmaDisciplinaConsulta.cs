using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elastic.Apm.Api;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaDisciplinaConsulta : IRepositorioBase<FechamentoTurmaDisciplina>
    {
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId, long[] disciplinasId, int bimestre = 0, long? tipoCalendario = null);

        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(string turmaCodigo, long[] disciplinasId, int bimestre = 0);

        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaCodigo, long disciplinaId, int bimestre = 0);
        Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId);
        Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestrePorCodigosAlunosIdsFechamentos(string[] codigoAluno, long[] fechamentoTurmaId);
        Task<SituacaoFechamento> ObterSituacaoFechamento(long turmaId, long componenteCurricularId, long periodoEscolarId);
        Task<IEnumerable<TurmaFechamentoDisciplinaSituacaoDto>> ObterFechamentosTurmaPorTurmaId(long turmaId);
        Task<IEnumerable<TurmaFechamentoDisciplinaDto>> ObterTotalDisciplinasPorTurma(int anoLetivo, int bimestre);
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(int anoLetivo);
        Task<IEnumerable<int>> ObterDisciplinaIdsPorTurmaIdBimestre(long turmaId, int bimestre);
        Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamento(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamentoPorEstudante(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<FechamentoPendenciaQuantidadeDto>> ObterSituacaoPendenteFechamento(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<long>> ObterFechamentosTurmaDisciplinaEmDuplicidade(DateTime dataInicio);
        Task<IEnumerable<(long fechamentoTurmaDisciplinaId, long periodoEscolarId, string codigoRf)>> ObterFechamentosTurmaDisciplinaEmProcessamentoComTempoExpirado(DateTime dataInicio, int tempoConsideradoExpiracaoMinutos);
        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplinaPorId(long id);
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentoTurmaDisciplinaPorTurmaidDisciplinaId(string turmaCodigo, long disciplinaId, int? bimestre = 0);
        Task<FechamentoTurmaDisciplinaPendenciaDto> ObterFechamentoTurmaDisciplinaDTOPorTurmaDisciplinaBimestre(string turmaCodigo, long disciplinaId, int bimestre, SituacaoFechamento[] situacoesFechamento);
        Task<bool> VerificaExistenciaFechamentoTurmaDisciplinPorTurmaDisciplinaBimestreSituacao(long turmaId, long disciplinaId, long periodoId, SituacaoFechamento[] situacoesFechamento);
        Task<IEnumerable<FechamentoTurmaDisciplinaPendenciaDto>> ObterFechamentosTurmaDisciplinaDTOPorUeSituacao(long idUe, SituacaoFechamento[] situacoesFechamento, long[] fechamentoTurmaDisciplinaIdsIgnorados = null);
        Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterFechamentosTurmasCodigosEBimestreEAlunoCodigoAsync(string[] turmasCodigos, int bimestre, string alunoCodigo);
    }
}