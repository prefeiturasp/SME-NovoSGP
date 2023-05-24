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
                          join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                          where cc.fechamento_turma_id = @fechamentoTurmaId";

            return await database.Conexao.QueryAsync<ConselhoClasseAlunosNotaPorFechamentoIdDto>(query, new { fechamentoTurmaId });
        }
    }
}
