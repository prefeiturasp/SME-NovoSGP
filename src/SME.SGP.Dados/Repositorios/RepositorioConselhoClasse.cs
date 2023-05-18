using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
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
                          join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id
                          where cc.fechamento_turma_id = @fechamentoTurmaId";

            return await database.Conexao.QueryAsync<ConselhoClasseAlunosNotaPorFechamentoIdDto>(query, new { fechamentoTurmaId });
        }

        public async Task<IEnumerable<AlunoTemRecomandacaoDto>> VerificarSeExisteRecomendacaoPorTurma(long turmaId,int bimestre)
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
                        where not cca.excluido
	                        and ft.turma_id = @turmaId
	                        and pe.bimestre = @bimestre ";
            return await database.Conexao.QueryAsync<AlunoTemRecomandacaoDto>(query, new { turmaId,bimestre });
        }
    }
}
