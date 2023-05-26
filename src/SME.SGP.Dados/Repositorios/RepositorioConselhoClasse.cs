using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasse : RepositorioBase<ConselhoClasse>, IRepositorioConselhoClasse
    {
        public RepositorioConselhoClasse(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<bool> AtualizarSituacao(long conselhoClasseId, SituacaoConselhoClasse situacaoConselhoClasse)
        {
            database.Conexao.Execute("update conselho_classe set situacao = @situacaoConselhoClasse where id = @conselhoClasseId", new { conselhoClasseId, situacaoConselhoClasse = (int)situacaoConselhoClasse });

            return Task.FromResult(true);
        }

        public async Task<IEnumerable<ConselhoClasseAlunosNotaPorFechamentoIdDto>> ObterConselhoClasseAlunosNotaPorFechamentoId(long fechamentoTurmaId)
        {
            var query = @"select cc.id ConselhoClasseId, ccn.id ConselhoClasseNotaId, 
                                cca.aluno_codigo AlunoCodigo, ccn.componente_curricular_codigo ComponenteCurricularCodigo
                          from conselho_classe cc
                          join conselho_classe_aluno cca on cc.id = cca.conselho_classe_id
                          join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                          where cc.fechamento_turma_id = @fechamentoTurmaId";

            return await database.Conexao.QueryAsync<ConselhoClasseAlunosNotaPorFechamentoIdDto>(query, new { fechamentoTurmaId });
        }

        public async Task<IEnumerable<AlunoTemRecomandacaoDto>> VerificarSeExisteRecomendacaoPorTurma(string[] turmasId,int bimestre)
        {
            var query = @"select
	                        distinct cca.aluno_codigo AluncoCodigo,
	                        ccr.id is not null TemRecomendacao
                        from conselho_classe_aluno cca
                        join conselho_classe cc on cc.id = cca.conselho_classe_id
                        join fechamento_turma ft on ft.id = cc.fechamento_turma_id
                        join conselho_classe_aluno_recomendacao ccar on ccar.conselho_classe_aluno_id = cca.id
                        join conselho_classe_recomendacao ccr on ccr.id = ccar.conselho_classe_recomendacao_id
                        join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                        join turma t on t.id = ft.turma_id
                        where not cca.excluido
	                        and t.turma_id = any(@turmasId)
	                        and pe.bimestre = @bimestre ";
            return await database.Conexao.QueryAsync<AlunoTemRecomandacaoDto>(query, new { turmasId,bimestre });
        }

        public async Task<IEnumerable<ConselhoClasseAlunoNotaDto>> ObterConselhoClasseAlunoNota(string[] turmasCodigos, int bimestre)
        {
            var sql = new StringBuilder(); 
            sql.AppendLine(@"select distinct ");
            sql.AppendLine(@"	cccat.aluno_codigo as AlunoCodigo,");
            sql.AppendLine(@"	coalesce(cccatn.nota,cccatn.conceito_id)  as Nota,");
            sql.AppendLine(@"	cccatn.componente_curricular_id as ComponenteCurricularId,");
            sql.AppendLine(@"	cc.descricao_sgp as Descricao");
            sql.AppendLine(@"from");
            sql.AppendLine(@"	consolidado_conselho_classe_aluno_turma_nota cccatn");
            sql.AppendLine(@"join consolidado_conselho_classe_aluno_turma cccat on");
            sql.AppendLine(@"	cccat.id = cccatn.consolidado_conselho_classe_aluno_turma_id");
            sql.AppendLine(@"join turma t on");
            sql.AppendLine(@"	t.id = cccat.turma_id");
            sql.AppendLine(@"join componente_curricular cc on");
            sql.AppendLine(@"	cc.id = cccatn.componente_curricular_id");
            sql.AppendLine(@"where");
            sql.AppendLine(@"	not cccat.excluido");
            sql.AppendLine(@"	and not cccat.excluido");
            sql.AppendLine(@"	and t.turma_id = any(@turmasCodigos) ");
            sql.AppendLine(@"	and cccatn.bimestre = @bimestre ");
            return await database.Conexao.QueryAsync<ConselhoClasseAlunoNotaDto>(sql.ToString(), new { turmasCodigos,bimestre });
        }
    }
}
