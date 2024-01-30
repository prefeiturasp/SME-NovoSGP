using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseConsulta : RepositorioBase<ConselhoClasse>, IRepositorioConselhoClasseConsulta
    {
        public RepositorioConselhoClasseConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<ConselhoClasse> ObterPorFechamentoId(long fechamentoTurmaId)
        {
            var query = @"select c.* 
                            from conselho_classe c 
                           where c.fechamento_turma_id = @fechamentoTurmaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasse>(query, new { fechamentoTurmaId });
        }

        public async Task<ConselhoClasse> ObterConselhoClassePorId(long conselhoClasseId)
        {
            var query = @"select c.* 
                            from conselho_classe c 
                           where c.id = @conselhoClasseId";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasse>(query, new { conselhoClasseId });
        }

        public async Task<IEnumerable<long>> ObterConselhoClasseIdsPorTurmaEPeriodoAsync(string[] turmasCodigos, long? periodoEscolarId = null)
        {
            var query = new StringBuilder(@"select c.id 
                            from conselho_classe c 
                            inner join fechamento_turma ft on ft.id = c.fechamento_turma_id
                            inner join turma t on t.id = ft.turma_id
                            where t.turma_id = ANY(@turmasCodigos) ");

            if (periodoEscolarId.HasValue)
                query.AppendLine("and ft.periodo_escolar_id = @periodoEscolarId");
            else
                query.AppendLine("and ft.periodo_escolar_id is null");

            return await database.Conexao.QueryAsync<long>(query.ToString(), new { turmasCodigos, periodoEscolarId });
        }

        public async Task<IEnumerable<long>> ObterConselhoClasseIdsPorTurmaEBimestreAsync(string[] turmasCodigos, long? bimestre = null)
        {
            var query = new StringBuilder(@"select c.id 
                            from conselho_classe c 
                            inner join fechamento_turma ft on ft.id = c.fechamento_turma_id
                            inner join turma t on t.id = ft.turma_id
                            left join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                            where t.turma_id = ANY(@turmasCodigos) ");

            if ((bimestre?? 0) > 0)
                query.AppendLine("and pe.bimestre = @bimestre");
            else
                query.AppendLine("and ft.periodo_escolar_id is null");

            return await database.Conexao.QueryAsync<long>(query.ToString(), new { turmasCodigos, bimestre });
        }

        public async Task<ConselhoClasse> ObterPorTurmaEPeriodoAsync(long turmaId, long? periodoEscolarId = null)
        {
            var query = new StringBuilder(@"select c.* 
                                            from conselho_classe c 
                                                inner join fechamento_turma t on t.id = c.fechamento_turma_id
                                                    and not t.excluido
                                            where t.turma_id = @turmaId 
                                            and not c.excluido");

            query.AppendLine(periodoEscolarId.HasValue
                ? " and t.periodo_escolar_id = @periodoEscolarId"
                : " and t.periodo_escolar_id is null");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasse>(query.ToString(), new { turmaId, periodoEscolarId });
        }

        public async Task<ConselhoClasse> ObterPorAlunoEPeriodoAsync(string alunoCodigo, long? periodoEscolarId = null)
        {
            var query = new StringBuilder(@"select c.* 
                            from conselho_classe c 
                           inner join fechamento_turma t on t.id = c.fechamento_turma_id
                           inner join conselho_classe_aluno cca on cca.conselho_classe_id = c.id ");

            if (periodoEscolarId.HasValue)
                query.AppendLine("where t.periodo_escolar_id = @periodoEscolarId");
            else
                query.AppendLine("where t.periodo_escolar_id is null");

            if (!String.IsNullOrEmpty(alunoCodigo))
                query.AppendLine(" and cca.aluno_codigo = @alunoCodigo");

            return await database.Conexao.QueryFirstOrDefaultAsync<ConselhoClasse>(query.ToString(), new { periodoEscolarId, alunoCodigo });
        }

        public async Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> ObterBimestreComConselhoClasseTurmaAsync(long turmaId)
        {
            var query = new StringBuilder(@"select          
                                               min(cc.id) as conselhoClasseId,
                                               cc.fechamento_turma_id as fechamentoTurmaId,
                                               coalesce(pe.bimestre, 0) as bimestre
                                          from fechamento_turma ft 
                                          inner join conselho_classe cc on
                                           ft.id = cc.fechamento_turma_id 
                                          left join periodo_escolar pe on 
                                           ft.periodo_escolar_id = pe.id  
                                          where ft.turma_id = @turmaId 
                                        group by ft.turma_id,cc.fechamento_turma_id, pe.bimestre
                                        order by pe.bimestre ");
            return await database.Conexao.QueryAsync<BimestreComConselhoClasseTurmaDto>(query.ToString(), new { turmaId });
        }

        public async Task<IEnumerable<string>> ObterAlunosComNotaLancadaPorConselhoClasseId(long conselhoClasseId)
        {
            var query = @"select distinct cca.aluno_codigo
                          from conselho_classe_aluno cca
                          inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                         where cca.conselho_classe_id = @conselhoClasseId";

            return await database.Conexao.QueryAsync<string>(query, new { conselhoClasseId });
        }

        public async Task<SituacaoConselhoClasse> ObterSituacaoConselhoClasse(long turmaId, long periodoEscolarId)
        {
            var query = @"select cc.situacao
                        from conselho_classe cc
                       inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                       where ft.turma_id = @turmaId
                        and ft.periodo_escolar_id = @periodoEscolarId";

            return (SituacaoConselhoClasse)await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, periodoEscolarId });
        }

        public async Task<IEnumerable<ConselhoClasseSituacaoQuantidadeDto>> ObterConselhoClasseSituacao(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = MontarQueryConselhoClasseSituacaoQuantidade(ueId, ano, dreId, modalidade, semestre, bimestre);
            return await database.Conexao.QueryAsync<ConselhoClasseSituacaoQuantidadeDto>(query.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }

        private string MontarQueryConselhoClasseSituacaoQuantidade(long ueId,
           int ano, long dreId, int modalidade, int semestre, int bimestre)
        {

            var sqlQuery = new StringBuilder($@"select Situacao, 
                                                       sum(x.Quantidade) as Quantidade,
                                                       x.AnoTurma,
                                                       x.CodigoTurma
                                                  from (
                                                        select  cccat.status as Situacao,
                                                                count(distinct cccat.aluno_codigo) as Quantidade, ");
            if (ueId > 0)
                sqlQuery.AppendLine(" t.nome as AnoTurma, t.turma_id as CodigoTurma ");
            else
                sqlQuery.AppendLine(" t.ano as AnoTurma ");
            sqlQuery.AppendLine(@" 
                                  from consolidado_conselho_classe_aluno_turma cccat
                                 inner join consolidado_conselho_classe_aluno_turma_nota cccatn on cccatn.consolidado_conselho_classe_aluno_turma_id = cccat.id
                                 inner join conselho_classe_aluno cca on cca.aluno_codigo = cccat.aluno_codigo
                                 inner join conselho_classe cc on cc.id = cca.conselho_classe_id 
                                 inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id 
                                 inner join turma t on t.id = ft.turma_id and cccat.turma_id = t.id 
                                 inner join ue on ue.id = t.ue_id where t.tipo_turma = 1 ");

            var queryWhere = new StringBuilder("");

            if (ano > 0)
            {
                queryWhere.AppendLine(" and t.ano_letivo = @ano ");
            }

            if (ueId > 0)
            {
                queryWhere.AppendLine(" and t.ue_id = @ueId ");
            }

            if (dreId > 0)
            {
                queryWhere.AppendLine(" and ue.dre_id = @dreId ");
            }

            if (modalidade > 0)
            {
                queryWhere.AppendLine(" and t.modalidade_codigo = @modalidade ");
            }

            if (semestre > 0)
            {
                queryWhere.AppendLine(" and t.semestre = @semestre ");
            }

            if (bimestre >= 0)
            {
                queryWhere.AppendLine(" and coalesce(cccatn.bimestre, 0) = @bimestre ");
            }

            queryWhere.AppendLine(" and not cccat.excluido");

            sqlQuery.AppendLine(queryWhere.ToString());
            sqlQuery.AppendLine($@" group by cccat.status, ");
            if (ueId > 0)
                sqlQuery.AppendLine(" t.nome, t.turma_id ");
            else
                sqlQuery.AppendLine(" t.ano ");

            sqlQuery.AppendLine(@") x group by x.Situacao, x.AnoTurma, x.CodigoTurma
                                   order by x.AnoTurma;");


            return sqlQuery.ToString();
        }

        public async Task<IEnumerable<FechamentoConselhoClasseNotaFinalDto>> ObterNotasFechamentoOuConselhoAlunos(long ueId, int anoLetivo, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder(@"select
                                            x.TurmaAnoNome,
                                            x.Ano,
                                            x.Bimestre,
                                            x.ComponenteCurricularCodigo,
                                            x.ConselhoClasseNotaId,
                                            x.ConceitoId,
                                            x.Nota,
                                            x.AlunoCodigo,
                                            x.Conceito,
                                            row_number() over(partition by x.TurmaAnoNome, x.ComponenteCurricularCodigo, x.AlunoCodigo
                                            order by x.Prioridade) as linha ");
            query.AppendLine("from f_obternotasfechamentoouconselhoalunos(@anoLetivo, @dreId, @ueId, @modalidade, @semestre, @bimestre) x");

            var parametros = new
            {
                ueId,
                anoLetivo,
                dreId,
                modalidade,
                semestre,
                bimestre
            };
            return await database.Conexao.QueryAsync<FechamentoConselhoClasseNotaFinalDto>(query.ToString(), parametros, commandTimeout: 120);
        }

        public async Task<IEnumerable<objConsolidacaoConselhoAluno>> ObterAlunosReprocessamentoConsolidacaoConselho(int dreId)
        {
            var query = new StringBuilder($@"select t.id as turmaId, pe.bimestre, cca.aluno_codigo as alunoCodigo   from conselho_classe_aluno cca
                          inner join conselho_classe cc on cc.id = cca.conselho_classe_id
                          inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                          inner join turma t on t.id = ft.turma_id
                          inner join ue ue on ue.id = t.ue_id 
                          inner join dre dre on dre.id = ue.dre_id 
                          inner join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                          inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                          where  t.ano_letivo = {DateTime.Now.Year}
                          and dre.id = @dreId 
                          and tc.modalidade in (1,2)");

            return await database.Conexao
                .QueryAsync<objConsolidacaoConselhoAluno>(query.ToString(), new { dreId });
        }


        public async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> ObterTotalAulasPorAlunoTurma(string disciplinaId, string codigoTurma)
        {
            var sql = @"select disciplina_id as disciplinaid,total_aulas as totalaulas, codigo_aluno as codigoaluno from frequencia_aluno fa 
                        where tipo = 1 
                        and disciplina_id = @disciplinaId 
                        and turma_id =@codigoTurma ";

            return await database.Conexao.QueryAsync<TotalAulasPorAlunoTurmaDto>(sql, new { disciplinaId, codigoTurma }, commandTimeout: 60);
        }

        public async Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> ObterTotalAulasSemFrequenciaPorTurma(string disciplinaId, string codigoTurma)
        {
            var dataAtual = DateTime.Now.ToString("yyyy-MM-dd");

            var sql = $@"select disciplina_id as disciplinaid,SUM(quantidade) as totalaulas from aula a
                        left join componente_curricular cc on cc.id = a.disciplina_id::int8 
                        where coalesce(cc.permite_lancamento_nota, false)  = false 
                        and a.turma_id = @codigoTurma
                        and a.data_aula <= '{dataAtual}'
                        and not a.excluido 
                        and a.disciplina_id =@disciplinaId
                        group by a.disciplina_id";

            return await database.Conexao.QueryAsync<TotalAulasPorAlunoTurmaDto>(sql, new { codigoTurma, disciplinaId }, commandTimeout: 60);
        }

        public async Task<IEnumerable<TotalAulasNaoLancamNotaDto>> ObterTotalAulasNaoLancamNotaPorBimestreTurma(string codigoTurma, int bimestre, string codigoAluno)
        {
            var sql = @"select fa.disciplina_id as DisciplinaID,total_aulas as TotalAulas from frequencia_aluno fa 
                        left join componente_curricular cc on cc.id = fa.disciplina_id::int8
                        where coalesce(cc.permite_lancamento_nota, false) = false 
                        and fa.turma_id = @codigoTurma
                        and fa.bimestre = @bimestre
                        and fa.tipo  = @tipo
                        and fa.codigo_aluno = @codigoAluno
                        group by fa.disciplina_id, total_aulas ";

            return await database.Conexao
                                .QueryAsync<TotalAulasNaoLancamNotaDto>(sql, new { codigoTurma, bimestre, tipo = (int)TipoAula.Normal, codigoAluno }, commandTimeout: 60);
        }

        public async Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> ObterTotalCompensacoesComponenteNaoLancaNotaPorBimestre(string codigoTurma, int bimestre)
        {
            var sql = @"select fa.disciplina_id as DisciplinaID,total_compensacoes as TotalCompensacoes, codigo_aluno as CodigoAluno from frequencia_aluno fa 
                        left join componente_curricular cc on cc.id = fa.disciplina_id::int8
                        where coalesce(cc.permite_lancamento_nota, false) = false 
                        and fa.turma_id = @codigoTurma
                        and fa.bimestre = @bimestre
                        and fa.tipo  = @tipo
                        group by fa.disciplina_id, total_compensacoes, codigo_aluno ";

            return await database.Conexao.QueryAsync<TotalCompensacoesComponenteNaoLancaNotaDto>(sql, new { codigoTurma, bimestre, tipo = (int)TipoFrequenciaAluno.PorDisciplina }, commandTimeout: 60);
        }

        public async Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> ObterTotalCompensacoesComponenteNaoLancaNota(string codigoTurma)
        {
            var sql = @"select fa.disciplina_id as DisciplinaID,total_compensacoes as TotalCompensacoes, codigo_aluno as CodigoAluno from frequencia_aluno fa 
                        left join componente_curricular cc on cc.id = fa.disciplina_id::int8
                        where coalesce(cc.permite_lancamento_nota, false) = false 
                        and fa.turma_id = @codigoTurma
                        and fa.tipo  = @tipo
                        group by fa.disciplina_id, total_compensacoes, codigo_aluno, fa.bimestre ";

            return await database.Conexao.QueryAsync<TotalCompensacoesComponenteNaoLancaNotaDto>(sql, new { codigoTurma, tipo = (int)TipoAula.Normal }, commandTimeout: 60);
        }

        public async Task<IEnumerable<int>> ObterTotalAulasSemFrequenciaPorTurmaBimestre(string disciplinaId, string codigoTurma, int bimestre, DateTime dataMatricula)
        {
            var sql = @"select COALESCE(SUM(quantidade), 0) as totalaulas from aula a
                        join  componente_curricular cc on cc.id = a.disciplina_id::int8 
                        join  periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id::int8  
                        where cc.permite_registro_frequencia = false 
                        and a.turma_id = @codigoTurma
                        and not a.excluido 
                        and a.disciplina_id = @disciplinaId
                        and pe.bimestre = @bimestre
                        and pe.periodo_inicio <= a.data_aula and pe.periodo_fim >= a.data_aula
                        and a.data_aula > @dataMatricula";

            return await database.Conexao.QueryAsync<int>(sql, new { disciplinaId, codigoTurma, bimestre, dataMatricula });
        }

        public async Task<bool> ExisteConselhoDeClasseParaTurma(string[] codigosTurmas, int bimestre)
        {
            var sql = new StringBuilder(@"select 1 from conselho_classe cc
                    left join fechamento_turma ft on cc.fechamento_turma_id = ft.id
                    left join turma t on t.id = ft.turma_id
                    left join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                    where t.turma_id = any(@codigosTurmas)");
            if (bimestre > 0)
                sql.AppendLine("and pe.bimestre = @bimestre");
            else
                sql.AppendLine("and ft.periodo_escolar_id is null");

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql.ToString(), new { codigosTurmas, bimestre });
        }
    }
}
