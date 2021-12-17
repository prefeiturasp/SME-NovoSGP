using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaDisciplinaConsulta : IRepositorioBase<FechamentoTurmaDisciplina>
    {
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId, long[] disciplinasId, int bimestre = 0);

        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(string turmaCodigo, long[] disciplinasId, int bimestre = 0);

        Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaCodigo, long disciplinaId, int bimestre = 0);

        Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno, long fechamentoTurmaId);
        Task<SituacaoFechamento> ObterSituacaoFechamento(long turmaId, long componenteCurricularId, long periodoEscolarId);
        Task<IEnumerable<TurmaFechamentoDisciplinaDto>> ObterTotalDisciplinasPorTurma(int anoLetivo, int bimestre);
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(int anoLetivo);
        Task<IEnumerable<int>> ObterDisciplinaIdsPorTurmaIdBimestre(long turmaId, int bimestre);
        Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamento(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamentoPorEstudante(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);        
        Task<IEnumerable<FechamentoPendenciaQuantidadeDto>> ObterSituacaoPendenteFechamento(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<long>> ObterFechamentosTurmaDisciplinaEmDuplicidade(DateTime dataInicio);
        Task<IEnumerable<(long fechamentoTurmaDisciplinaId, long periodoEscolarId, string codigoRf)>> ObterFechamentosTurmaDisciplinaEmProcessamentoComTempoExpirado(DateTime dataInicio, int tempoConsideradoExpiracaoMinutos);
    }
}