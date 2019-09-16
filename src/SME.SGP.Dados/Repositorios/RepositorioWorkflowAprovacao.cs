using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovacao : RepositorioBase<WorkflowAprovacao>, IRepositorioWorkflowAprovacao
    {
        public RepositorioWorkflowAprovacao(ISgpContext conexao) : base(conexao)
        {
        }

        public WorkflowAprovacao ObterEntidadeCompleta(long workflowId = 0, long notificacaoId = 0)
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
            query.AppendLine("left join wf_aprovacao_nivel_usuario wfnu");
            query.AppendLine("on wfnu.wf_aprovacao_nivel_id = wfn.id");
            query.AppendLine("left join usuario u");
            query.AppendLine("on wfnu.usuario_id = u.id");
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

            database.Conexao.Query<WorkflowAprovacao, WorkflowAprovacaoNivel, Notificacao, Usuario, WorkflowAprovacao>(query.ToString(),
                 (wf, wfn, n, u) =>
                {
                    WorkflowAprovacao workflowAprovacao;
                    if (!lookup.TryGetValue(wf.Id, out workflowAprovacao))
                    {
                        workflowAprovacao = wf;
                        lookup.Add(wf.Id, workflowAprovacao);
                    }
                    workflowAprovacao.Adicionar(wfn);
                    workflowAprovacao.Adicionar(wfn.Id, n);
                    workflowAprovacao.Adicionar(wfn.Id, u);

                    return workflowAprovacao;
                }, param: new { workflowId, notificacaoId }, splitOn: "Id").FirstOrDefault();

            return lookup.Values.FirstOrDefault();
        }

        public IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo)
        {
            return database.Conexao.Query<WorkflowAprovacao>("select * from WorkflowAprovaNivel w where w.codigo = @codigo ", new { codigo })
                .AsList();
        }
    }
}