using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioFechamentoTurmaDisciplinaConsulta : RepositorioBase<FechamentoTurmaDisciplina>, IRepositorioFechamentoTurmaDisciplinaConsulta
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public RepositorioFechamentoTurmaDisciplinaConsulta(ISgpContextConsultas database, IRepositorioTurmaConsulta repositorioTurma, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<int>> ObterDisciplinaIdsPorTurmaIdBimestre(long turmaId, int bimestre)
        {
            var query = new StringBuilder(
                @"select disciplina_id as ComponenteCurricularId from fechamento_turma_disciplina ftd 
                            inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                            left join periodo_escolar pe on ft.periodo_escolar_id = pe.id 
                            where ft.turma_id = @turmaId");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");

            return await database.Conexao.QueryAsync<int>(query.ToString(), new { turmaId, bimestre });
        }

        public async Task<bool> AtualizarSituacaoFechamento(long fechamentoTurmaDisciplinaId, int situacaoFechamento)
        {
            var query = @"update fechamento_turma_disciplina 
                             set situacao = @situacaoFechamento
                         where id = @fechamentoTurmaDisciplinaId";

            await database.Conexao.ExecuteAsync(query, new { fechamentoTurmaDisciplinaId, situacaoFechamento });
            return true;
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(long turmaId,
            long[] disciplinasId, int bimestre = 0, long? tipoCalendario = null)
        {
            var query = new StringBuilder(@"with lista as (
                        select f.*, fa.*, ft.*, p.*,
                            row_number() over (partition by t.id, fa.aluno_codigo, p.id, f.disciplina_id order by f.id desc) sequencia
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id 
                        inner join turma t on t.id = ft.turma_id
                        inner join fechamento_aluno fa on f.id = fa.fechamento_turma_disciplina_id
                        left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
                        left join componente_curricular cc on cc.id = fn.disciplina_id
                        where t.id = @turmaId ");

            if (disciplinasId.NaoEhNulo() && disciplinasId.Length > 0)
                query.AppendLine("and (f.disciplina_id = ANY(@disciplinasId) or cc.id = ANY(@disciplinasId))");

            if (bimestre > 0)
                query.AppendLine("and p.bimestre = @bimestre");
            else if (bimestre == 0)
                query.AppendLine(tipoCalendario.HasValue ? " and (ft.periodo_escolar_id is null  or p.tipo_calendario_id = @tipoCalendario)" : " and (ft.periodo_escolar_id is null or p.bimestre in (1,2,3,4))");

            if (bimestre > 0 && tipoCalendario.HasValue)
                query.AppendLine(" and p.tipo_calendario_id = @tipoCalendario");

            query.AppendLine(") select * from lista where sequencia = 1;");

            IList<FechamentoTurmaDisciplina> fechammentosTurmaDisciplina = new List<FechamentoTurmaDisciplina>();

            await database.Conexao
                .QueryAsync<FechamentoTurmaDisciplina, FechamentoAluno, FechamentoTurma, PeriodoEscolar,
                    FechamentoTurmaDisciplina>
                (query.ToString(), (fechamentoTurmaDiscplina, fechamentoAluno, fechamentoTurma, periodoEscolar) =>
                {
                    var fechamentoTurmaDisciplinaLista =
                        fechammentosTurmaDisciplina.FirstOrDefault(ftd => ftd.Id == fechamentoTurmaDiscplina.Id);
                    if (fechamentoTurmaDisciplinaLista.EhNulo())
                    {
                        if (periodoEscolar.NaoEhNulo())
                            fechamentoTurma.AdicionarPeriodoEscolar(periodoEscolar);

                        fechamentoTurmaDiscplina.FechamentoTurma = fechamentoTurma;

                        fechamentoTurmaDisciplinaLista = fechamentoTurmaDiscplina;
                        fechammentosTurmaDisciplina.Add(fechamentoTurmaDiscplina);
                    }

                    fechamentoTurmaDisciplinaLista.FechamentoAlunos.Add(fechamentoAluno);
                    return fechamentoTurmaDiscplina;
                }, new { turmaId, disciplinasId, bimestre, tipoCalendario });

            return fechammentosTurmaDisciplina;
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentosTurmaDisciplinas(string turmaCodigo,
            long[] disciplinasId, int bimestre = 0)
        {
            var turma = await repositorioTurma.ObterPorCodigo(turmaCodigo);

            if (turma.EhNulo())
                return Enumerable.Empty<FechamentoTurmaDisciplina>();

            return await ObterFechamentosTurmaDisciplinas(turma.Id, disciplinasId, bimestre);
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplina(string turmaCodigo,
            long disciplinaId, int bimestre = 0)
        {
            var query = new StringBuilder(@"select f.*
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id
                        inner join turma t on t.id = ft.turma_id
                        where t.turma_id = @turmaCodigo
                          and f.disciplina_id = @disciplinaId ");
            if (bimestre > 0)
                query.AppendLine(" and p.bimestre = @bimestre ");
            else
                query.AppendLine(" and ft.periodo_escolar_id is null ");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurmaDisciplina>(query.ToString(),
                new { turmaCodigo, disciplinaId, bimestre });
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>> ObterFechamentoTurmaDisciplinaPorTurmaidDisciplinaId(string turmaCodigo, long disciplinaId, int? bimestre = 0)
        {
            var query = new StringBuilder(@"select f.*
                         from fechamento_turma_disciplina f
                        inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                         left join periodo_escolar p on p.id = ft.periodo_escolar_id
                        inner join turma t on t.id = ft.turma_id
                        where t.turma_id = @turmaCodigo
                          and f.disciplina_id = @disciplinaId");
            if (bimestre > 0)
                query.AppendLine(" and p.bimestre = @bimestre ");

            return await database.Conexao.QueryAsync<FechamentoTurmaDisciplina>(query.ToString(),
                new { turmaCodigo, disciplinaId, bimestre });
        }


        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestre(string codigoAluno,
            long fechamentoTurmaDisciplinaId)
        {
            var query = @"select n.disciplina_id as DisciplinaId, 
                                 n.nota as Nota, 
                                 n.conceito_id as ConceitoId, 
                                 fa.aluno_codigo as CodigoAluno, 
                                 n.sintese_id as SinteseId,
                                 n.criado_em CriadoEm,
                                 n.criado_rf CriadoRf,
                                 n.criado_por CriadoPor,
                                 n.alterado_em AlteradoEm,
                                 n.alterado_rf AlteradoRf,
                                 n.alterado_por AlteradoPor
                         from fechamento_nota n
                        inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                        where fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                          and fa.aluno_codigo = @codigoAluno ";

            return await database.Conexao.QueryAsync<FechamentoNotaDto>(query,
                new { codigoAluno, fechamentoTurmaDisciplinaId });
        }
        public async Task<IEnumerable<FechamentoNotaDto>> ObterNotasBimestrePorCodigosAlunosIdsFechamentos(string[] codigoAluno, long[] fechamentoTurmaDisciplinaId)
        {
            var query = @"select n.disciplina_id as DisciplinaId, 
                                 n.nota as Nota, 
                                 n.conceito_id as ConceitoId, 
                                 fa.aluno_codigo as CodigoAluno, 
                                 n.sintese_id as SinteseId,
                                 n.criado_em CriadoEm,
                                 n.criado_rf CriadoRf,
                                 n.criado_por CriadoPor,
                                 n.alterado_em AlteradoEm,
                                 n.alterado_rf AlteradoRf,
                                 n.alterado_por AlteradoPor,
                                 fa.fechamento_turma_disciplina_id as FechamentoId
                        from fechamento_nota n
                        inner join fechamento_aluno fa on fa.id = n.fechamento_aluno_id
                        where fa.fechamento_turma_disciplina_id = any(@fechamentoTurmaDisciplinaId)
                          and fa.aluno_codigo = any(@codigoAluno) ";

            return await database.Conexao.QueryAsync<FechamentoNotaDto>(query,
                new { codigoAluno, fechamentoTurmaDisciplinaId });
        }

        public async Task<FechamentoTurmaDisciplina> ObterFechamentoTurmaDisciplinaPorId(long id)
        {
            var query = @"select ftd.*, ft.* 
                            from fechamento_turma_disciplina ftd
                          inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                          where ftd.id = @id";

            var retorno = await database.Conexao.QueryAsync<FechamentoTurmaDisciplina, FechamentoTurma, FechamentoTurmaDisciplina>(query
                , (fechamentoTurmaDisciplina, fechamentoTurma) =>
                {
                    fechamentoTurmaDisciplina.FechamentoTurma = fechamentoTurma;
                    return fechamentoTurmaDisciplina;
                }
                , new { id });

            return retorno.FirstOrDefault();
        }

        public async Task<SituacaoFechamento> ObterSituacaoFechamento(long turmaId, long componenteCurricularId,
            long periodoEscolarId)
        {
            var query = @"select ftd.situacao
                         from fechamento_turma_disciplina ftd
                        inner join fechamento_turma ft on ft.id = ftd.fechamento_turma_id
                        where ftd.disciplina_id = @componenteCurricularId
                          and ft.turma_id = @turmaId
                          and ft.periodo_escolar_id = @periodoEscolarId ";

            return (SituacaoFechamento)await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaId, componenteCurricularId, periodoEscolarId });
        }

        public async Task<IEnumerable<FechamentoTurmaDisciplina>>
            ObterFechamentosComSituacaoEmProcessamentoPorAnoLetivo(int anoLetivo)
        {
            var sqlQuery = @"select distinct ftd.*
                             from fechamento_turma_disciplina ftd 
                                inner join fechamento_turma ft
                                    on ftd.fechamento_turma_id = ft.id
                                inner join turma t
                                    on ft.turma_id = t.id
                             where t.ano_letivo = @anoLetivo and
                                  ftd.situacao = @situacao";

            return await database.Conexao.QueryAsync<FechamentoTurmaDisciplina>(sqlQuery,
                new { anoLetivo, situacao = SituacaoFechamento.EmProcessamento });
        }

        public async Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamento(long ueId,
            int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = ueId > 0 ? MontarQuerySituacaoProcessoFechamento(ueId, ano, dreId, modalidade, semestre, bimestre) : MontarQuerySituacaoProcessoFechamentoSME(ano, dreId, modalidade, semestre, bimestre);
            return await database.Conexao.QueryAsync<FechamentoSituacaoQuantidadeDto>(query.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }

        private string MontarQuerySituacaoProcessoFechamento(long ueId,
            int ano, long dreId, int modalidade, int semestre, int bimestre)
        {

            var sqlQuery = $@"select Situacao, sum(x.Quantidade) as Quantidade,
                                    x.AnoTurma,
                                    x.Ano,
                                    x.Modalidade
                               from (
                                     select  case  when cfct.status in (0, 1) then 0 else cfct.status end as Situacao,
                                               count(cfct.id) as Quantidade, 
                                               t.ano as Ano,                                                 
                                               {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")} as AnoTurma,
                                               t.modalidade_codigo  as Modalidade
                                          from consolidado_fechamento_componente_turma cfct 
                                         inner join turma t on t.id = cfct.turma_id
                                         inner join ue on ue.id = t.ue_id where t.tipo_turma in (1,2,7) ";

            var queryBuilder = new StringBuilder(sqlQuery);

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
                queryWhere.AppendLine(" and cfct.bimestre = @bimestre ");
            }

            queryBuilder.AppendLine(queryWhere.ToString());
            queryBuilder.AppendLine($@" group by cfct.status, t.ano,{(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")}, t.modalidade_codigo) x
                                   group by x.Situacao, x.Ano, x.AnoTurma, x.Modalidade
                                   order by x.Ano;");


            return queryBuilder.ToString();
        }

        private string MontarQuerySituacaoProcessoFechamentoSME(int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var sqlQuery = $@"select Situacao, sum(x.Quantidade) as Quantidade,
                                    x.AnoTurma,
                                    x.Ano,
                                    x.Modalidade
                               from (
                                     select  case  when cfct.status in (0, 1) then 0 else cfct.status end as Situacao,
                                               count(cfct.id) as Quantidade, 
                                               t.ano as Ano, 
                                               {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")} as AnoTurma,
                                               t.modalidade_codigo  as Modalidade
                                          from consolidado_fechamento_componente_turma cfct 
                                         inner join turma t on t.id = cfct.turma_id
                                         inner join ue on ue.id = t.ue_id where t.tipo_turma in (1,7) ";

            var queryBuilder = new StringBuilder(sqlQuery);

            var queryWhere = new StringBuilder("");

            if (ano > 0)
            {
                queryWhere.AppendLine(" and t.ano_letivo = @ano ");
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
                queryWhere.AppendLine(" and cfct.bimestre = @bimestre ");
            }

            queryBuilder.AppendLine(queryWhere.ToString());
            queryBuilder.AppendLine($@" group by cfct.status, t.ano, {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")}, t.modalidade_codigo  
            UNION 
            select  case  when cfct.status in (0, 1) then 0 else cfct.status end as Situacao,
                                               count(cfct.id) as Quantidade, 
                                               '1' as ano,
                                               'Ed. Física' as AnoTurma,
                                               t.modalidade_codigo  as Modalidade
                                          from consolidado_fechamento_componente_turma cfct 
                                         inner join turma t on t.id = cfct.turma_id
                                         inner join ue on ue.id = t.ue_id where t.tipo_turma in (2) ");

            queryBuilder.AppendLine(queryWhere.ToString());
            queryBuilder.AppendLine($@" group by cfct.status, t.modalidade_codigo) x
                                   group by x.Situacao, x.Ano, x.AnoTurma, x.Modalidade
                                   order by x.Ano;");


            return queryBuilder.ToString();
        }

        public async Task<IEnumerable<FechamentoPendenciaQuantidadeDto>> ObterSituacaoPendenteFechamento(long ueId,
            int ano, long dreId, int modalidade, int semestre, int bimestre)
        {

            var query = ueId > 0 ? ObterSituacaoPendenteFechamentoQuery(ueId, ano, dreId, modalidade, semestre, bimestre) :
                ObterSituacaoPendenteFechamentoSMEQuery(ano, dreId, modalidade, semestre, bimestre);

            return await database.Conexao.QueryAsync<FechamentoPendenciaQuantidadeDto>(query, new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }

        private string ObterSituacaoPendenteFechamentoSMEQuery(int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var sqlQuery = new StringBuilder($@"select {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")} as Ano , t.modalidade_codigo as Modalidade,  count(pf.id) as Quantidade
                                from pendencia_fechamento pf
                                inner join fechamento_turma_disciplina ftd  on ftd.id = pf.fechamento_turma_disciplina_id 
                                inner join fechamento_turma ft  on ft.id = ftd.fechamento_turma_id 
                                inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                inner join turma t on ft.turma_id = t.id 
                                inner join pendencia p on p.id = pf.pendencia_id
                                inner join ue on ue.id = t.ue_id 
                                where t.tipo_turma in (1,7)
                                  and not p.excluido
                                  and p.situacao = 1 ");

            var queryWhere = new StringBuilder("");

            if (ano > 0)
                queryWhere.AppendLine(" and t.ano_letivo = @ano ");

            if (dreId > 0)
                queryWhere.AppendLine(" and ue.dre_id = @dreId ");

            if (modalidade > 0)
                queryWhere.AppendLine(" and t.modalidade_codigo = @modalidade ");

            if (semestre > 0)
                queryWhere.AppendLine(" and t.semestre = @semestre ");

            if (bimestre >= 0)
                queryWhere.AppendLine(" and pe.bimestre = @bimestre ");

            sqlQuery.AppendLine(queryWhere.ToString());

            sqlQuery.AppendLine($"group by {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")} , t.modalidade_codigo ");

            sqlQuery.AppendLine(@"UNION select 'Ed. Física' as Ano , t.modalidade_codigo as Modalidade,  count(pf.id) as Quantidade
                                from pendencia_fechamento pf
                                inner join fechamento_turma_disciplina ftd  on ftd.id = pf.fechamento_turma_disciplina_id 
                                inner join fechamento_turma ft  on ft.id = ftd.fechamento_turma_id 
                                inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                inner join turma t on ft.turma_id = t.id 
                                inner join pendencia p on p.id = pf.pendencia_id
                                inner join ue on ue.id = t.ue_id 
                                where t.tipo_turma in (2)
                                  and not p.excluido
                                  and p.situacao = 1 ");

            sqlQuery.AppendLine(queryWhere.ToString());
            sqlQuery.AppendLine($"group by t.modalidade_codigo ");
            return sqlQuery.ToString();
        }

        private string ObterSituacaoPendenteFechamentoQuery(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var sqlQuery = $@"select {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")} as Ano , t.modalidade_codigo as Modalidade,  count(pf.id) as Quantidade
                                from pendencia_fechamento pf
                                inner join fechamento_turma_disciplina ftd  on ftd.id = pf.fechamento_turma_disciplina_id 
                                inner join fechamento_turma ft  on ft.id = ftd.fechamento_turma_id 
                                inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                inner join turma t on ft.turma_id = t.id
                                inner join pendencia p on p.id = pf.pendencia_id
                                inner join ue on ue.id = t.ue_id 
                                where t.tipo_turma in (1,2,7)
                                  and not p.excluido
                                  and p.situacao = 1 ";

            var queryBuilder = new StringBuilder(sqlQuery);

            if (ano > 0)
                queryBuilder.Append(" and t.ano_letivo = @ano ");

            if (ueId > 0)
                queryBuilder.Append(" and t.ue_id = @ueId ");

            if (dreId > 0)
                queryBuilder.Append(" and ue.dre_id = @dreId ");

            if (modalidade > 0)
                queryBuilder.Append(" and t.modalidade_codigo = @modalidade ");

            if (semestre > 0)
                queryBuilder.Append(" and t.semestre = @semestre ");

            if (bimestre >= 0)
                queryBuilder.Append(" and pe.bimestre = @bimestre ");

            queryBuilder.Append($"group by {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")} , t.modalidade_codigo order by {(modalidade == (int)Modalidade.EJA ? "t.nome_filtro" : "t.nome")};");

            return queryBuilder.ToString();
        }

        public async Task<IEnumerable<FechamentoSituacaoQuantidadeDto>> ObterSituacaoProcessoFechamentoPorEstudante(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var sqlQuery = @"select
                                    distinct ftd.situacao as Situacao, count(ftd.id) as Quantidade, t.ano as Ano, t.modalidade_codigo  as Modalidade
                                    from fechamento_turma_disciplina ftd
                                    inner join fechamento_turma ft on ftd.fechamento_turma_id = ft.id
                                    inner join turma t on ft.turma_id = t.id
                                    inner join periodo_escolar pe on ft.periodo_escolar_id = pe.id
                                    inner join ue on ue.id = t.ue_id 
                                    where 1=1 ";

            var queryBuilder = new StringBuilder(sqlQuery);

            if (ano > 0)
            {
                queryBuilder.Append(" and t.ano_letivo = @ano ");
            }

            if (ueId > 0)
            {
                queryBuilder.Append(" and t.ue_id = @ueId ");
            }

            if (dreId > 0)
            {
                queryBuilder.Append(" and ue.dre_id = @dreId ");
            }

            if (modalidade > 0)
            {
                queryBuilder.Append(" and t.modalidade_codigo = @modalidade ");

            }

            if (semestre > 0)
            {
                queryBuilder.Append(" and t.semestre = @semestre ");

            }

            if (bimestre > 0)
            {
                queryBuilder.Append(" and pe.bimestre = @bimestre ");

            }

            queryBuilder.Append(@"group by ftd.situacao, t.ano , t.modalidade_codigo order by t.ano;");


            return await database.Conexao.QueryAsync<FechamentoSituacaoQuantidadeDto>(queryBuilder.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }

        public async Task<IEnumerable<TurmaFechamentoDisciplinaDto>> ObterTotalDisciplinasPorTurma(int anoLetivo, int bimestre)
        {
            var query = new StringBuilder(@"select cfct.turma_id as TurmaId, count(componente_curricular_id) as QuantidadeDisciplinas 
                from consolidado_fechamento_componente_turma cfct 
                inner join turma t on cfct.turma_id = t.id
                inner join componente_curricular cc on componente_curricular_id = cc.id
                where t.ano_letivo = @anoLetivo and cc.permite_lancamento_nota ");

            if (bimestre >= 0)
                query.AppendLine(" and cfct.bimestre = @bimestre ");

            query.AppendLine("group by cfct.turma_id order by 1");


            return await database.Conexao.QueryAsync<TurmaFechamentoDisciplinaDto>(query.ToString(), new { anoLetivo, bimestre });
        }

        public async Task<IEnumerable<long>> ObterFechamentosTurmaDisciplinaEmDuplicidade(DateTime dataInicio)
        {
            var sqlQuery = new StringBuilder();

            ConsultaRegistrosFechamentosTurmaDisciplinaDuplicados(sqlQuery);

            sqlQuery.AppendLine(", tmp_fechamento_disciplinas_duplicados_sequenciados as");
            sqlQuery.AppendLine("(select ftd.id fechamento_turma_disciplina_id,");
            sqlQuery.AppendLine("        row_number() over (partition by ft.turma_id, ft.periodo_escolar_id, ftd.disciplina_id order by ftd.id desc) sequencia");
            sqlQuery.AppendLine("    from fechamento_turma_disciplina ftd");
            sqlQuery.AppendLine("        inner join fechamento_turma ft");
            sqlQuery.AppendLine("            on ftd.fechamento_turma_id = ft.id");
            sqlQuery.AppendLine("        inner join tmp_fechamento_disciplinas_duplicados tmp_fdd");
            sqlQuery.AppendLine("            on ft.turma_id = tmp_fdd.turma_id and");
            sqlQuery.AppendLine("               ft.periodo_escolar_id = tmp_fdd.periodo_escolar_id and");
            sqlQuery.AppendLine("               ftd.disciplina_id = tmp_fdd.disciplina_id");
            sqlQuery.AppendLine("where ftd.criado_em::date >= @dataInicio::date)");
            sqlQuery.AppendLine("select fechamento_turma_disciplina_id");
            sqlQuery.AppendLine("  from tmp_fechamento_disciplinas_duplicados_sequenciados");
            sqlQuery.AppendLine("where sequencia > 1;");

            return await database.Conexao.QueryAsync<long>(sqlQuery.ToString(), new { dataInicio });
        }

        public async Task<IEnumerable<(long fechamentoTurmaDisciplinaId, long periodoEscolarId, string codigoRf)>> ObterFechamentosTurmaDisciplinaEmProcessamentoComTempoExpirado(DateTime dataInicio, int tempoConsideradoExpiracaoMinutos)
        {
            var sqlQuery = new StringBuilder();

            ConsultaRegistrosFechamentosTurmaDisciplinaDuplicados(sqlQuery);

            sqlQuery.AppendLine("select distinct ftd.id fechamentoTurmaDisciplinaId,");
            sqlQuery.AppendLine("                 ft.periodo_escolar_id as periodoEscolarId,");
            sqlQuery.AppendLine("                 coalesce(ftd.alterado_rf, ftd.criado_rf) codigo_rf");
            sqlQuery.AppendLine("    from fechamento_turma_disciplina ftd");
            sqlQuery.AppendLine("        inner join fechamento_turma ft");
            sqlQuery.AppendLine("            on ftd.fechamento_turma_id = ft.id");
            sqlQuery.AppendLine("where ftd.situacao = @situacaoFechamentoTurmaDisciplina and");
            sqlQuery.AppendLine("       ftd.criado_em::date >= @dataInicio::date and");
            sqlQuery.AppendLine($"       coalesce(ftd.alterado_em, ftd.criado_em)::timestamp < (current_timestamp - interval '{tempoConsideradoExpiracaoMinutos} minutes') and");
            sqlQuery.AppendLine("       not exists (select 1");
            sqlQuery.AppendLine("                         from tmp_fechamento_disciplinas_duplicados tmp_fdd");
            sqlQuery.AppendLine("                     where tmp_fdd.turma_id = ft.turma_id and");
            sqlQuery.AppendLine("                             tmp_fdd.periodo_escolar_id = ft.periodo_escolar_id and");
            sqlQuery.AppendLine("                             tmp_fdd.disciplina_id = ftd.disciplina_id)");
            sqlQuery.AppendLine("order by 1;");

            return await database.Conexao.QueryAsync<(long, long, string)>(sqlQuery.ToString(), new
            {
                situacaoFechamentoTurmaDisciplina = (int)SituacaoFechamento.EmProcessamento,
                dataInicio
            });
        }

        public async Task<bool> ExcluirLogicamenteFechamentosTurmaDisciplina(long[] idsFechamentoTurmaDisciplina)
        {
            var sqlQuery = @"update fechamento_turma_disciplina
                             set excluido = true
                             where id = any(@idsFechamentoTurmaDisciplina);";

            await database.Conexao
                .ExecuteAsync(sqlQuery, new { idsFechamentoTurmaDisciplina });

            return true;
        }

        private void ConsultaRegistrosFechamentosTurmaDisciplinaDuplicados(StringBuilder sqlQuery)
        {
            sqlQuery.AppendLine(";with tmp_fechamento_disciplinas_duplicados as");
            sqlQuery.AppendLine("(select ft.turma_id,");
            sqlQuery.AppendLine("        ft.periodo_escolar_id,");
            sqlQuery.AppendLine("       ftd.disciplina_id");
            sqlQuery.AppendLine("    from fechamento_turma_disciplina ftd");
            sqlQuery.AppendLine("        inner join fechamento_turma ft");
            sqlQuery.AppendLine("            on ftd.fechamento_turma_id = ft.id");
            sqlQuery.AppendLine("where ftd.criado_em::date >= @dataInicio::date");
            sqlQuery.AppendLine("group by ft.turma_id,");
            sqlQuery.AppendLine("          ft.periodo_escolar_id,");
            sqlQuery.AppendLine("          ftd.disciplina_id");
            sqlQuery.AppendLine("having count(0) > 1)");
        }

        public async Task<IEnumerable<TurmaFechamentoDisciplinaSituacaoDto>> ObterFechamentosTurmaPorTurmaId(long turmaId)
        {
            var sqlQuery = @"select ftd.id Id, ft.turma_id TurmaId, ft.periodo_escolar_id PeriodoEscolarId, ftd.disciplina_id DisciplinaId, ftd.situacao Situacao
                                from fechamento_turma ft 
                                join fechamento_turma_disciplina ftd on ft.id = ftd.fechamento_turma_id                                                                 
                                where ft.turma_id = @turmaId 
                                      and ft.periodo_escolar_id is not null ";

            return await database.Conexao.QueryAsync<TurmaFechamentoDisciplinaSituacaoDto>(sqlQuery, new { turmaId });
        }

        public async Task<FechamentoTurmaDisciplinaPendenciaDto> ObterFechamentoTurmaDisciplinaDTOPorTurmaDisciplinaBimestre(string turmaCodigo, long disciplinaId, int bimestre, SituacaoFechamento[] situacoesFechamento)
        {
            var query = new StringBuilder(@"select f.id,
                                            f.disciplina_id as DisciplinaId,
                                            f.situacao as SituacaoFechamento, 
                                            ft.turma_id as TurmaId,
                                            t.turma_id as CodigoTurma,
                                            t.nome as NomeTurma,
                                            p.periodo_inicio as PeriodoInicio,
                                            p.periodo_fim  as PeriodoFim,
                                            p.bimestre as bimestre,
                                            f.justificativa,
                                            f.criado_rf as CriadoRF,
                                            f.alterado_rf as AlteradoRF,
                                            u.id as UsuarioId,
                                            t.tipo_turma as TipoTurma
                                     from fechamento_turma_disciplina f
                                    inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                                    inner join periodo_escolar p on p.id = ft.periodo_escolar_id
                                    inner join turma t on t.id = ft.turma_id
                                    left join usuario u on u.rf_codigo = coalesce(f.alterado_rf, f.criado_rf)
                                    where t.turma_id = @turmaCodigo
                                    and f.disciplina_id = @disciplinaId
                                    and p.bimestre = @bimestre");
            if (situacoesFechamento.NaoEhNulo() && situacoesFechamento.Any())
                query.Append(" and f.situacao = any(@situacoesFechamento)");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurmaDisciplinaPendenciaDto>(query.ToString(),
                new { turmaCodigo, disciplinaId, bimestre, situacoesFechamento = (situacoesFechamento.NaoEhNulo() ? situacoesFechamento.Select(situacao => (int)situacao).ToArray() : null) });
        }

        public async Task<bool> VerificaExistenciaFechamentoTurmaDisciplinPorTurmaDisciplinaBimestreSituacao(long turmaId, long disciplinaId, long periodoId, SituacaoFechamento[] situacoesFechamento)
        {
            var query = new StringBuilder(@"select count(f.id)
                                     from fechamento_turma_disciplina f
                                    inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                                    where ft.turma_id = @turmaId
                                    and f.disciplina_id = @disciplinaId
                                    and ft.periodo_escolar_id = @periodoId
                                    and f.situacao = any(@situacoesFechamento)");
            return (await database.Conexao.QueryFirstAsync<int>(query.ToString(),
            new { turmaId, disciplinaId, periodoId, situacoesFechamento = situacoesFechamento.Select(situacao => (int)situacao).ToArray() })) > 0;
        }

        public Task<IEnumerable<FechamentoTurmaDisciplinaPendenciaDto>> ObterFechamentosTurmaDisciplinaDTOPorUeSituacao(long idUe, SituacaoFechamento[] situacoesFechamento, long[] fechamentoTurmaDisciplinaIdsIgnorados = null)
        {
            var query = new StringBuilder(@"select f.id,
                                            f.disciplina_id as DisciplinaId,
                                            f.situacao as SituacaoFechamento, 
                                            ft.turma_id as TurmaId,
                                            t.turma_id as CodigoTurma,
                                            t.nome as NomeTurma,
                                            p.periodo_inicio as PeriodoInicio,
                                            p.periodo_fim  as PeriodoFim,
                                            p.bimestre as bimestre,
                                            f.justificativa,
                                            f.criado_rf as CriadoRF,
                                            f.alterado_rf as AlteradoRF,
                                            u.id as UsuarioId,
                                            t.tipo_turma as TipoTurma
                                     from fechamento_turma_disciplina f
                                    inner join fechamento_turma ft on ft.id = f.fechamento_turma_id
                                    inner join periodo_escolar p on p.id = ft.periodo_escolar_id
                                    inner join turma t on t.id = ft.turma_id
                                    left join usuario u on u.rf_codigo = coalesce(f.alterado_rf, f.criado_rf)
                                    where f.situacao = any(@situacoesFechamento) and t.ue_id = @idUe and t.ano_letivo = @anoletivo ");
            if (fechamentoTurmaDisciplinaIdsIgnorados.NaoEhNulo() && fechamentoTurmaDisciplinaIdsIgnorados.Any())
                query.Append(" and f.id != any(@fechamentoTurmaDisciplinaIdsIgnorados) ");

            return database.Conexao.QueryAsync<FechamentoTurmaDisciplinaPendenciaDto>(query.ToString(),
                new
                {
                    situacoesFechamento = situacoesFechamento.Select(situacao => (int)situacao).ToArray(),
                    fechamentoTurmaDisciplinaIdsIgnorados,
                    idUe,
                    anoletivo = DateTimeExtension.HorarioBrasilia().Year
                });
        }

        public Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> ObterFechamentosTurmasCodigosEBimestreEAlunoCodigoAsync(string[] turmasCodigos, int bimestre, string alunoCodigo)
        {
            var query = @"select * from (
                            select 
                            fa.aluno_codigo as AlunoCodigo, 
	                        fn.disciplina_id as ComponenteCurricularId,
	                        fn.nota,
	                        fn.conceito_id as ConceitoId,
	                        p.bimestre,
                            row_number() over (partition by ft.id, fa.aluno_codigo, p.bimestre, fn.disciplina_id order by fn.id desc) sequencia
                          from fechamento_aluno fa 
                          left join fechamento_nota fn on fn.fechamento_aluno_id = fa.id 
                          left join fechamento_turma_disciplina f on f.id = fa.fechamento_turma_disciplina_id
                          left join fechamento_turma ft on ft.id = f.fechamento_turma_id
                          left join periodo_escolar p on p.id = ft.periodo_escolar_id
                          left join turma t on t.id = ft.turma_id
                          where not fn.excluido
	                            and t.turma_id = any(@turmasCodigos)
                                and fa.aluno_codigo = @alunoCodigo";

            if (bimestre > 0)
                query += " and p.bimestre = @bimestre";
            
            query += " ) t Where sequencia = 1";

            return database.Conexao.QueryAsync<FechamentoNotaAlunoAprovacaoDto>(query.ToString(), new { turmasCodigos, bimestre, alunoCodigo });
        }
    }
}