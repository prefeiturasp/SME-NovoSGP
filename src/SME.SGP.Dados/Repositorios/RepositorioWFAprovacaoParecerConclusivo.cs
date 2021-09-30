using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioWFAprovacaoParecerConclusivo : IRepositorioWFAprovacaoParecerConclusivo
    {
        private readonly ISgpContext database;

        public RepositorioWFAprovacaoParecerConclusivo(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Excluir(long id)
        {
            var query = @"delete from wf_aprovacao_parecer_conclusivo where id = @id";

            await database.Conexao.ExecuteScalarAsync(query, new { id });
        }

        public async Task<WFAprovacaoParecerConclusivo> ObterPorConselhoClasseAlunoId(long conselhoClasseAlunoId)
        {
            var query = @"select * from wf_aprovacao_parecer_conclusivo where conselho_classe_aluno_id = @conselhoClasseAlunoId";

            return await database.Conexao.QueryFirstOrDefaultAsync<WFAprovacaoParecerConclusivo>(query, new { conselhoClasseAlunoId });
        }

        public async Task<WFAprovacaoParecerConclusivo> ObterPorWorkflowId(long workflowId)
        {
            var query = @"select wa.*, ca.*, cpa.*, cpp.*
                          from wf_aprovacao_parecer_conclusivo wa
                         inner join conselho_classe_aluno ca on ca.id = wa.conselho_classe_aluno_id
                          left join conselho_classe_parecer cpa on cpa.id = ca.conselho_classe_parecer_id
                          left join conselho_classe_parecer cpp on cpp.id = wa.conselho_classe_parecer_id
                        where wa.wf_aprovacao_id = @workflowId";

            return (await database.Conexao.QueryAsync<WFAprovacaoParecerConclusivo, ConselhoClasseAluno, ConselhoClasseParecerConclusivo, ConselhoClasseParecerConclusivo, WFAprovacaoParecerConclusivo>(query
                , (wfAprovacao, conselhoClasseAluno, parecerAnterior, parecerNovo) =>
                {
                    conselhoClasseAluno.ConselhoClasseParecer = parecerAnterior;

                    wfAprovacao.ConselhoClasseParecer = parecerNovo;
                    wfAprovacao.ConselhoClasseAluno = conselhoClasseAluno;

                    return wfAprovacao;
                })).FirstOrDefault();
        }

        public async Task Salvar(WFAprovacaoParecerConclusivo entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }
    }
}
