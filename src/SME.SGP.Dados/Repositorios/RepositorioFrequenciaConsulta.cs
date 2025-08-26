using Dapper;
using Polly;
using Polly.Registry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaConsulta : RepositorioBase<RegistroFrequencia>, IRepositorioFrequenciaConsulta
    {
        private readonly IAsyncPolicy policy;
        public RepositorioFrequenciaConsulta(ISgpContextConsultas database,
                                             IReadOnlyPolicyRegistry<string> registry,
                                             IServicoAuditoria servicoAuditoria)
            : base(database, servicoAuditoria)
        {
            policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<bool> FrequenciaAulaRegistrada(long aulaId)
        {
            var query = @"select 1 from registro_frequencia where aula_id = @aulaId";

            var retorno = await policy
                .ExecuteAsync(() => database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { aulaId }));

            return retorno;
        }

        public async Task<IEnumerable<AlunoComponenteCurricularDto>> ObterAlunosAusentesPorTurmaEPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId)
        {
            var query = new StringBuilder(@"select distinct a.disciplina_id as ComponenteCurricularId, raa.codigo_aluno as AlunoCodigo
                        from registro_frequencia_aluno raa 
                       inner join aula a on a.id = raa.aula_id 
                       where raa.valor = @tipoFrequencia
                         and a.turma_id = @turmaCodigo
                         and a.data_aula between @dataInicio and @dataFim ");

            if (!string.IsNullOrEmpty(componenteCurricularId))
                query.AppendLine("and a.disciplina_id = @componenteCurricularId");

            return await database.Conexao.QueryAsync<AlunoComponenteCurricularDto>(query.ToString(), new { turmaCodigo, dataInicio, dataFim, componenteCurricularId,tipoFrequencia = (int)TipoFrequencia.F });
        }

        public async Task<IEnumerable<AlunosFaltososDto>> ObterAlunosFaltosos(DateTime dataReferencia, long tipoCalendarioId, long ueId)
        {
            const string query = @"select a.TurmaCodigo
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
                                          inner join turma t on t.turma_id = a.turma_id
                                          left join registro_frequencia_aluno raa on raa.aula_id = a.id and not raa.excluido and raa.valor = @tipoFrequencia
                                        where not a.excluido
                                          and a.data_aula >= @dataReferencia
                                          and a.tipo_calendario_id = @tipoCalendarioId
                                          and t.ue_id = @ueId
                                        group by a.turma_id, t.modalidade_codigo, t.ano, a.id, a.data_aula, a.quantidade, raa.codigo_aluno
                                    ) a
                                    group by a.TurmaCodigo
                                         , a.ModalidadeCodigo
                                         , a.Ano
                                         , a.DataAula
                                         , a.CodigoAluno";

            return await database.Conexao.QueryAsync<AlunosFaltososDto>(query,
                new
                {
                    dataReferencia, tipoCalendarioId, ueId,
                    tipoFrequencia = (int)TipoFrequencia.F
                });
        }

        public RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId)
        {
            var query = @"select a.ue_id as codigoUe, a.turma_id as codigoTurma
                            , a.disciplina_id as codigoDisciplina, a.data_aula as DataAula
                            , a.professor_rf as professorRf, t.nome as nomeTurma, ue.nome as nomeUe
                            , ue.dre_id as codigoDre
                            , te.descricao nomeTipoEscola
                            , d.abreviacao nomeDre
                           from registro_frequencia rf
                           inner join aula a on a.id = rf.aula_id
                           inner join turma t on t.turma_id = a.turma_id
                           inner join ue on ue.ue_id = a.ue_id
                           inner join tipo_escola te on ue.tipo_escola = te.id
                           inner join dre d on d.id = ue.dre_id
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

            return database.Conexao.Query<AulasPorTurmaDisciplinaDto>(query, new { turmaId, disciplinaId, tipoNotificacao });
        }

        public async Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos)
        {
            var query = @"SELECT a.codigo_aluno AS AlunoCodigo,
                                 a.data_aula AS AulaData
                          FROM
                            (SELECT raa.codigo_aluno,
                                    a.quantidade,
                                    a.data_aula,
                                    count(raa.id) AS faltas
                             FROM registro_frequencia_aluno raa
                             INNER JOIN aula a ON raa.aula_id = a.id AND raa.valor = @tipoFrequencia
                             WHERE a.turma_id = @turmaCodigo
                               AND a.disciplina_id = @disciplinaCodigo
                               AND a.data_aula::date = ANY(@datas)
                               AND raa.codigo_aluno = ANY(@alunoCodigos)
                             GROUP BY raa.codigo_aluno,
                                      a.quantidade,
                                      a.data_aula) a
                          WHERE a.quantidade = a.faltas";

            return await database.Conexao.QueryAsync<AusenciaAlunoDto>(query, new { turmaCodigo, disciplinaCodigo, datas, alunoCodigos,tipoFrequencia = (int)TipoFrequencia.F });
        }
        
        public async Task<IEnumerable<AusenciaAlunoDto>> ObterAusenciasPorAluno(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string alunoCodigo)
        {
            var query = @"SELECT a.codigo_aluno AS AlunoCodigo,
                                 a.data_aula AS AulaData
                          FROM
                            (SELECT raa.codigo_aluno,
                                    a.quantidade,
                                    a.data_aula,
                                    count(raa.id) AS faltas
                             FROM registro_frequencia_aluno raa
                             INNER JOIN aula a ON raa.aula_id = a.id AND raa.valor = @tipoFrequencia
                             WHERE a.turma_id = @turmaCodigo
                               AND a.disciplina_id = @disciplinaCodigo
                               AND a.data_aula::date = ANY(@datas)
                               AND raa.codigo_aluno = @alunoCodigo
                             GROUP BY raa.codigo_aluno,
                                      a.quantidade,
                                      a.data_aula) a
                          WHERE a.quantidade = a.faltas";

            return await database.Conexao.QueryAsync<AusenciaAlunoDto>(query, new { turmaCodigo, disciplinaCodigo, datas, alunoCodigo,tipoFrequencia = (int)TipoFrequencia.F });
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

        public Task<IEnumerable<RegistroFrequencia>> ObterRegistroFrequenciaPorDataEAulaId(string disciplina, string turmaId, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"select rf.*
                            from registro_frequencia rf
                            inner join aula a on rf.aula_id  = a.id 
                          where DATE(a.data_aula) between Date(@dataInicio) and Date(@dataFim)
                                and a.disciplina_id = @disciplina
                                and a.turma_id = @turmaId
                            order by rf.criado_em desc";

            return database.Conexao.QueryAsync<RegistroFrequencia>(query, new { disciplina, turmaId, dataInicio, dataFim });
        }

        public Task<IEnumerable<RegistroAusenciaAluno>> ObterListaFrequenciaPorAula(long aulaId)
        {
            var query = @"SELECT ra.*
                            FROM registro_frequencia_aluno ra
                            INNER JOIN aula a ON a.id = ra.aula_id AND ra.valor = @tipoFrequencia
                            WHERE ra.excluido = FALSE
                              AND a.id = @aulaId";

            return database.Conexao.QueryAsync<RegistroAusenciaAluno>(query, new { aulaId,tipoFrequencia = (int)TipoFrequencia.F });
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
                    and (a.data_aula >= pe.periodo_inicio and a.data_aula <= pe.periodo_fim ) 
                order by a.data_aula desc
                limit 5
            ";

            return await database
                .Conexao
                .QueryAsync<AusenciaMotivoDto>(sql, new { codigoAluno, turma, bimestre, anoLetivo });
        }

        public async Task<IEnumerable<GraficoBaseDto>> ObterDashboardFrequenciaAusenciasPorMotivo(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, string ano, long turmaId, int semestre)
        {
            var query = @"SELECT ma.descricao,
                                   count(afa.id) AS Quantidade
                            FROM anotacao_frequencia_aluno afa
                            LEFT JOIN motivo_ausencia ma ON ma.id = afa.motivo_ausencia_id
                            INNER JOIN aula a ON a.id = afa.aula_id
                            INNER JOIN registro_frequencia_aluno raa ON raa.aula_id  = a.id
                            AND raa.codigo_aluno = afa.codigo_aluno
                            AND raa.valor = @tipoFrequencia
                            INNER JOIN turma t ON t.turma_id = a.turma_id
                            INNER JOIN ue ON ue.id = t.ue_id
                            INNER JOIN dre ON dre.id = ue.dre_id
                            WHERE NOT a.excluido
                              AND NOT afa.excluido
                              AND t.ano_letivo = @anoLetivo";

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
                semestre,
                tipoFrequencia = (int)TipoFrequencia.F
            });
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

        public Task<IEnumerable<RegistroFrequenciaAlunoPorAulaDto>> ObterFrequenciasDetalhadasPorData(string turmaCodigo, string[] componentesCurricularesId, string[] codigosAlunos, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"select a.id as AulaId
                                , rfa.registro_frequencia_id as RegistroFrequenciaId
                                , rfa.codigo_aluno as AlunoCodigo
                                , rfa.numero_aula as NumeroAula
                                , rfa.valor as TipoFrequencia
                                , rfa.criado_em as CriadoEm
                                , rfa.criado_por as CriadoPor
                                , rfa.criado_rf as CriadoRf
                                , rfa.alterado_em as AlteradoEm
                                , rfa.alterado_por as AlteradoPor
                                , rfa.alterado_rf as AlteradoRf
                           from registro_frequencia_aluno rfa 
                           inner join aula a on a.id = rfa.aula_id 
                           where rfa.codigo_aluno = any(@codigosAlunos)
                            and not rfa.excluido
                            and not a.excluido 
                            and a.turma_id = @turmaCodigo
                            and a.disciplina_id = any(@componentesCurricularesId)
                            and a.data_aula between @dataInicio and @dataFim ";

            return database.Conexao.QueryAsync<RegistroFrequenciaAlunoPorAulaDto>(query, new { turmaCodigo, componentesCurricularesId, codigosAlunos, dataInicio, dataFim });
        }

        public Task<bool> RegistraFrequencia(long componenteCurricularId, long? codigoTerritorioSaber = null)
        {
            var query = "select permite_registro_frequencia from componente_curricular where id = @componenteCurricularId";

            return database.Conexao
                .QueryFirstOrDefaultAsync<bool>(query, new { componenteCurricularId = codigoTerritorioSaber.HasValue ? codigoTerritorioSaber.Value : componenteCurricularId });
        }

        public async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(int anoLetivo,
            string dreCodigo, string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            if (dreCodigo.Trim() == "-99")
                dreCodigo = string.Empty;

            if (ueCodigo.Trim() == "-99")
                ueCodigo = string.Empty;

            if (string.IsNullOrEmpty(dreCodigo) && string.IsNullOrEmpty(ueCodigo))
                return await ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorDre(anoLetivo, modalidade, semestre, mes);
            else if (!string.IsNullOrEmpty(dreCodigo) && string.IsNullOrEmpty(ueCodigo))
                return await ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorUe(anoLetivo, dreCodigo, modalidade, semestre, mes);
            else
                return await ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorTurma(anoLetivo, dreCodigo, ueCodigo, modalidade, semestre, mes);
        }

        private async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorDre(int anoLetivo, Modalidade modalidade,
            int semestre, int mes)
        {
            var query = @"select d.Abreviacao as Descricao, d.dre_id as DreCodigo,
                                sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id";
            var where = @" where t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_abaixo_50_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes ";

            if (semestre > 0)
                where += " and t.semestre = @semestre ";

            query += where;

            query += @" group by d.dre_id, d.Abreviacao
                        order by d.dre_id; ";

            query += @" select sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                              from frequencia_turma_evasao fte
                              inner join turma t on t.id = fte.turma_id";
            query += where;
            query += ";";

            var parametros = new { modalidade, semestre, anoLetivo, mes };
            var retorno = new FrequenciaTurmaEvasaoDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var graficos = multi.Read<GraficoFrequenciaTurmaEvasaoDto>().ToList();
                graficos.ForEach(c => c.Descricao = c.Descricao.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim());
                retorno.GraficosFrequencia = graficos;
                retorno.TotalEstudantes = multi.ReadFirst<long?>() ?? 0;
            }

            return retorno;
        }

        private async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorUe(int anoLetivo, string dreCodigo,
            Modalidade modalidade, int semestre, int mes)
        {
            var query = @"select coalesce(te.descricao || '-', '') || coalesce(u.nome, '') as Descricao, u.ue_id as UeCodigo,
                                sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                                left join tipo_escola te on te.cod_tipo_escola_eol = u.tipo_escola";
            var where = @" where d.dre_id = @dreCodigo
                            and t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_abaixo_50_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes";

            if (semestre > 0)
                where += " and t.semestre = @semestre ";

            query += where;

            query += @" group by te.descricao, u.nome, u.ue_id
                        order by te.descricao, u.nome; ";

            query += @" select sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                              from frequencia_turma_evasao fte
                              inner join turma t on t.id = fte.turma_id
                              inner join ue u on u.id = t.ue_id 
                              inner join dre d on d.id = u.dre_id ";
            query += where;
            query += ";";

            var parametros = new { dreCodigo, modalidade, semestre, anoLetivo, mes };
            var retorno = new FrequenciaTurmaEvasaoDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.GraficosFrequencia = multi.Read<GraficoFrequenciaTurmaEvasaoDto>().ToList();
                retorno.TotalEstudantes = multi.ReadFirst<long?>() ?? 0;
            }

            return retorno;
        }

        private async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorTurma(int anoLetivo, string dreCodigo,
            string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            var with = @"WITH TurmaEvasao AS
                          (SELECT DISTINCT turma_id,
                                           mes,
                                           quantidade_alunos_abaixo_50_porcento
                           FROM frequencia_turma_evasao)";
            var query = @$"{with} SELECT t.nome AS Descricao, t.turma_id as TurmaCodigo,
                               sum(fte.quantidade_alunos_abaixo_50_porcento) AS Quantidade
                        FROM TurmaEvasao fte
                        INNER JOIN turma t ON t.id = fte.turma_id
                        INNER JOIN ue u ON u.id = t.ue_id
                        INNER JOIN dre d ON d.id = u.dre_id";
            var where = @" WHERE d.dre_id = @dreCodigo
                          AND u.ue_id = @ueCodigo
                          AND t.modalidade_codigo = @modalidade
                          AND (fte.quantidade_alunos_abaixo_50_porcento>0)
                          AND t.ano_letivo = @anoLetivo
                          AND fte.mes = @mes";

            if (semestre > 0)
                where += " and t.semestre = @semestre ";

            query += where;
            query += @" group by t.nome, t.turma_id
                        order by t.nome; ";

            query += @$"{with} SELECT sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                              FROM TurmaEvasao fte
                              INNER JOIN turma t ON t.id = fte.turma_id
                              INNER JOIN ue u ON u.id = t.ue_id
                              INNER JOIN dre d ON d.id = u.dre_id";
            query += where;
            query += ";";

            var parametros = new { dreCodigo, ueCodigo, modalidade, semestre, anoLetivo, mes };
            var retorno = new FrequenciaTurmaEvasaoDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var graficos = multi.Read<GraficoFrequenciaTurmaEvasaoDto>().ToList();
                graficos.ForEach(c => c.Descricao = string.Concat(modalidade.ShortName(), " - ", c.Descricao));
                retorno.GraficosFrequencia = graficos;
                retorno.TotalEstudantes = multi.ReadFirst<long?>() ?? 0;
            }

            return retorno;
        }

        public async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoSemPresenca(int anoLetivo, string dreCodigo,
            string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            if (dreCodigo.Trim() == "-99")
                dreCodigo = string.Empty;

            if (ueCodigo.Trim() == "-99")
                ueCodigo = string.Empty;

            if (string.IsNullOrEmpty(dreCodigo) && string.IsNullOrEmpty(ueCodigo))
                return await ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorDre(anoLetivo, modalidade, semestre, mes);
            else if (!string.IsNullOrEmpty(dreCodigo) && string.IsNullOrEmpty(ueCodigo))
                return await ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorUe(anoLetivo, dreCodigo, modalidade, semestre, mes);
            else
                return await ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorTurma(anoLetivo, dreCodigo, ueCodigo, modalidade, semestre, mes);
        }

        private async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorDre(int anoLetivo,
            Modalidade modalidade, int semestre, int mes)
        {
            var query = @"select d.Abreviacao as Descricao, d.dre_id as DreCodigo,
                                sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id"; 
            var where = @" where t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_0_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes";

            if (semestre > 0)
                where += " and t.semestre = @semestre ";

            query += where;

            query += @" group by d.dre_id, d.Abreviacao
                        order by d.dre_id; ";

            query += @" select sum(fte.quantidade_alunos_0_porcento) as Quantidade
                              from frequencia_turma_evasao fte
                              inner join turma t on t.id = fte.turma_id";
            query += where;
            query += ";";

            var parametros = new { modalidade, semestre, anoLetivo, mes };
            var retorno = new FrequenciaTurmaEvasaoDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var graficos = multi.Read<GraficoFrequenciaTurmaEvasaoDto>().ToList();
                graficos.ForEach(c => c.Descricao = c.Descricao.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim());
                retorno.GraficosFrequencia = graficos;
                retorno.TotalEstudantes = multi.ReadFirst<long?>() ?? 0;
            }

            return retorno;
        }

        private async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorUe(int anoLetivo, string dreCodigo,
            Modalidade modalidade, int semestre, int mes)
        {
            var query = @"select coalesce(te.descricao || '-', '') || coalesce(u.nome, '') as Descricao, u.ue_id as UeCodigo,
                                 te.descricao as DescricaoTipo,
                                sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                                left join tipo_escola te on te.cod_tipo_escola_eol = u.tipo_escola";
            var where = @" where d.dre_id = @dreCodigo
                            and t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_0_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes";

            if (semestre > 0)
                where += " and t.semestre = @semestre ";

            query += where;

            query += @" group by te.descricao, u.nome, u.ue_id
                        order by te.descricao, u.nome; ";

            query += @" select sum(fte.quantidade_alunos_0_porcento) as Quantidade
                              from frequencia_turma_evasao fte
                              inner join turma t on t.id = fte.turma_id 
                              inner join ue u on u.id = t.ue_id 
                              inner join dre d on d.id = u.dre_id ";
            query += where;
            query += ";";

            var parametros = new { dreCodigo, modalidade, semestre, anoLetivo, mes };
            var retorno = new FrequenciaTurmaEvasaoDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.GraficosFrequencia = multi.Read<GraficoFrequenciaTurmaEvasaoDto>().ToList();
                retorno.TotalEstudantes = multi.ReadFirst<long?>() ?? 0;
            }

            return retorno;
        }

        private async Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorTurma(int anoLetivo, string dreCodigo,
            string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            var with = @" WITH TurmaEvasao AS
                          (SELECT DISTINCT turma_id,
                                           mes,
                                           quantidade_alunos_0_porcento
                           FROM frequencia_turma_evasao)";
            var query = @$"{with} select t.nome as Descricao, t.turma_id as TurmaCodigo,
                                sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from TurmaEvasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id ";
            var where = @" where d.dre_id = @dreCodigo
                            and u.ue_id = @ueCodigo
                            and t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_0_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes";

            if (semestre > 0)
                where += " and t.semestre = @semestre ";

            query += where;

            query += @" group by t.nome, t.turma_id
                        order by t.nome; ";

            query += @$"{with} select sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from TurmaEvasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id ";

            query += where;
            query += ";";

            var parametros = new { dreCodigo, ueCodigo, modalidade, semestre, anoLetivo, mes };
            var retorno = new FrequenciaTurmaEvasaoDto();

            using (var multi = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var graficos = multi.Read<GraficoFrequenciaTurmaEvasaoDto>().ToList();
                graficos.ForEach(c => c.Descricao = string.Concat(modalidade.ShortName(), " - ", c.Descricao));
                retorno.GraficosFrequencia = graficos;
                retorno.TotalEstudantes = multi.ReadFirst<long?>() ?? 0;
            }

            return retorno;
        }

        public async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaPorTurmaPeriodo(string codigoTurma, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"with totalAulas as (
                            select  t.id as TurmaId,
                                    sum(a.quantidade) as TotalAulas
                            from aula a
                                join turma t on t.turma_id = a.turma_id
                            where
                                not a.excluido
                                and a.turma_id = @codigoTurma
                                and a.data_aula >= @dataInicio
                                and a.data_aula <= @dataFim
                            group by t.id
                        ),
                        totalFrequencia as (
                            select rfa.codigo_aluno as AlunoCodigo,
                                   (count(distinct(a.id)) filter (    where rfa.valor = 1)* a.quantidade) as TotalPresencas,
                                   (count(distinct(a.id)) filter (where rfa.valor = 2)* a.quantidade) as TotalAusencias,
                                   (count(distinct(a.id)) filter (where rfa.valor = 3)* a.quantidade) as TotalRemotos
                            from registro_frequencia rf
                              join registro_frequencia_aluno rfa on rf.id = rfa.registro_frequencia_id
                              join aula a on a.id = rf.aula_id
                            where
                                not rfa.excluido
                                and not a.excluido
                                and a.turma_id = @codigoTurma
                                and a.data_aula >= @dataInicio
                                and a.data_aula <= @dataFim
                                and rfa.numero_aula <= a.quantidade
                            group by rfa.codigo_aluno, a.quantidade
                        )
                        select alunoCodigo, 
                               totalPresencas, 
                               totalAusencias,
                               totalRemotos,
                               totalAulas
                        from totalAulas
                          left join totalFrequencia on 1 = 1
                        order by AlunoCodigo ";

            return await database.Conexao.QueryAsync<FrequenciaAlunoDto>(query, new { codigoTurma, dataInicio, dataFim });
        }

        public async Task<long[]> ObterFrequenciasAlunosIdsComPresencasMaiorTotalAulas(long ueId, int anoLetivo)
        {
            var query = @"select fa.id
                              from frequencia_aluno fa
                                  inner join turma t 
                                      on fa.turma_id = t.turma_id         
                          where not fa.excluido and
                                  t.ano_letivo = @anoLetivo and
                                  fa.total_presencas > fa.total_aulas and
                                  t.ue_id = @ueId;";

            var resultado = await database.Conexao
                .QueryAsync<long>(query, new { ueId, anoLetivo });

            return resultado.ToArray();
        }

        public async Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoPaginado(int mes, FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, Paginacao paginacao)
        {
            var sql = new StringBuilder();
            MontaQueryConsultaFrequenciaTurmaEvasaoAbaixo50Porcento(paginacao, sql, mes, filtroAbrangencia);
            sql.AppendLine(";");
            MontaQueryConsultaFrequenciaTurmaEvasaoAbaixo50Porcento(paginacao, sql, mes, filtroAbrangencia, true);

            var parametros = new
            {
                filtroAbrangencia.AnoLetivo,
                filtroAbrangencia.DreCodigo,
                filtroAbrangencia.UeCodigo,
                filtroAbrangencia.TurmaCodigo,
                modalidade = (int)filtroAbrangencia.Modalidade,
                filtroAbrangencia.Semestre,
                mes
            };
            
            var retorno = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = multi.Read<AlunoFrequenciaTurmaEvasaoDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private void MontaQueryConsultaFrequenciaTurmaEvasaoAbaixo50Porcento(Paginacao paginacao, StringBuilder sql, int mes, FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, bool ehContador = false)
        {
            var ehConsolidacaoAcumulada = mes == 0;

            sql.AppendLine($@"select {(ehContador ? "count(distinct freqaluno.codigo_aluno)"
                                                  : $@"d.abreviacao as Dre,
                                                       coalesce(te.descricao || ' ', '') || coalesce(u.nome, '') as Ue,
                                                       {EnumExtensao.ObterCaseWhenSQL<Modalidade>("t.modalidade_codigo")}||'-'||t.nome as Turma,
                                                       freqaluno.nome_aluno || ' (' || freqaluno.codigo_aluno || ')' as Aluno,
                                                       freqaluno.percentual_frequencia as PercentualFrequencia")}
                          from frequencia_turma_evasao_aluno freqAluno
                          inner join frequencia_turma_evasao freq on freq.id = freqAluno.frequencia_turma_evasao_id
                          inner join turma t on t.id = freq.turma_id 
                          inner join ue u on u.id = t.ue_id 
                          inner join dre d on d.id = u.dre_id
                          left join tipo_escola te on te.cod_tipo_escola_eol = u.tipo_escola 
                          where t.ano_letivo = @anoLetivo 
                                and t.modalidade_codigo = @modalidade 
                                and freq.mes = @mes
                                {(!string.IsNullOrEmpty(filtroAbrangencia.DreCodigo) ? "and d.dre_id = @dreCodigo" : string.Empty)}
                                {(!string.IsNullOrEmpty(filtroAbrangencia.UeCodigo) ? "and u.ue_id = @ueCodigo" : string.Empty)}
                                {(!string.IsNullOrEmpty(filtroAbrangencia.TurmaCodigo) ? "and t.turma_id = @turmaCodigo" : string.Empty)}
                                {(filtroAbrangencia.Semestre > 0 ? "and t.semestre = @semestre" : string.Empty)}
                                {(ehConsolidacaoAcumulada ? "and freqaluno.percentual_frequencia < 50"
                                                          : "and freqaluno.percentual_frequencia < 50 and freqaluno.percentual_frequencia > 0")}
                          ");

            if (!ehContador)
                sql.AppendLine($@"order by d.dre_id, 
                                           coalesce(te.descricao || ' ', '') || coalesce(u.nome, ''),
                                           {EnumExtensao.ObterCaseWhenSQL<Modalidade>("t.modalidade_codigo")}||'-'||t.nome,
                                           freqaluno.nome_aluno || ' (' || freqaluno.codigo_aluno || ')' ");

            if (paginacao.QuantidadeRegistros > 0 && !ehContador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        public async Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaPaginado(int mes, FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, Paginacao paginacao)
        {
            var sql = new StringBuilder();
            MontaQueryConsultaFrequenciaTurmaEvasaoSemPresenca(paginacao, sql, filtroAbrangencia, mes);
            sql.AppendLine(";");
            MontaQueryConsultaFrequenciaTurmaEvasaoSemPresenca(paginacao, sql, filtroAbrangencia, mes, true);

            var parametros = new
            {
                filtroAbrangencia.AnoLetivo,
                filtroAbrangencia.DreCodigo,
                filtroAbrangencia.UeCodigo,
                filtroAbrangencia.TurmaCodigo,
                modalidade = (int)filtroAbrangencia.Modalidade,
                filtroAbrangencia.Semestre,
                mes
            };

            var retorno = new PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = multi.Read<AlunoFrequenciaTurmaEvasaoDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private void MontaQueryConsultaFrequenciaTurmaEvasaoSemPresenca(Paginacao paginacao, StringBuilder sql, FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, int mes, bool ehContador = false)
        {
            sql.AppendLine($@"select {(ehContador ? "count(distinct freqaluno.codigo_aluno)"
                                                  : $@"d.abreviacao as Dre,
                                                       coalesce(te.descricao || ' ', '') || coalesce(u.nome, '') as Ue,
                                                       {EnumExtensao.ObterCaseWhenSQL<Modalidade>("t.modalidade_codigo")}||'-'||t.nome as Turma,
                                                       freqaluno.nome_aluno || ' (' || freqaluno.codigo_aluno || ')' as Aluno,
                                                       freqaluno.percentual_frequencia as PercentualFrequencia")}
                          from frequencia_turma_evasao_aluno freqAluno
                          inner join frequencia_turma_evasao freq on freq.id = freqAluno.frequencia_turma_evasao_id
                          inner join turma t on t.id = freq.turma_id 
                          inner join ue u on u.id = t.ue_id 
                          inner join dre d on d.id = u.dre_id
                          left join tipo_escola te on te.cod_tipo_escola_eol = u.tipo_escola 
                          where t.ano_letivo = @anoLetivo 
                                and t.modalidade_codigo = @modalidade 
                                and freq.mes = @mes
                                {(!string.IsNullOrEmpty(filtroAbrangencia.DreCodigo) ? "and d.dre_id = @dreCodigo" : string.Empty)}
                                {(!string.IsNullOrEmpty(filtroAbrangencia.UeCodigo) ? "and u.ue_id = @ueCodigo" : string.Empty)}
                                {(!string.IsNullOrEmpty(filtroAbrangencia.TurmaCodigo) ? "and t.turma_id = @turmaCodigo" : string.Empty)}
                                {(filtroAbrangencia.Semestre > 0 ? "and t.semestre = @semestre" : string.Empty)}
                                and freqaluno.percentual_frequencia = 0
                          ");

            if (mes == 0)
                sql.AppendLine(@" and not exists (select 1 from frequencia_turma_evasao_aluno freqAlunoi
                          				      inner join frequencia_turma_evasao freqi on freqi.id = freqAlunoi.frequencia_turma_evasao_id
                          			          where	freqAlunoi.codigo_aluno = freqAluno.codigo_aluno 
                          			      	    and freqAlunoi.percentual_frequencia > 0
                          			      	    and freqi.turma_id = freq.turma_id)");

            if (!ehContador)
                sql.AppendLine($@"order by d.dre_id, 
                                           coalesce(te.descricao || ' ', '') || coalesce(u.nome, ''),
                                           {EnumExtensao.ObterCaseWhenSQL<Modalidade>("t.modalidade_codigo")}||'-'||t.nome,
                                           freqaluno.nome_aluno || ' (' || freqaluno.codigo_aluno || ')' ");

            if (paginacao.QuantidadeRegistros > 0 && !ehContador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        public Task<IEnumerable<RegistroFrequenciaProdutividadeDto>> ObterInformacoesProdutividadeFrequencia(int anoLetivo, string ueCodigo, int bimestre)
        => database.Conexao
                .QueryAsync<RegistroFrequenciaProdutividadeDto>(@"
                                    select t.turma_id as TurmaCodigo
                                           ,t.nome as TurmaNome 
                                           ,t.modalidade_codigo as Modalidade
                                           ,ue.ue_id as UeCodigo
                                           ,ue.nome as UeNome
                                           ,ue.tipo_escola as TipoEscola
                                           ,dre.dre_id as DreCodigo
                                           ,dre.abreviacao  as DreAbreviacao
                                           ,rf.criado_em as DataFrequencia
	                                       ,a.data_aula as DataAula
	                                       ,DATE_PART('day', rf.criado_em - a.data_aula) as DifDias
	                                       ,pe.bimestre 
                                           ,t.ano_letivo as AnoLetivo
                                           ,a.disciplina_id as ComponenteCodigo
                                           ,coalesce(cc.descricao_infantil , cc.descricao_sgp, cc.descricao) as ComponenteNome
                                           ,rf.criado_rf as ProfRf
                                           ,rf.criado_por  as ProfNome
                                    from aula a 
                                    inner join registro_frequencia rf on rf.aula_id = a.id and not rf.excluido 
                                    inner join turma t on t.turma_id = a.turma_id
                                    inner join ue on ue.id = t.ue_id 
                                    inner join dre on dre.id = ue.dre_id 
                                    inner join periodo_escolar pe on pe.tipo_calendario_id  = a.tipo_calendario_id 
                                    left join componente_curricular cc on cc.id = a.disciplina_id::int8
                                    where not a.excluido and not rf.excluido
                                      and t.ano_letivo = @anoLetivo	
                                      and pe.bimestre = @bimestre
                                      and a.data_aula between pe.periodo_inicio and periodo_fim
                                      and ue.ue_id = @ueCodigo
                                      and DATE_PART('day', rf.criado_em - a.data_aula) > 0;", new { ueCodigo, anoLetivo, bimestre });

        public async Task<IEnumerable<RegistroFrequenciaPainelEducacionalDto>> ObterInformacoesFrequenciaPainelEducacional(int anoLetivo)
        {
            var query = @"SELECT cfam.id,
                                 cfam.aluno_codigo as CodigoAluno,
                                 d.dre_id AS DreCodigo,
                                 u.ue_id AS CodigoUe
                                 cfam.mes,
                                 cfam.percentual,
                                 cfam.quantidade_aulas as QuantidadeAulas,
                                 cfam.quantidade_ausencias as QuantidadeAusencias,
                                 cfam.quantidade_compensacoes as QuantidadeCompensacoes,
                                 t.modalidade_codigo as ModalidadeCodigo,
                                 t.ano_letivo AS AnoLetivo,
                                 t.ano AS AnoTurma
                            FROM consolidacao_frequencia_aluno_mensal cfam
                            INNER JOIN turma t ON t.id = cfam.turma_id
                            INNER JOIN ue u ON u.id = t.ue_id
                            INNER JOIN tipo_escola te ON te.cod_tipo_escola_eol = u.tipo_escola
                            INNER JOIN dre d ON d.id = u.dre_id
                            WHERE t.ano_letivo =  @anoLetivo";

            var parametros = new
            {
                anoLetivo
            };

            return await database.Conexao.QueryAsync<RegistroFrequenciaPainelEducacionalDto>(query, parametros);
        }
    }
}