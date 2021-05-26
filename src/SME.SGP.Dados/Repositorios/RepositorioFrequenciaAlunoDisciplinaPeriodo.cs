using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodo : RepositorioBase<FrequenciaAluno>, IRepositorioFrequenciaAlunoDisciplinaPeriodo
    {
        public RepositorioFrequenciaAlunoDisciplinaPeriodo(ISgpContext database) : base(database)
        {
        }

        private string BuildQueryObter()
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

        private String BuildQueryObterPorAlunoData(string disciplinaId = "", string codigoTurma = "")
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
            String query =
                BuildQueryObterPorAlunoData(disciplinaId, codigoTurma);

            return await database.QueryFirstOrDefaultAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                dataAtual,
                tipoFrequencia,
                disciplinaId,
                codigoTurma
            });
        }

        public FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual)
        {
            var query = @"select *
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where codigo_aluno = @codigoAluno
                            and disciplina_id = @disciplinaId
	                        and tipo = 1
	                        and pe.periodo_inicio <= @dataAtual
	                        and pe.periodo_fim >= @dataAtual";

            return database.QueryFirstOrDefault<FrequenciaAluno>(query, new
            {
                codigoAluno,
                disciplinaId,
                dataAtual,
            });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolar(string codigoTurma, string componenteCurricularId, TipoFrequenciaAluno tipoFrequencia, IEnumerable<long> periodosEscolaresIds)
        {
            const string sql = @"select 
	                                fa.*
                                from 
	                                frequencia_aluno fa 
                                where
	                                turma_id = @codigoTurma and 
	                                disciplina_id = @componenteCurricularId and 
	                                tipo = @tipoFrequencia and
	                                periodo_escolar_id = any(@periodosEscolaresIds)";

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
            var query = @"select codigo_aluno as AlunoCodigo
	                    , sum(total_aulas) as TotalAulas
	                    , sum(total_ausencias) as TotalAusencias
	                    , sum(total_compensacoes ) as TotalCompensacoes 
                      from frequencia_aluno fa 
                     where fa.turma_id = @turmaCodigo
                       and tipo = 2
                      group by codigo_aluno";

            return await database.Conexao.QueryAsync<FrequenciaAlunoDto>(query, new { turmaCodigo });
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosAsync(IEnumerable<string> alunosCodigo, IEnumerable<long?> periodosEscolaresId, string turmaId)
        {
            var query = new StringBuilder(@"select
	                        *
                        from
	                        frequencia_aluno
                        where
	                        codigo_aluno = any(@alunosCodigo)	                        	                        
                            and turma_id = @turmaId ");
            if (periodosEscolaresId != null && periodosEscolaresId.AsList().Count > 0)
            {
                query.AppendLine("and periodo_escolar_id = any(@periodosEscolaresId)");
            }

            return await database.QueryAsync<FrequenciaAluno>(query.ToString(), new
            {
                alunosCodigo,
                periodosEscolaresId,
                turmaId
            });
        }

        public async Task SalvarVariosAsync(IEnumerable<FrequenciaAluno> entidades)
        {
            var sql = @"copy frequencia_aluno (                                         
                                        codigo_aluno, 
                                        tipo, 
                                        disciplina_id, 
                                        periodo_inicio, 
                                        periodo_fim, 
                                        bimestre, 
                                        total_aulas, 
                                        total_ausencias, 
                                        criado_em,
                                        criado_por,                                        
                                        criado_rf,
                                        total_compensacoes,
                                        turma_id,
                                        periodo_escolar_id)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var frequencia in entidades)
                {
                    writer.StartRow();
                    writer.Write(frequencia.CodigoAluno, NpgsqlDbType.Varchar);
                    writer.Write((int)frequencia.Tipo, NpgsqlDbType.Integer);
                    writer.Write(frequencia.DisciplinaId, NpgsqlDbType.Varchar);
                    writer.Write(frequencia.PeriodoInicio, NpgsqlDbType.Timestamp);
                    writer.Write(frequencia.PeriodoFim, NpgsqlDbType.Timestamp);
                    writer.Write(frequencia.Bimestre, NpgsqlDbType.Integer);
                    writer.Write(frequencia.TotalAulas, NpgsqlDbType.Integer);
                    writer.Write(frequencia.TotalAusencias, NpgsqlDbType.Integer);
                    writer.Write(frequencia.CriadoEm, NpgsqlDbType.Timestamp);
                    writer.Write(database.UsuarioLogadoNomeCompleto, NpgsqlDbType.Varchar);
                    writer.Write(database.UsuarioLogadoRF, NpgsqlDbType.Varchar);
                    writer.Write(frequencia.TotalCompensacoes, NpgsqlDbType.Integer);
                    writer.Write(frequencia.TurmaId, NpgsqlDbType.Varchar);

                    if (frequencia.PeriodoEscolarId.HasValue)
                        writer.Write((long)frequencia.PeriodoEscolarId, NpgsqlDbType.Bigint);

                }
                await Task.FromResult(writer.Complete());
            }
        }

        public async Task RemoverVariosAsync(long[] ids)
        {
            var query = @"delete from frequencia_aluno where id = any(@ids)";

            await database.Conexao.ExecuteAsync(query, new
            {
                ids
            });
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

        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmasDisciplinasDataAsync(string codigoAluno, TipoFrequenciaAluno tipoFrequencia,
                     string[] disciplinasId, string[] turmasCodigo, int[] bimestres)
        {
            var query = new StringBuilder(@"select fa.*
                        from frequencia_aluno fa
                        inner join periodo_escolar pe on fa.periodo_escolar_id = pe.id
                        where
	                        codigo_aluno = @codigoAluno
	                        and tipo = @tipoFrequencia                            	                       
                            and turma_id = ANY(@turmasCodigo)
                            and disciplina_id = ANY(@disciplinasId) ");

            if (bimestres.Length > 0)
                query.AppendLine(" and pe.bimestre = ANY(@bimestres)");


            return await database.QueryAsync<FrequenciaAluno>(query.ToString(), new
            {
                codigoAluno,
                tipoFrequencia,
                disciplinasId,
                turmasCodigo,
                bimestres
            });
        }
    }
}