using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasse : RepositorioBase<ConselhoClasse>, IRepositorioConselhoClasse
    {
        public RepositorioConselhoClasse(ISgpContext database) : base(database)
        {
        }

        public Task<bool> AtualizarSituacao(long conselhoClasseId, SituacaoConselhoClasse situacaoConselhoClasse)
        {
            database.Conexao.Execute("update conselho_classe set situacao = @situacaoConselhoClasse where id = @conselhoClasseId", new { conselhoClasseId, situacaoConselhoClasse = (int)situacaoConselhoClasse });

            return Task.FromResult(true);
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

            var sqlQuery = new StringBuilder($@"select Situacao, sum(x.Quantidade) as Quantidade,
                            x.AnoTurma
                       from (
                             select  cccat.status as Situacao,
                                       count(cccat.id) as Quantidade, ");
            if (ueId > 0)
                sqlQuery.AppendLine(" t.nome as AnoTurma ");
            else
                sqlQuery.AppendLine(" t.ano as AnoTurma ");
            sqlQuery.AppendLine(@" 
                                  from consolidado_conselho_classe_aluno_turma cccat 
                                 inner join turma t on t.id = cccat.turma_id 
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
                queryWhere.AppendLine(" and cccat.bimestre = @bimestre ");
            }

            sqlQuery.AppendLine(queryWhere.ToString());
            sqlQuery.AppendLine($@" group by cccat.status, ");
            if (ueId > 0)
                sqlQuery.AppendLine(" t.nome  ");
            else
                sqlQuery.AppendLine(" t.ano ");

            sqlQuery.AppendLine(@") x group by x.Situacao, x.AnoTurma
                                   order by x.AnoTurma;");


            return sqlQuery.ToString();
        }

        public async Task<IEnumerable<FechamentoConselhoClasseNotaFinalDto>> ObterNotasFechamentoOuConselhoAlunos(long ueId, int anoLetivo, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder(@"CREATE TEMPORARY TABLE temp_dados
                                            (
                                                turmaanonome varchar(200),
                                                bimestre int8,
                                                componentecurricularcodigo int8,
                                                conselhoclassenotaid varchar(10),
                                                conceitoid int8,
                                                nota int8,
                                                alunocodigo varchar(10),
                                                conceito varchar(10),
                                                prioridade int8
                                            ); ");

            query.AppendLine(MontarQueryNotasFinasFechamentoQuantidade(ueId, anoLetivo, dreId, modalidade, semestre, bimestre));
            query.AppendLine(MontarQueryNotasFinasConselhoClasseQuantidade(ueId, anoLetivo, dreId, modalidade, semestre, bimestre));

            query.AppendLine(@" select
                                x.TurmaAnoNome,
                                x.Bimestre,
                                x.ComponenteCurricularCodigo,
                                x.ConselhoClasseNotaId,
                                x.ConceitoId,
                                x.Nota,
                                x.AlunoCodigo,
                                x.Conceito,
                                row_number() over(partition by x.TurmaAnoNome, x.ComponenteCurricularCodigo, x.AlunoCodigo
                                order by x.Prioridade) as linha
                                from temp_dados x; ");

            query.AppendLine(@" drop table temp_dados; ");

            var parametros = new
            {
                ueId,
                anoLetivo,
                dreId,
                modalidade,
                semestre,
                bimestre
            };
            return await database.Conexao.QueryAsync<FechamentoConselhoClasseNotaFinalDto>(query.ToString(), parametros);
        }

        private string MontarQueryNotasFinasFechamentoQuantidade(long ueId, int anoLetivo, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder(@"    insert into temp_dados
                                                select t.nome as TurmaAnoNome, 
				                                     pe.bimestre, 
                                                    fn.disciplina_id as ComponenteCurricularCodigo, 
                                                    null as ConselhoClasseNotaId, 
                                                    fn.conceito_id as ConceitoId, 
                                                    fn.nota as Nota, 
                                                    fa.aluno_codigo as AlunoCodigo,
                                                    cv.valor as Conceito,
                                                    2 as prioridade
                                                from
                                                    fechamento_turma ft
                                                left join periodo_escolar pe on
                                                    pe.id = ft.periodo_escolar_id
                                                inner join turma t on
                                                    t.id = ft.turma_id
                                                inner join ue on
                                                    ue.id = t.ue_id
                                                inner join fechamento_turma_disciplina ftd on
                                                    ftd.fechamento_turma_id = ft.id
                                                    and not ftd.excluido
                                                inner join fechamento_aluno fa on
                                                    fa.fechamento_turma_disciplina_id = ftd.id
                                                    and not fa.excluido
                                                inner join fechamento_nota fn on
                                                    fn.fechamento_aluno_id = fa.id
                                                    and not fn.excluido
				                                inner join conceito_valores cv on fn.conceito_id = cv.id
                                                where t.ano_letivo = @anoLetivo ");

            if (ueId > 0)
                query.Append(" and ue.id = @ueId ");

            if (dreId > 0)
                query.Append(" and ue.dre_id = @dreId ");

            if (modalidade > 0)
                query.Append(" and t.modalidade_codigo = @modalidade ");

            if (bimestre > 0)
                query.Append(" and pe.bimestre = @bimestre ");

            if (semestre > 0)
                query.Append(" and t.semestre = @semestre ");

            query.Append(";");
            return query.ToString();
        }

        private string MontarQueryNotasFinasConselhoClasseQuantidade(long ueId, int anoLetivo, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder(@"    insert into temp_dados
                                                select t.nome as TurmaAnoNome, 
                                                    pe.bimestre, 
		                                            ccn.componente_curricular_codigo as ComponenteCurricularCodigo, 
		                                            ccn.id as ConselhoClasseNotaId, 
		                                            ccn.conceito_id as ConceitoId, 
		                                            ccn.nota as Nota, 
		                                            cca.aluno_codigo as AlunoCodigo,
		                                            cv.valor as Conceito,
		                                            1 as prioridade
	                                            from
		                                            fechamento_turma ft
	                                            left join periodo_escolar pe on
		                                            pe.id = ft.periodo_escolar_id
	                                            inner join turma t on
		                                            t.id = ft.turma_id
	                                            inner join ue on
		                                            ue.id = t.ue_id
	                                            inner join conselho_classe cc on
		                                            cc.fechamento_turma_id = ft.id
	                                            inner join conselho_classe_aluno cca on
		                                            cca.conselho_classe_id = cc.id
	                                            inner join conselho_classe_nota ccn on
		                                            ccn.conselho_classe_aluno_id = cca.id
	                                            inner join conceito_valores cv on ccn.conceito_id = cv.id
	                                            where t.ano_letivo = @anoLetivo ");

            if (ueId > 0)
                query.Append(" and ue.id = @ueId ");

            if (dreId > 0)
                query.Append(" and ue.dre_id = @dreId ");

            if (modalidade > 0)
                query.Append(" and t.modalidade_codigo = @modalidade ");

            if (bimestre > 0)
                query.Append(" and pe.bimestre = @bimestre ");

            if (semestre > 0)
                query.Append(" and t.semestre = @semestre ");

            query.Append(";");
            return query.ToString();
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
                          and pe.bimestre in (1,2,3)
                          and tc.id in (24,25,26,27)");
            
            return await database.Conexao
                .QueryAsync<objConsolidacaoConselhoAluno>(query.ToString(), new { dreId });
        }
    }
}
