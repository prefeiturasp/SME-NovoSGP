using Dapper;
using Polly;
using Polly.Registry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(int anoLetivo,
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

        private async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorDre(int anoLetivo, Modalidade modalidade,
            int semestre, int mes)
        {
            var query = @"select d.Abreviacao as Descricao,
                                sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                            where t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_abaixo_50_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes ";

            if (semestre > 0)
                query += " and t.semestre = @semestre ";

            query += @" group by d.dre_id, d.Abreviacao
                        order by d.dre_id ";

            var parametros = new { modalidade, semestre, anoLetivo, mes };

            var resultado = (await database.Conexao.QueryAsync<GraficoFrequenciaTurmaEvasaoDto>(query, parametros)).ToList();
            resultado.ForEach(c => c.Descricao = c.Descricao.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim());

            return resultado;
        }

        private async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorUe(int anoLetivo, string dreCodigo,
            Modalidade modalidade, int semestre, int mes)
        {
            var query = @"select coalesce(te.descricao || '-', '') || coalesce(u.nome, '') as Descricao,
                                sum(fte.quantidade_alunos_abaixo_50_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                                left join tipo_escola te on te.cod_tipo_escola_eol = u.tipo_escola
                            where d.dre_id = @dreCodigo
                            and t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_abaixo_50_porcento > 0)
                            and t.ano_letivo = @anoLetivo
                            and fte.mes = @mes";

            if (semestre > 0)
                query += " and t.semestre = @semestre ";

            query += @" group by te.descricao, u.nome
                        order by te.descricao, u.nome ";

            var parametros = new { dreCodigo, modalidade, semestre, anoLetivo, mes };

            return await database.Conexao.QueryAsync<GraficoFrequenciaTurmaEvasaoDto>(query, parametros);
        }

        private async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoAgrupadoPorTurma(int anoLetivo, string dreCodigo,
            string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            var query = @"WITH TurmaEvasao AS
                          (SELECT DISTINCT turma_id,
                                           mes,
                                           quantidade_alunos_abaixo_50_porcento
                           FROM frequencia_turma_evasao)";
            query += @" SELECT t.nome AS Descricao,
                               sum(fte.quantidade_alunos_abaixo_50_porcento) AS Quantidade
                        FROM TurmaEvasao fte
                        INNER JOIN turma t ON t.id = fte.turma_id
                        INNER JOIN ue u ON u.id = t.ue_id
                        INNER JOIN dre d ON d.id = u.dre_id
                        WHERE d.dre_id = @dreCodigo
                          AND u.ue_id = @ueCodigo
                          AND t.modalidade_codigo = @modalidade
                          AND (fte.quantidade_alunos_abaixo_50_porcento>0)
                          AND t.ano_letivo = @anoLetivo
                          AND fte.mes = @mes";

            if (semestre > 0)
                query += " and t.semestre = @semestre ";            

            query += @" group by t.nome
                        order by t.nome ";

            var parametros = new { dreCodigo, ueCodigo, modalidade, semestre, anoLetivo, mes };

            var resultado = (await database.Conexao.QueryAsync<GraficoFrequenciaTurmaEvasaoDto>(query, parametros)).ToList();
            resultado.ForEach(c => c.Descricao = string.Concat(modalidade.ShortName(), " - ", c.Descricao));

            return resultado;
        }

        public async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoSemPresenca(int anoLetivo, string dreCodigo,
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

        private async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorDre(int anoLetivo,
            Modalidade modalidade, int semestre, int mes)
        {
            var query = @"select d.Abreviacao as Descricao,
                                sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                            where t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_0_porcento > 0)
                            and t.ano_letivo = @anoLetivo";

            if (semestre > 0)
                query += " and t.semestre = @semestre ";

            if (mes > 0)
                query += " and fte.mes = @mes ";

            query += @" group by d.dre_id, d.Abreviacao
                        order by d.dre_id ";

            var parametros = new { modalidade, semestre, anoLetivo, mes };

            var resultado = (await database.Conexao.QueryAsync<GraficoFrequenciaTurmaEvasaoDto>(query, parametros)).ToList();
            resultado.ForEach(c => c.Descricao = c.Descricao.Replace(DashboardConstants.PrefixoDreParaSerRemovido, string.Empty).Trim());

            return resultado;
        }

        private async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorUe(int anoLetivo, string dreCodigo,
            Modalidade modalidade, int semestre, int mes)
        {
            var query = @"select coalesce(te.descricao || '-', '') || coalesce(u.nome, '') as Descricao,
                                 te.descricao as DescricaoTipo,
                                sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from frequencia_turma_evasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                                left join tipo_escola te on te.cod_tipo_escola_eol = u.tipo_escola
                            where d.dre_id = @dreCodigo
                            and t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_0_porcento > 0)
                            and t.ano_letivo = @anoLetivo";

            if (semestre > 0)
                query += " and t.semestre = @semestre ";

            if (mes > 0)
                query += " and fte.mes = @mes ";

            query += @" group by te.descricao, u.nome
                        order by te.descricao, u.nome ";

            var parametros = new { dreCodigo, modalidade, semestre, anoLetivo, mes };

            return await database.Conexao.QueryAsync<GraficoFrequenciaTurmaEvasaoDto>(query, parametros);
        }

        private async Task<IEnumerable<GraficoFrequenciaTurmaEvasaoDto>> ObterDashboardFrequenciaTurmaEvasaoSemPresencaAgrupadoPorTurma(int anoLetivo, string dreCodigo,
            string ueCodigo, Modalidade modalidade, int semestre, int mes)
        {
            var query = @" WITH TurmaEvasao AS
                          (SELECT DISTINCT turma_id,
                                           mes,
                                           quantidade_alunos_0_porcento
                           FROM frequencia_turma_evasao)
                            select t.nome as Descricao,
                                sum(fte.quantidade_alunos_0_porcento) as Quantidade
                            from TurmaEvasao fte
                                inner join turma t on t.id = fte.turma_id 
                                inner join ue u on u.id = t.ue_id 
                                inner join dre d on d.id = u.dre_id 
                            where d.dre_id = @dreCodigo
                            and u.ue_id = @ueCodigo
                            and t.modalidade_codigo = @modalidade
                            and (fte.quantidade_alunos_0_porcento > 0)
                            and t.ano_letivo = @anoLetivo";

            if (semestre > 0)
                query += " and t.semestre = @semestre ";

            if (mes > 0)
                query += " and fte.mes = @mes ";

            query += @" group by t.nome
                        order by t.nome ";

            var parametros = new { dreCodigo, ueCodigo, modalidade, semestre, anoLetivo, mes };

            var resultado = (await database.Conexao.QueryAsync<GraficoFrequenciaTurmaEvasaoDto>(query, parametros)).ToList();
            resultado.ForEach(c => c.Descricao = string.Concat(modalidade.ShortName(), " - ", c.Descricao));

            return resultado;
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
    }
}