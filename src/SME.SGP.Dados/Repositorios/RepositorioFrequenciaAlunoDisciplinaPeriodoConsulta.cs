﻿using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta : IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public FrequenciaAluno Obter(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId)
        {
            var query = BuildQueryObter();
            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                periodoEscolarId,
                tipoFrequencia,
                turmaId
            });
        }

        public async Task<FrequenciaAluno> ObterAsync(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId)
        {
            var query = BuildQueryObter();
            return await database.QueryFirstOrDefaultAsync<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                periodoEscolarId,
                tipoFrequencia,
                turmaId
            });
        }

        private static string BuildQueryObter()
        {
            var query = @"select
	                        *
                        from
	                        frequencia_aluno
                        where
	                        codigo_aluno = @codigoAluno
	                        and disciplina_id = @disciplinaId
	                        and tipo = @tipoFrequencia
	                        and periodo_escolar_id = @periodoEscolarId
                            and turma_id = @turmaId";
            return query;
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunos(IEnumerable<string> alunosCodigo, IEnumerable<long?> periodosEscolaresId, string turmaId, string disciplinaId)
        {
            var query = new StringBuilder(@"with lista as (
                        select *,
                               row_number() over (partition by codigo_aluno, bimestre order by id desc) sequencia
                            from
	                            frequencia_aluno
                        where
	                        codigo_aluno = any(@alunosCodigo)	                        	                        
                            and turma_id = @turmaId
                            and (disciplina_id = @disciplinaId or tipo = @tipo)");

            if (periodosEscolaresId != null && periodosEscolaresId.Any())
                query.AppendLine("and periodo_escolar_id = any(@periodosEscolaresId)");

            query.AppendLine(") select * from lista where sequencia = 1;");

            return await database.QueryAsync<FrequenciaAluno>(query.ToString(), new
            {
                alunosCodigo,
                periodosEscolaresId = periodosEscolaresId.ToArray(),
                turmaId,
                disciplinaId,
                tipo = TipoFrequenciaAluno.Geral
            });
        }

        public IEnumerable<FrequenciaAluno> ObterAlunosComAusenciaPorDisciplinaNoPeriodo(long periodoId, bool eja)
        {
            var query = $@"select f.* 
                          from frequencia_aluno f
                         inner join turma t on t.turma_id = f.turma_id
                        where not f.excluido
                          and f.periodo_escolar_id = @periodoId
                          and f.tipo = 1
                          and f.total_ausencias - f.total_compensacoes > 0 "
                        + (eja ? " and t.modalidade_codigo = 3" : " and t.modalidade_codigo <> 3");


            return database.Conexao.Query<FrequenciaAluno>(query, new { periodoId });
        }

        public IEnumerable<AlunoFaltosoBimestreDto> ObterAlunosFaltososBimestre(ModalidadeTipoCalendario modalidade, double percentualFrequenciaMinimo, int bimestre, int? anoLetivo)
        {
            var query = new StringBuilder();

            query.AppendLine("select dre.dre_id as DreCodigo, dre.Abreviacao as DreAbreviacao, dre.Nome as DreNome, ue.tipo_escola as TipoEscola, ue.ue_id as UeCodigo, ue.nome as UeNome");
            query.AppendLine(", t.turma_id as TurmaCodigo, t.nome as TurmaNome, t.modalidade_codigo as TurmaModalidade, fa.codigo_aluno as AlunoCodigo");
            query.AppendLine(", ((fa.total_ausencias::numeric - fa.total_compensacoes::numeric ) / fa.total_aulas::numeric)*100 PercentualFaltas");
            query.AppendLine("from frequencia_aluno fa");
            query.AppendLine("inner join turma t on t.turma_id = fa.turma_id");
            query.AppendLine("inner join ue on ue.id = t.ue_id ");
            query.AppendLine("inner join dre on dre.id = ue.dre_id");
            query.AppendLine("inner join (select codigo_aluno, max(id) as maiorId from frequencia_aluno where not excluido group by codigo_aluno ) m on m.codigo_aluno = fa.codigo_aluno and m.maiorId = fa.id");
            query.AppendLine("where fa.tipo = 2");
            query.AppendLine("and fa.bimestre = @bimestre");

            if (anoLetivo.HasValue)
                query.AppendLine("and extract(year from fa.periodo_inicio) = @anoLetivo");

            query.AppendLine("and ((fa.total_ausencias::numeric - fa.total_compensacoes::numeric ) / fa.total_aulas::numeric) > (1 -(@percentualFrequenciaMinimo::numeric / 100::numeric)) ");

            if (modalidade == ModalidadeTipoCalendario.EJA)
                query.AppendLine("and t.modalidade_codigo = 3");
            else if (modalidade == ModalidadeTipoCalendario.Infantil)
                query.AppendLine("and t.modalidade_codigo = 1");
            else query.AppendLine("and t.modalidade_codigo in (5,6)");


            return database.Conexao.Query<AlunoFaltosoBimestreDto>(query.ToString(), new { bimestre, anoLetivo, percentualFrequenciaMinimo });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "")
        {
            var query = new StringBuilder($@"select * 
                            from frequencia_aluno
                           where tipo = {(string.IsNullOrWhiteSpace(componenteCurricularCodigo) ? "2" : "1")}
	                        and codigo_aluno = @alunoCodigo
                            and turma_id = @turmaCodigo ");

            if (!string.IsNullOrEmpty(componenteCurricularCodigo))
                query.AppendLine(" and disciplina_id = @componenteCurricularCodigo");

            return await database.Conexao
                .QueryAsync<FrequenciaAluno>(query.ToString(), new
                {
                    alunoCodigo,
                    turmaCodigo,
                    componenteCurricularCodigo
                });
        }
        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaBimestresAsync(string codigoAluno, int bimestre, string codigoTurma, TipoFrequenciaAluno tipoFrequencia = TipoFrequenciaAluno.PorDisciplina)
        {
            var query = @"select * 
                            from frequencia_aluno fa 
                           where fa.codigo_aluno = @codigoAluno
                             and fa.turma_id = @turmaId 
                             and fa.tipo = @tipoFrequencia";

            if (bimestre > 0)
                query += " and fa.bimestre = @bimestre";

            var parametros = new
            {
                codigoAluno,
                bimestre,
                turmaId = codigoTurma,
                tipoFrequencia,
            };

            return await database.Conexao.QueryAsync<FrequenciaAluno>(query, parametros);
        }

        public async Task<FrequenciaAluno> ObterPorAlunoBimestreAsync(string codigoAluno, int bimestre, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string disciplinaId = "")
        {
            var query = new StringBuilder(@"select *
                        from frequencia_aluno
                        where codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia
	                        and bimestre = @bimestre 
                            and turma_id = @codigoTurma ");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and disciplina_id = @disciplinaId");

            query.AppendLine(" order by id desc");
            return await database.Conexao.QueryFirstOrDefaultAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                bimestre,
                tipoFrequencia,
                disciplinaId,
                codigoTurma
            });
        }

        private static string BuildQueryObterPorAlunoData(string disciplinaId = "", string codigoTurma = "")
        {
            var query = new StringBuilder(@"select fa.*
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where
	                        codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia                            
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual ");

            if (!string.IsNullOrWhiteSpace(codigoTurma))
                query.AppendLine("and turma_id = @codigoTurma");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and disciplina_id = @disciplinaId");

            return query.ToString();
        }

        public FrequenciaAluno ObterPorAlunoData(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "")
        {
            String query =
                BuildQueryObterPorAlunoData(disciplinaId, codigoTurma);

            return database.QueryFirstOrDefault<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                dataAtual,
                tipoFrequencia,
                disciplinaId,
                codigoTurma
            });
        }

        public async Task<FrequenciaAluno> ObterPorAlunoDataAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "")
        {
            string query = BuildQueryObterPorAlunoData(disciplinaId, codigoTurma);

            return await database.QueryFirstOrDefaultAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                dataAtual,
                tipoFrequencia,
                disciplinaId,
                codigoTurma
            });
        }

        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual, string turmaCodigo)
        {
            var query = @"select *
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where codigo_aluno = @codigoAluno
                            and disciplina_id = @disciplinaId
	                        and tipo = 1
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual ";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += "and fa.turma_id = @turmaCodigo";

            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                dataAtual,
                turmaCodigo
            });
        }
        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaPorListaDeAlunosDisciplinaData(string[] codigosAlunos, string disciplinaId, DateTime dataAtual, string turmaCodigo)
        {
            var query = @"select *
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where codigo_aluno = any(@codigosAlunos)
                            and disciplina_id = @disciplinaId
	                        and tipo = 1
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual ";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += "and fa.turma_id = @turmaCodigo";

            return await database.QueryAsync<FrequenciaAluno>(query, new
            {
                codigosAlunos,
                disciplinaId,
                dataAtual,
                turmaCodigo
            });
        }

        public async Task<FrequenciaAluno> ObterPorAlunoDisciplinaDataAsync(string codigoAluno, string disciplinaId, DateTime dataAtual, string turmaCodigo)
        {
            var query = @"select *
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where codigo_aluno = @codigoAluno
                            and disciplina_id = @disciplinaId
	                        and tipo = 1
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual ";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += "and fa.turma_id = @turmaCodigo";

            return await database.QueryFirstOrDefaultAsync<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                dataAtual,
                turmaCodigo
            });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolar(string codigoTurma, string componenteCurricularId, TipoFrequenciaAluno tipoFrequencia, IEnumerable<long> periodosEscolaresIds)
        {
            const string sql = @"select *
	                                from (     
		                                select 
			                                    fa.*,
			                                    row_number() over (partition by fa.id, fa.codigo_aluno, fa.bimestre, fa.disciplina_id order by fa.id desc) sequencia
			                                from 
			                                    frequencia_aluno fa 
		                                    where
                                                turma_id = @codigoTurma and 
                                                disciplina_id = @componenteCurricularId and 
                                                tipo = @tipoFrequencia and
                                                periodo_escolar_id = any(@periodosEscolaresIds)
		                                )rf
	                                where rf.sequencia = 1";

            var parametros = new { codigoTurma, componenteCurricularId, tipoFrequencia = (short)tipoFrequencia, periodosEscolaresIds = periodosEscolaresIds.ToList() };
            return await database.Conexao.QueryAsync<FrequenciaAluno>(sql, parametros);
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorAnoModalidadeSemestre(string alunoCodigo, int anoTurma, long tipoCalendarioId)
        {
            var query = new StringBuilder($@"select fa.* 
                            from frequencia_aluno fa
                            inner join turma t on fa.turma_id = t.turma_id ");

            if (tipoCalendarioId > 0)
                query.AppendLine("inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id");

            query.AppendLine(@" where fa.tipo = 2 
                and fa.codigo_aluno = @alunoCodigo 
                and t.ano_letivo = @anoTurma 
                and t.tipo_turma in(1,2,7) ");

            if (tipoCalendarioId > 0)
                query.AppendLine(" and pe.tipo_calendario_id = @tipoCalendarioId");

            return await database.Conexao
                .QueryAsync<FrequenciaAluno>(query.ToString(), new
                {
                    alunoCodigo,
                    anoTurma,
                    tipoCalendarioId
                });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaComponentesAlunoPorTurmas(string alunoCodigo, string[] codigosTurmas, long tipoCalendarioId, int bimestre = 0)
        {
            var query = new StringBuilder($@"select fa.* 
                            from frequencia_aluno fa
                            inner join turma t on fa.turma_id = t.turma_id ");

            if (tipoCalendarioId > 0)
                query.AppendLine("inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id");

            query.AppendLine(@" where fa.tipo = 1 
                and fa.codigo_aluno = @alunoCodigo 
                and t.turma_id = any(@codigosTurmas)
                and t.tipo_turma in(1,2,7) ");

            if (bimestre > 0)
                query.AppendLine(" and fa.bimestre = @bimestre ");

            if (tipoCalendarioId > 0)
                query.AppendLine(" and pe.tipo_calendario_id = @tipoCalendarioId ");

            query.AppendLine(" order by alterado_em desc ");

            return await database.Conexao
                .QueryAsync<FrequenciaAluno>(query.ToString(), new
                {
                    alunoCodigo,
                    codigosTurmas,
                    tipoCalendarioId,
                    bimestre
                });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorTurmas(string alunoCodigo, string[] codigosTurmas, long tipoCalendarioId)
        {
            var query = new StringBuilder($@"with lista as (
                            select fa.*,
                                   row_number() over (partition by fa.bimestre order by fa.id desc) sequencia
                            from frequencia_aluno fa
                            inner join turma t on fa.turma_id = t.turma_id ");

            if (tipoCalendarioId > 0)
                query.AppendLine("inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id");

            query.AppendLine(@" where fa.tipo = 2 
                and fa.codigo_aluno = @alunoCodigo 
                and t.turma_id = any(@codigosTurmas)
                and t.tipo_turma in(1,2,7) ");

            if (tipoCalendarioId > 0)
                query.AppendLine(" and pe.tipo_calendario_id = @tipoCalendarioId");

            query.AppendLine(") select * from lista where sequencia = 1;");

            return await database.Conexao
                .QueryAsync<FrequenciaAluno>(query.ToString(), new
                {
                    alunoCodigo,
                    codigosTurmas,
                    tipoCalendarioId
                });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaAsync(string alunoCodigo, string[] turmasCodigos, string[] componenteCurricularCodigos)
        {
            var query = $@"select * 
                            from frequencia_aluno
                           where tipo = 1
	                        and codigo_aluno = @alunoCodigo
                            and turma_id = ANY(@turmasCodigos)
                            and disciplina_id = ANY(@componenteCurricularCodigos)";

            return await database.Conexao
                .QueryAsync<FrequenciaAluno>(query.ToString(), new
                {
                    alunoCodigo,
                    turmasCodigos,
                    componenteCurricularCodigos
                });
        }
        public async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaGeralPorTurma(string turmaCodigo)
        {
            var query = @"with lista as (
                          select fa.codigo_aluno,
                                 fa.periodo_fim,
	                             fa.total_aulas,
	                             fa.total_ausencias,
	                             fa.total_compensacoes,
	                             row_number() over (partition by fa.codigo_aluno, fa.bimestre order by fa.id desc) sequencia
	                          from frequencia_aluno fa
                          where fa.turma_id = @turmaCodigo and
	                          fa.tipo = 2 and
	                          not fa.excluido)
                          select codigo_aluno as AlunoCodigo,
                                 periodo_fim PeriodoFim,
	                             total_aulas as TotalAulas, 
	                             total_ausencias as TotalAusencias, 
	                             total_compensacoes as TotalCompensacoes
                          from lista
                          where sequencia = 1;";

            return await database.Conexao.QueryAsync<FrequenciaAlunoDto>(query, new { turmaCodigo });
        }
        public async Task<FrequenciaAluno> ObterPorAlunoDataTurmasAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string[] turmasCodigo, string disciplinaId = "")
        {
            var query = new StringBuilder(@"select fa.*
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where
	                        codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia                            
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual ");

            if (turmasCodigo.Length > 0)
                query.AppendLine("and turma_id = ANY(turmasCodigo)");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and disciplina_id = @disciplinaId");

            return await database.QueryFirstOrDefaultAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                dataAtual,
                tipoFrequencia,
                disciplinaId,
                turmasCodigo
            });
        }
        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmasDisciplinasDataAsync(string codigoAluno, TipoFrequenciaAluno tipoFrequencia, string[] disciplinasId, string[] turmasCodigo, int[] bimestres, long[] periodosEscolaresId = null)
        {
            var query = new StringBuilder(@"select * 
	                                        from (select fa.*,
				                                         row_number() over (partition by fa.bimestre, fa.disciplina_id order by fa.id desc) sequencia
          	                                        from frequencia_aluno fa
            	                                        inner join periodo_escolar pe 
            		                                        on fa.periodo_escolar_id = pe.id
	                                              where not fa.excluido
                                                    and codigo_aluno = @codigoAluno
	       	                                        and tipo = @tipoFrequencia
	                                                and turma_id = ANY(@turmasCodigo)
	                                                and disciplina_id = ANY(@disciplinasId)");

            if (periodosEscolaresId != null)
                query.AppendLine(" and fa.periodo_escolar_id = ANY(@periodosEscolaresId)");

            query.AppendLine(") rf where rf.sequencia = 1");

            if (bimestres.Length > 0)
                query.AppendLine(" and rf.bimestre = ANY(@bimestres)");

            return await database.QueryAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                tipoFrequencia,
                disciplinasId,
                turmasCodigo,
                bimestres,
                periodosEscolaresId
            });
        }

        public async Task<bool> ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(string codigoTurma, string componenteCurricularId, long periodoEscolarId)
        {
            const string sql = @"select distinct(1)
                                  from registro_frequencia_aluno rfa
                                  inner join registro_frequencia rf on rfa.registro_frequencia_id = rf.id     
                                  inner join aula a on a.id = rf.aula_id 
                                  inner join tipo_calendario tc on tc.id = a.tipo_calendario_id
                                  inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                                  where pe.id = @periodoEscolarId
                                    and not rf.excluido
                                    and not rfa.excluido
                                    and a.turma_id = @codigoTurma
                                    and a.disciplina_id = @componenteCurricularId
                                    and a.data_aula between pe.periodo_inicio and pe.periodo_fim ";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { codigoTurma, componenteCurricularId, periodoEscolarId });
        }
        public async Task<bool> ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestres(string codigoTurma, string componenteCurricularId, long[] periodosEscolaresIds)
        {
            const string sql = @"select 1
                                   from registro_frequencia_aluno rfa
                                  inner join registro_frequencia rf on rfa.registro_frequencia_id = rf.id
                                  inner join aula a on a.id = rf.aula_id 
                                  inner join tipo_calendario tc on tc.id = a.tipo_calendario_id
                                  inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                                  where pe.id = ANY(@periodosEscolaresIds)
                                    and a.turma_id = @codigoTurma
                                    and a.disciplina_id = @componenteCurricularId
                                    and a.data_aula between pe.periodo_inicio and pe.periodo_fim 
                                limit 1";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql, new { codigoTurma, componenteCurricularId, periodosEscolaresIds });
        }
        private String BuildQueryObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, string turmaId)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("select ");
            query.AppendLine("COALESCE(SUM(a.quantidade),0) AS total");
            query.AppendLine("from ");
            query.AppendLine("aula a ");
            query.AppendLine("inner join registro_frequencia rf on ");
            query.AppendLine("rf.aula_id = a.id ");
            query.AppendLine("inner join periodo_escolar p on ");
            query.AppendLine("a.tipo_calendario_id = p.tipo_calendario_id ");
            query.AppendLine("where not a.excluido");
            query.AppendLine("and p.periodo_inicio <= @dataAula ");
            query.AppendLine("and p.periodo_fim >= @dataAula ");
            query.AppendLine("and a.data_aula >= p.periodo_inicio");
            query.AppendLine("and a.data_aula <= p.periodo_fim ");

            if (!string.IsNullOrWhiteSpace(disciplinaId))
                query.AppendLine("and a.disciplina_id = @disciplinaId ");

            query.AppendLine("and a.turma_id = @turmaId ");
            return query.ToString();
        }

        public async Task<int> ObterTotalAulasPorDisciplinaETurmaAsync(DateTime dataAula, string disciplinaId, string turmaId)
        {
            String query = BuildQueryObterTotalAulasPorDisciplinaETurma(dataAula, disciplinaId, turmaId);
            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { dataAula, disciplinaId, turmaId });
        }

        public async Task<IEnumerable<TurmaComponenteQntAulasDto>> ObterTotalAulasPorDisciplinaETurmaEBimestre(string[] turmasCodigo, string[] componentesCurricularesId, long tipoCalendarioId, int[] bimestres, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null)
        {
            var query = new StringBuilder();
            query.AppendLine(@"select a.disciplina_id as ComponenteCurricularCodigo, a.turma_id as TurmaCodigo, 
                               p.bimestre as Bimestre, p.periodo_inicio as PeriodoInicio, p.periodo_fim as PeriodoFim,
                               COALESCE(SUM(a.quantidade),0) AS AulasQuantidade from 
                                    aula a 
                                    inner join registro_frequencia_aluno rfa on 
                                    rfa.aula_id = a.id 
                                    inner join periodo_escolar p on 
                                    a.tipo_calendario_id = p.tipo_calendario_id 
                                    where not a.excluido 
                                    and not rfa.excluido
                                    and a.tipo_calendario_id = @tipoCalendarioId
                                    and a.data_aula::date between p.periodo_inicio and p.periodo_fim ");

            if (componentesCurricularesId.Length > 0)
                query.AppendLine("and a.disciplina_id = any(@componentesCurricularesId) ");
            if (bimestres.Length > 0)
                query.AppendLine(" and p.bimestre = any(@bimestres) ");

            if (dataMatriculaAluno.HasValue && dataSituacaoAluno.HasValue)
                query.AppendLine("and a.data_aula::date between @dataMatriculaAluno and @dataSituacaoAluno");
            else if (dataMatriculaAluno.HasValue)
                query.AppendLine("and a.data_aula::date >= @dataMatriculaAluno");
            else if (dataSituacaoAluno.HasValue)
                query.AppendLine("and a.data_aula::date < @dataSituacaoAluno");

            query.AppendLine(" and a.turma_id = any(@turmasCodigo) group by a.disciplina_id, a.turma_id, p.bimestre, p.periodo_inicio, p.periodo_fim");

            return await database.Conexao.QueryAsync<TurmaComponenteQntAulasDto>(query.ToString(),
           new { turmasCodigo, componentesCurricularesId, tipoCalendarioId, bimestres, dataMatriculaAluno, dataSituacaoAluno });
        }

        public async Task<IEnumerable<TurmaDataAulaComponenteQtdeAulasDto>> ObterAulasPorDisciplinaETurmaEBimestre(string[] turmasCodigo, string[] codigosAluno, string[] componentesCurricularesId, long tipoCalendarioId, int[] bimestres, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null)
        {
            var query = new StringBuilder();
            query.AppendLine("select distinct a.id AulaId,");
            query.AppendLine("                a.data_aula DataAula,");
            query.AppendLine("				  a.disciplina_id as ComponenteCurricularCodigo,");
            query.AppendLine("				  a.turma_id as TurmaCodigo,");
            query.AppendLine("				  p.bimestre as Bimestre,");
            query.AppendLine("                a.quantidade AulasQuantidade");
            query.AppendLine("	from aula a");
            query.AppendLine("    	left join registro_frequencia_aluno rfa");
            query.AppendLine($"    		on a.id = rfa.aula_id {(codigosAluno != null && codigosAluno.Length > 0 ? " and rfa.codigo_aluno = any(@codigosAluno) " : string.Empty)}");
            query.AppendLine("        inner join periodo_escolar p");
            query.AppendLine("        	on a.tipo_calendario_id = p.tipo_calendario_id");
            query.AppendLine("where not a.excluido");
            query.AppendLine("      and a.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("      and a.data_aula >= p.periodo_inicio");
            query.AppendLine("      and a.data_aula <= p.periodo_fim");
            query.AppendLine(" 	    and a.turma_id = any(@turmasCodigo) and");
            query.AppendLine("exists (select 1");
            query.AppendLine(" 		    from registro_frequencia_aluno rfa2");
            query.AppendLine(" 		  where a.id = rfa2.aula_id and");
            query.AppendLine(" 			not rfa2.excluido)");

            if (componentesCurricularesId != null && componentesCurricularesId.Length > 0)
                query.AppendLine("and a.disciplina_id = any(@componentesCurricularesId) ");

            if (bimestres != null && bimestres.Length > 0)
                query.AppendLine("and p.bimestre = any(@bimestres) ");

            if (dataMatriculaAluno.HasValue && dataSituacaoAluno.HasValue)
            {
                dataMatriculaAluno = Convert.ToDateTime(dataMatriculaAluno.Value.ToString("yyyy/MM/dd"));
                dataSituacaoAluno = Convert.ToDateTime(dataSituacaoAluno.Value.AddDays(-1).ToString("yyyy/MM/dd"));
                query.AppendLine("and a.data_aula::date between @dataMatriculaAluno and @dataSituacaoAluno");
            }
            else if (dataMatriculaAluno.HasValue)
            {
                dataMatriculaAluno = Convert.ToDateTime(dataMatriculaAluno.Value.ToString("yyyy/MM/dd"));
                query.AppendLine("and a.data_aula::date >= @dataMatriculaAluno");
            }
            else if (dataSituacaoAluno.HasValue)
            {
                dataSituacaoAluno = Convert.ToDateTime(dataSituacaoAluno.Value.ToString("yyyy/MM/dd"));
                query.AppendLine("and a.data_aula::date < @dataSituacaoAluno");
            }

            return await database.Conexao.QueryAsync<TurmaDataAulaComponenteQtdeAulasDto>(query.ToString(),
                new { turmasCodigo, codigosAluno, componentesCurricularesId, tipoCalendarioId, bimestres, dataMatriculaAluno, dataSituacaoAluno });
        }

        public async Task<IEnumerable<RegistroFrequenciaAlunoBimestreDto>> ObterFrequenciasRegistradasPorTurmasComponentesCurriculares(string codigoAluno, string[] codigosTurma, string[] componentesCurricularesId, long? periodoEscolarId)
        {
            var sql = new StringBuilder(@"with lista as (
                                    select pe.bimestre, a.turma_id CodigoTurma, 
                                                a.disciplina_id CodigoComponenteCurricular, 
                                                rfa.codigo_aluno CodigoAluno,
                                                row_number() over (partition by a.id, pe.id, a.disciplina_id order by rfa.id desc) sequencia
                                  from registro_frequencia_aluno rfa
                                  inner join registro_frequencia rf on rfa.registro_frequencia_id = rf.id
                                  inner join aula a on a.id = rf.aula_id 
                                  inner join tipo_calendario tc on tc.id = a.tipo_calendario_id
                                  inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                                  where rfa.codigo_aluno = @codigoAluno 
                                    and a.turma_id = ANY(@codigosTurma)
                                    and a.disciplina_id = ANY(@componentesCurricularesId)");

            if (periodoEscolarId.HasValue && periodoEscolarId > 0)
                sql.AppendLine(@" and pe.id = @periodoEscolarId 
                                  and a.data_aula between pe.periodo_inicio and pe.periodo_fim ");

            sql.AppendLine(") select * from lista where sequencia = 1;");

            return await database.Conexao.QueryAsync<RegistroFrequenciaAlunoBimestreDto>(sql.ToString(), new { codigoAluno, codigosTurma, componentesCurricularesId, periodoEscolarId });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosDataAsync(string[] alunosCodigo, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string componenteCurricularId)
        {
            var query = @"select fa.*
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where
	                        fa.codigo_aluno = ANY(@alunosCodigo)
	                        and fa.tipo = @tipoFrequencia                            
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual
                            and fa.turma_id = @codigoTurma
                            and fa.disciplina_id = @componenteCurricularId";

            return await database.Conexao.QueryAsync<FrequenciaAluno>(query, new { alunosCodigo, dataAtual, tipoFrequencia, codigoTurma, componenteCurricularId });
        }
        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmaComponenteBimestres(string codigoAluno, TipoFrequenciaAluno tipoFrequencia, string componenteCurricularId, string turmaCodigo, int[] bimestres)
        {
            var query = new StringBuilder(@"select fa.*
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where
	                        codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia                            	                       
                            and turma_id = @turmaCodigo
                            and disciplina_id = @componenteCurricularId ");

            if (bimestres.Length > 0)
                query.AppendLine($" and ({(bimestres.Contains(0) ? " fa.bimestre is null or " : "")}  fa.bimestre = any(@bimestres)) ");


            return await database.QueryAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                tipoFrequencia,
                componenteCurricularId,
                turmaCodigo,
                bimestres
            });
        }

        public async Task<IEnumerable<TotalFrequenciaEAulasAlunoDto>> ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestres(string alunoCodigo, long tipoCalendarioId, string[] componentesCurricularesIds, string[] turmasCodigo, int bimestre)
        {
            var query = new StringBuilder(@"select sum(tb1.TotalPresencas) as TotalPresencas,
 		                                            sum(tb1.TotalAusencias) as TotalAusencias,
 		                                            sum(tb1.TotalRemotos) as TotalRemotos,
 		                                            tb1.AlunoCodigo,
 		                                            tb1.ComponenteCurricularId
                                               from (
                                             select
                                                    (count(distinct(a.id)) filter (where rfa.valor = 1)*a.quantidade) as TotalPresencas,
                                                    (count(distinct(a.id)) filter (where rfa.valor = 2)*a.quantidade) as TotalAusencias,
                                                    (count(distinct(a.id)) filter (where rfa.valor = 3)*a.quantidade) as TotalRemotos,
                                                    p.id as PeriodoEscolarId,
                                                    p.periodo_inicio as PeriodoInicio,
                                                    p.periodo_fim as PeriodoFim,
                                                    p.bimestre,
                                                    rfa.codigo_aluno as AlunoCodigo,
                                                    a.disciplina_id as ComponenteCurricularId
                                                from
                                                    registro_frequencia_aluno rfa
                                                inner join aula a on
                                                    rfa.aula_id = a.id
                                                inner join periodo_escolar p on
                                                    a.tipo_calendario_id = p.tipo_calendario_id
                                                where
                                              not rfa.excluido
                                              and not a.excluido
                                              and rfa.codigo_aluno = @alunoCodigo         
                                              and a.turma_id = any(@turmasCodigo)     
                                              and a.data_aula >= p.periodo_inicio
                                              and a.data_aula <= p.periodo_fim                    
                                              and rfa.numero_aula <= a.quantidade 
                                              and p.tipo_calendario_id = @tipoCalendarioId
                                              and a.disciplina_id = any(@componentesCurricularesIds) ");
            if (bimestre > 0)
                query.AppendLine(" and p.bimestre = @bimestre ");

            query.AppendLine(@" group by
                                    p.id,
                                    p.periodo_inicio,
                                    p.periodo_fim,
                                    p.bimestre,
                                    rfa.codigo_aluno,
                                    a.disciplina_id,
                                    a.quantidade) tb1
                              group by tb1.AlunoCodigo,
                                       tb1.ComponenteCurricularId");

            var parametros = new
            {
                alunoCodigo,
                tipoCalendarioId,
                componentesCurricularesIds,
                turmasCodigo,
                bimestre
            };

            return await database.QueryAsync<TotalFrequenciaEAulasAlunoDto>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosDisciplinasDataAsync(string[] codigosAlunos, string[] disciplinasIds, DateTime dataAtual, string turmaCodigo = "")
        {
            var query = @"select *
                from frequencia_aluno fa
                inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                where codigo_aluno = any(@codigosAlunos)
                and disciplina_id = any(@disciplinasIds)
	            and tipo = 1
	            and pe.periodo_inicio <= @dataAtual
	            and pe.periodo_fim >= @dataAtual";

            if (!string.IsNullOrEmpty(turmaCodigo))
                query += "and fa.turma_id = @turmaCodigo";

            return await database.QueryAsync<FrequenciaAluno>(query, new
            {
                codigosAlunos,
                disciplinasIds,
                dataAtual,
                turmaCodigo
            });
        }
    }
}
