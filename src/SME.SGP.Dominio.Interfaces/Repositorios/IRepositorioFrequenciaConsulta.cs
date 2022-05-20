﻿using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaConsulta : IRepositorioBase<RegistroFrequencia>
    {
        Task<bool> FrequenciaAulaRegistrada(long aulaId);
        RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId);
        IEnumerable<AulasPorTurmaDisciplinaDto> ObterAulasSemRegistroFrequencia(string turmaId, string disciplinaId, TipoNotificacaoFrequencia tipoNotificacao);
        Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos);
        Task<IEnumerable<RecuperacaoParalelaFrequenciaDto>> ObterFrequenciaAusencias(string[] CodigoAlunos, string CodigoDisciplina, int Ano, PeriodoRecuperacaoParalela Periodo);
        Task<IEnumerable<AulaComFrequenciaNaDataDto>> ObterAulasComRegistroFrequenciaPorTurma(string turmaCodigo);
        Task<IEnumerable<AlunoComponenteCurricularDto>> ObterAlunosAusentesPorTurmaEPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId);
        Task<IEnumerable<RegistroAusenciaAluno>> ObterListaFrequenciaPorAula(long aulaId);
        Task<RegistroFrequencia> ObterRegistroFrequenciaPorAulaId(long aulaId);
        IEnumerable<AlunosFaltososDto> ObterAlunosFaltosos(DateTime dataReferencia, long tipoCalendarioId);
        Task<IEnumerable<AusenciaMotivoDto>> ObterAusenciaMotivoPorAlunoTurmaBimestreAno(string codigoAluno, string turma, short bimestre, short anoLetivo);
        Task<IEnumerable<GraficoBaseDto>> ObterDashboardFrequenciaAusenciasPorMotivo(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, string ano, long turmaId, int semestre);
        Task<IEnumerable<string>> ObterTurmasCodigosFrequenciasExistentesPorAnoAsync(int[] anosLetivos);        
        Task<IEnumerable<TotalFrequenciaEAulasPorPeriodoDto>> ObterTotalFrequenciaEAulasPorPeriodo(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard);
        Task<IEnumerable<RegistroFrequencia>> ObterRegistroFrequenciaPorDataEAulaId(string disciplina, string turmaId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<RegistroFrequenciaAlunoPorAulaDto>> ObterFrequenciasDetalhadasPorData(string turmaCodigo, string componenteCurricularId, string[] codigosAlunos, DateTime dataInicio, DateTime dataFim);
        Task<bool> RegistraFrequencia(long componenteCurricularId);
        Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(int anoLetivo, string dreCodigo, string ueCodigo, Modalidade modalidade, int semestre, int mes);
        Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoSemPresenca(int anoLetivo, string dreCodigo, string ueCodigo, Modalidade modalidade, int semestre, int mes);
    }
}