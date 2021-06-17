using Dapper;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequencia : RepositorioBase<RegistroFrequencia>, IRepositorioFrequencia
    {
        public RepositorioFrequencia(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirFrequenciaAula(long aulaId)
        {
            // Exclui registros de ausencia do aluno
            var command = @"update registro_ausencia_aluno
                                set excluido = true
                            where not excluido
                              and registro_frequencia_id in (
                                select id from registro_frequencia
                                 where not excluido
                                   and aula_id = @aulaId)";
            await database.ExecuteAsync(command, new { aulaId });

            // Exclui registro de frequencia da aula
            command = @"update registro_frequencia
                            set excluido = true
                        where not excluido
                          and aula_id = @aulaId ";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
        {
            var query = @"select 1 from registro_frequencia where aula_id = @aulaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { aulaId });
        }

        public async Task<IEnumerable<AlunoComponenteCurricularDto>> ObterAlunosAusentesPorTurmaEPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            var query = new StringBuilder(@"select distinct a.disciplina_id as ComponenteCurricularId, raa.codigo_aluno as AlunoCodigo
                        from registro_ausencia_aluno raa 
                       inner join registro_frequencia rf on rf.id = raa.registro_frequencia_id 
                       inner join aula a on a.id = rf.aula_id 
                       where 
                         a.turma_id = @turmaCodigo
                         and a.data_aula between @dataInicio and @dataFim ");

            if (!string.IsNullOrEmpty(componenteCurricularId))
                query.AppendLine("and a.disciplina_id = @componenteCurricularId");

            return await database.Conexao.QueryAsync<AlunoComponenteCurricularDto>(query.ToString(), new { turmaCodigo, dataInicio, dataFim, componenteCurricularId });
        }

        public IEnumerable<AlunosFaltososDto> ObterAlunosFaltosos(DateTime dataReferencia, long tipoCalendarioId)
        {
            var query = @"select a.TurmaCodigo
	                     , a.ModalidadeCodigo
	                     , a.Ano
	                     , a.DataAula
	                     , a.CodigoAluno
	                     , Sum(a.Aulas) as QuantidadeAulas
	                     , Sum(a.Ausencias) as QuantidadeFaltas
                    from (
	                    select a.turma_id as TurmaCodigo
	                         , t.modalidade_codigo as modalidadeCodigo
	                         , t.ano
	                         , a.id as AulaId
	                         , a.data_aula as DataAula
	                         , a.quantidade as Aulas
	                         , raa.codigo_aluno as CodigoAluno
	                         , count(raa.id) as Ausencias
	
	                    from aula a
	                      inner join registro_frequencia rf on a.id = rf.aula_id
	                      inner join turma t on t.turma_id = a.turma_id
	                       left join registro_ausencia_aluno raa on raa.registro_frequencia_id = rf.id and not raa.excluido
	                    where not a.excluido
	                      and not rf.excluido
	                      and a.data_aula >= @dataReferencia
	                      and a.tipo_calendario_id = @tipoCalendarioId
	                    group by a.turma_id, t.modalidade_codigo, t.ano, a.id, a.data_aula, a.quantidade, raa.codigo_aluno
                    ) a
                    group by a.TurmaCodigo
	                     , a.ModalidadeCodigo
	                     , a.Ano
	                     , a.DataAula
	                     , a.CodigoAluno";

            return database.Conexao.Query<AlunosFaltososDto>(query, new { dataReferencia, tipoCalendarioId });
        }

        public RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId)
        {
            var query = @"select a.ue_id as codigoUe, a.turma_id as codigoTurma
                            , a.disciplina_id as codigoDisciplina, a.data_aula as DataAula
	                        , a.professor_rf as professorRf, t.nome as nomeTurma, ue.nome as nomeUe
                            , ue.dre_id as codigoDre
                         from registro_frequencia rf
                        inner join aula a on a.id = rf.aula_id
                        inner join turma t on t.turma_id = a.turma_id
                        inner join ue on ue.ue_id = a.ue_id
                        where rf.id = @registroFrequenciaId";

            return database.Conexao.QueryFirstOrDefault<RegistroFrequenciaAulaDto>(query, new { registroFrequenciaId });
        }

        public IEnumerable<AulasPorTurmaDisciplinaDto> ObterAulasSemRegistroFrequencia(string turmaId, string disciplinaId, TipoNotificacaoFrequencia tipoNotificacao)
        {
            var query = @"select
	                        a.id,
	                        a.professor_rf as professorId,
	                        a.data_aula as dataAula,
	                        a.quantidade
                        from
	                        aula a
                        where
	                        not a.excluido
	                        and not a.migrado
	                        and not exists (
	                        select
		                        1
	                        from
		                        notificacao_frequencia n
	                        where
		                        n.aula_id = a.id
		                        and n.tipo = @tipoNotificacao)
	                        and not exists (
	                        select
		                        1
	                        from
		                        registro_frequencia r
	                        where
		                        r.aula_id = a.id)
	                        and a.data_aula < date(now())
	                        and a.turma_id = @turmaId
	                        and a.disciplina_id = @disciplinaId";

            IEnumerable<AulasPorTurmaDisciplinaDto> lista = new List<AulasPorTurmaDisciplinaDto>();
            try
            {
                lista = database.Conexao.Query<AulasPorTurmaDisciplinaDto>(query, new { turmaId, disciplinaId, tipoNotificacao });
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureEvent(new SentryEvent(ex));
                SentrySdk.CaptureEvent(new SentryEvent(new NegocioException($"ObterAulasSemRegistroFrequencia - {turmaId} - {disciplinaId}")));
            }
            return lista;
        }

        public async Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos)
        {
            var query = @"select a.codigo_aluno as AlunoCodigo, a.data_aula as AulaData from (select raa.codigo_aluno, a.quantidade, a.data_aula, count(raa.id) as faltas  from registro_frequencia rf
                            inner join aula a
                            on rf.aula_id = a.id
                            inner join registro_ausencia_aluno raa
                            on raa.registro_frequencia_id = rf.id
                            where  a.turma_id = @turmaCodigo
                            and a.disciplina_id = @disciplinaCodigo
                            and a.data_aula::date = ANY(@datas)
                            and raa.codigo_aluno = ANY(@alunoCodigos)
                            group by raa.codigo_aluno, a.quantidade, a.data_aula) a
                            where a.quantidade = a.faltas";

            return await database.Conexao.QueryAsync<AusenciaAlunoDto>(query, new { turmaCodigo, disciplinaCodigo, datas, alunoCodigos });
        }

        public async Task<IEnumerable<RecuperacaoParalelaFrequenciaDto>> ObterFrequenciaAusencias(string[] CodigoAlunos, string CodigoDisciplina, int Ano, PeriodoRecuperacaoParalela Periodo)
        {
            var query = new StringBuilder();
            query.AppendLine("select codigo_aluno CodigoAluno,");
            query.AppendLine("SUM(total_aulas) TotalAulas,");
            query.AppendLine("SUM(total_ausencias) TotalAusencias");
            query.AppendLine("from frequencia_aluno");
            query.AppendLine("where codigo_aluno::varchar(100) = ANY(@CodigoAlunos)");
            query.AppendLine("and date_part('year',periodo_inicio) = @Ano");
            query.AppendLine("and date_part('year',periodo_fim) = @Ano");
            query.AppendLine("and disciplina_id = @CodigoDisciplina");
            if (Periodo == PeriodoRecuperacaoParalela.AcompanhamentoPrimeiroSemestre)
                query.AppendLine("and bimestre IN  (1,2)");
            query.AppendLine("group by codigo_aluno");

            return await database.Conexao.QueryAsync<RecuperacaoParalelaFrequenciaDto>(query.ToString(), new { CodigoAlunos, CodigoDisciplina = CodigoDisciplina.ToArray(), Ano });
        }

        public IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId)
        {
            var query = @"select ra.*
                        from
	                        registro_frequencia rf
                        inner join registro_ausencia_aluno ra on
	                        rf.id = ra.registro_frequencia_id
                        inner join aula a on
	                        a.id = rf.aula_id
                        where ra.excluido = false and
	                        a.id = @aulaId";

            return database.Conexao.Query<RegistroAusenciaAluno>(query, new { aulaId });
        }

        public async Task<RegistroFrequencia> ObterRegistroFrequenciaPorAulaId(long aulaId)
        {
            var query = @"select *
                            from registro_frequencia
                          where not excluido
                            and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<RegistroFrequencia>(query, new { aulaId });
        }

        public async Task<IEnumerable<AusenciaMotivoDto>> ObterAusenciaMotivoPorAlunoTurmaBimestreAno(string codigoAluno, string turma, short bimestre, short anoLetivo)
        {
            var sql = @"
                select
	                a.data_aula dataAusencia,
	                afa.criado_por registradoPor,
	                ma.descricao motivoAusencia,
	                afa.anotacao justificativaAusencia
                from 
	                anotacao_frequencia_aluno afa 
                inner join aula a on a.id = afa.aula_id 
                inner join tipo_calendario tc on tc.id = a.tipo_calendario_id 
                inner join periodo_escolar pe on pe.tipo_calendario_id = tc.id
                 left join motivo_ausencia ma on afa.motivo_ausencia_id = ma.id 
                where 
	                not afa.excluido and not a.excluido and 
	                afa.codigo_aluno = @codigoAluno and
	                a.turma_id = @turma and
	                tc.ano_letivo = @anoLetivo and 
	                pe.bimestre = @bimestre
                order by a.data_aula desc
                limit 5
            ";

            return await database
                .Conexao
                .QueryAsync<AusenciaMotivoDto>(sql, new { codigoAluno, turma, bimestre, anoLetivo });
        }

        public async Task<IEnumerable<GraficoBaseDto>> ObterDashboardFrequenciaAusenciasPorMotivo(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, string ano, long turmaId, int semestre)
        {
            var query = @"select ma.descricao, count(afa.id) as Quantidade
                      from anotacao_frequencia_aluno afa   
                     left join motivo_ausencia ma on ma.id = afa.motivo_ausencia_id 
                     inner join aula a on a.id = afa.aula_id 
                     inner join registro_frequencia rf on rf.aula_id = a.id 
                     inner join registro_ausencia_aluno raa on raa.registro_frequencia_id = rf.id and raa.codigo_aluno = afa.codigo_aluno 
                     inner join turma t on t.turma_id = a.turma_id 
                     inner join ue on ue.id = t.ue_id 
                     inner join dre on dre.id = ue.dre_id 
                     where not a.excluido 
                       and not afa.excluido 
                       and t.ano_letivo = @anoLetivo ";

            if (dreId > 0)
                query += " and ue.dre_id = @dreId";

            if (ueId > 0)
                query += " and ue.id = @ueId";

            if (modalidade.HasValue && modalidade.Value > 0)
                query += " and t.modalidade_codigo = @modalidade";

            if (!string.IsNullOrEmpty(ano))
                query += " and t.ano = @ano";

            if (turmaId > 0)
                query += " and t.id = @turmaId";

            if (semestre > 0)
                query += " and t.semestre = @semestre";

            query += " group by ma.descricao";

            return await database.Conexao.QueryAsync<GraficoBaseDto>(query, new
            {
                anoLetivo,
                dreId,
                ueId,
                modalidade = (int)modalidade,
                ano,
                turmaId,
                semestre
            });
        }
        public async Task SalvarConciliacaoTurma(string turmaId, string disciplinaId, DateTime dataReferencia, string alunos)
        {
            var query = @"insert into conciliacao_turma (turma_id, disciplina_id, data_referencia, alunos) 
                          values (@turmaId, @disciplinaId, @dataReferencia, @alunos)";

            await database.Conexao.ExecuteAsync(query, new { turmaId, disciplinaId, dataReferencia, alunos });
        }

        public async Task<IEnumerable<string>> ObterTurmasCodigosFrequenciasExistentesPorAnoAsync(int[] anosLetivos)
        {
            var query = @"  select distinct(t.turma_id)
                             from registro_frequencia rf
                            inner join aula a on a.id = rf.aula_id 
                            inner join turma t on t.turma_id = a.turma_id 
                            where not a.excluido
                              and not rf.excluido
                              and t.ano_letivo = ANY(@anosLetivos)";

            return await database.Conexao.QueryAsync<string>(query, new { anosLetivos });
        }

        public async Task<IEnumerable<AulaComFrequenciaNaDataDto>> ObterAulasComRegistroFrequenciaPorTurma(string turmaCodigo)
        {
            var query = @"select a.id as AulaId
	                        , a.data_aula as DataAula
	                        , a.quantidade as QuantidadeAulas
	                        , rf.id as RegistroFrequenciaId
                          from aula a 
                         inner join registro_frequencia rf on rf.aula_id = a.id
                         where not a.excluido 
                           and not rf.excluido 
                           and a.turma_id = @turmaCodigo ";

            return await database.Conexao.QueryAsync<AulaComFrequenciaNaDataDto>(query, new { turmaCodigo });
        }
    }
}