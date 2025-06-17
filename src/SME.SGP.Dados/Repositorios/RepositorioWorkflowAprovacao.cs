using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovacao : RepositorioBase<WorkflowAprovacao>, IRepositorioWorkflowAprovacao
    {
        public RepositorioWorkflowAprovacao(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<string> ObterCriador(long workflowId)
        {
            var query = "select criado_rf from wf_aprovacao wa where id = @workflowId";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { workflowId });
        }

        private string CamposEntidadeBase(string alias)
            => $"{alias}.id, {alias}.criado_em, {alias}.criado_por, {alias}.alterado_em, {alias}.alterado_por, {alias}.alterado_rf, {alias}.criado_rf";

        public async Task<WorkflowAprovacao> ObterEntidadeCompleta(long workflowId = 0, long notificacaoId = 0)
        {
            var query = new StringBuilder();
            query.AppendLine($"select {CamposEntidadeBase("wf")}, wf.ano, wf.excluido, wf.dre_id, wf.notificacao_mensagem, wf.notificacao_titulo, wf.notificacao_categoria, wf.notificacao_tipo, wf.tipo, wf.turma_id, wf.ue_id");
            query.AppendLine($"  , {CamposEntidadeBase("wfn")}, wfn.cargo, wfn.nivel, wfn.observacao, wfn.status, wfn.wf_aprovacao_id");
            query.AppendLine($"  , {CamposEntidadeBase("n")}, n.ano, n.categoria, n.codigo, n.dre_id, n.excluida, n.mensagem, n.status, n.tipo, n.titulo, n.turma_id, n.ue_id, n.usuario_id");
            query.AppendLine($"  , {CamposEntidadeBase("u")}, u.rf_codigo, u.expiracao_recuperacao_senha, u.login, u.nome, u.token_recuperacao_senha, u.ultimo_login");
            query.AppendLine("from wf_aprovacao wf");
            query.AppendLine("inner join wf_aprovacao_nivel wfn");
            query.AppendLine("on wfn.wf_aprovacao_id = wf.id");
            query.AppendLine("left join wf_aprovacao_nivel_notificacao wfnn");
            query.AppendLine("on wfnn.wf_aprovacao_nivel_id = wfn.id");
            query.AppendLine("left join notificacao n");
            query.AppendLine("on wfnn.notificacao_id = n.id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");

            query.AppendLine("where 1=1");

            if (workflowId > 0)
                query.AppendLine("and wf.id = @workflowId");

            if (notificacaoId > 0)
            {
                query.AppendLine("and wf.id=");
                query.AppendLine("(select wfn.wf_aprovacao_id");
                query.AppendLine("from wf_aprovacao_nivel_notificacao wfnn");
                query.AppendLine("inner join wf_aprovacao_nivel wfn");
                query.AppendLine("on wfnn.wf_aprovacao_nivel_id = wfn.id");
                query.AppendLine("where wfnn.notificacao_id=@notificacaoId)");
            }

            var lookup = new Dictionary<long, WorkflowAprovacao>();

            await database.Conexao.QueryAsync<WorkflowAprovacao, WorkflowAprovacaoNivel, Notificacao, Usuario, WorkflowAprovacao>(query.ToString(),
                 (workflow, workflowNivel, notificacao, usuario) =>
                {
                    WorkflowAprovacao workflowAprovacao;
                    if (!lookup.TryGetValue(workflow.Id, out workflowAprovacao))
                    {
                        workflowAprovacao = workflow;
                        lookup.Add(workflow.Id, workflowAprovacao);
                    }
                    workflowAprovacao.Adicionar(workflowNivel);

                    if (notificacao.NaoEhNulo())
                        workflowAprovacao.Adicionar(workflowNivel.Id, notificacao, usuario);                    

                    return workflowAprovacao;
                }, param: new { workflowId, notificacaoId });

            return lookup.Values.FirstOrDefault();
        }
        
        public async Task<WorkflowAprovacao> ObterEntidadeCompletaPorId(long workflowId)
        {
            var query = new StringBuilder();
            query.AppendLine("select wf.*, wfn.*, n.*, u.*");
            query.AppendLine("from wf_aprovacao wf");
            query.AppendLine("inner join wf_aprovacao_nivel wfn");
            query.AppendLine("on wfn.wf_aprovacao_id = wf.id");
            query.AppendLine("left join wf_aprovacao_nivel_notificacao wfnn");
            query.AppendLine("on wfnn.wf_aprovacao_nivel_id = wfn.id");
            query.AppendLine("left join notificacao n");
            query.AppendLine("on wfnn.notificacao_id = n.id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on n.usuario_id = u.id");

            query.AppendLine("where 1=1");

            if (workflowId > 0)
                query.AppendLine("and wf.id = @workflowId");

            var lookup = new Dictionary<long, WorkflowAprovacao>();

            await database.Conexao.QueryAsync<WorkflowAprovacao, WorkflowAprovacaoNivel, Notificacao, Usuario, WorkflowAprovacao>(query.ToString(),
                 (workflow, workflowNivel, notificacao, usuario) =>
                 {
                     WorkflowAprovacao workflowAprovacao;
                     if (!lookup.TryGetValue(workflow.Id, out workflowAprovacao))
                     {
                         workflowAprovacao = workflow;
                         lookup.Add(workflow.Id, workflowAprovacao);
                     }
                     workflowAprovacao.Adicionar(workflowNivel);

                     if (notificacao.NaoEhNulo())
                         workflowAprovacao.Adicionar(workflowNivel.Id, notificacao, usuario);

                     return workflowAprovacao;
                 }, param: new { workflowId });

            return lookup.Values.FirstOrDefault();
        }

        public async Task<IEnumerable<long>> ObterIdsWorkflowPorWfAprovacaoId(long id, string tabelaVinculada)
        {
            var sql = @$"select wa2.id from wf_aprovacao wa 
                        inner join {tabelaVinculada} wa2 on wa2.wf_aprovacao_id = wa.id
                        where wa.id = @id ";
            return await database.Conexao.QueryAsync<long>(sql, new { id });            
        }

        public IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo)
        {
            return database.Conexao.Query<WorkflowAprovacao>("select * from WorkflowAprovaNivel w where w.codigo = @codigo ", new { codigo });
        }
    }
}