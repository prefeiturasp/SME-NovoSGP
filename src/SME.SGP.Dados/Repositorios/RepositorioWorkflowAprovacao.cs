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

                    query.AppendLine(@"
                        SELECT
                            -- WorkflowAprovacao
                            wf.id AS Id,
                            wf.criado_em AS CriadoEm,
                            wf.criado_por AS CriadoPor,
                            wf.alterado_em AS AlteradoEm,
                            wf.alterado_por AS AlteradoPor,
                            wf.alterado_rf AS AlteradoRF,
                            wf.criado_rf AS CriadoRF,
                            wf.ano AS Ano,
                            wf.excluido AS Excluido,
                            wf.dre_id AS DreId,
                            wf.notificacao_mensagem AS NotifacaoMensagem,
                            wf.notificacao_titulo AS NotifacaoTitulo,
                            wf.notificacao_categoria AS NotificacaoCategoria,
                            wf.notificacao_tipo AS NotificacaoTipo,
                            wf.tipo AS Tipo,
                            wf.turma_id AS TurmaId,
                            wf.ue_id AS UeId,

                            -- marcador do WorkflowAprovacaoNivel
                            wfn.id AS NivelInicio,

                            -- WorkflowAprovacaoNivel
                            wfn.id AS Id,
                            wfn.criado_em AS CriadoEm,
                            wfn.criado_por AS CriadoPor,
                            wfn.alterado_em AS AlteradoEm,
                            wfn.alterado_por AS AlteradoPor,
                            wfn.alterado_rf AS AlteradoRF,
                            wfn.criado_rf AS CriadoRF,
                            wfn.cargo AS Cargo,
                            wfn.nivel AS Nivel,
                            wfn.observacao AS Observacao,
                            wfn.status AS Status,
                            wfn.wf_aprovacao_id AS WorkflowId,

                            -- marcador da Notificacao
                            n.id AS NotificacaoInicio,

                            -- Notificacao
                            n.id AS Id,
                            n.criado_em AS CriadoEm,
                            n.criado_por AS CriadoPor,
                            n.alterado_em AS AlteradoEm,
                            n.alterado_por AS AlteradoPor,
                            n.alterado_rf AS AlteradoRF,
                            n.criado_rf AS CriadoRF,
                            n.ano AS Ano,
                            n.categoria AS Categoria,
                            n.codigo AS Codigo,
                            n.dre_id AS DreId,
                            n.excluida AS Excluida,
                            n.mensagem AS Mensagem,
                            n.status AS Status,
                            n.tipo AS Tipo,
                            n.titulo AS Titulo,
                            n.turma_id AS TurmaId,
                            n.ue_id AS UeId,
                            n.usuario_id AS UsuarioId,

                            -- marcador do Usuario
                            u.id AS UsuarioInicio,

                            -- Usuario
                            u.id AS Id,
                            u.criado_em AS CriadoEm,
                            u.criado_por AS CriadoPor,
                            u.alterado_em AS AlteradoEm,
                            u.alterado_por AS AlteradoPor,
                            u.alterado_rf AS AlteradoRF,
                            u.criado_rf AS CriadoRF,
                            u.rf_codigo AS CodigoRf,
                            u.expiracao_recuperacao_senha
                                AS ExpiracaoRecuperacaoSenha,
                            u.login AS Login,
                            u.nome AS Nome,
                            u.token_recuperacao_senha
                                AS TokenRecuperacaoSenha,
                            u.ultimo_login AS UltimoLogin

                        FROM wf_aprovacao wf
                        INNER JOIN wf_aprovacao_nivel wfn
                            ON wfn.wf_aprovacao_id = wf.id
                        LEFT JOIN wf_aprovacao_nivel_notificacao wfnn
                            ON wfnn.wf_aprovacao_nivel_id = wfn.id
                        LEFT JOIN notificacao n
                            ON wfnn.notificacao_id = n.id
                        LEFT JOIN usuario u
                            ON n.usuario_id = u.id
                        WHERE 1 = 1");

                    if (workflowId > 0)
                    {
                        query.AppendLine(
                            "AND wf.id = @workflowId");
                    }

                    if (notificacaoId > 0)
                    {
                        query.AppendLine(@"
                            AND wf.id = (
                                SELECT wfn2.wf_aprovacao_id
                                FROM wf_aprovacao_nivel_notificacao wfnn2
                                INNER JOIN wf_aprovacao_nivel wfn2
                                    ON wfnn2.wf_aprovacao_nivel_id =
                                       wfn2.id
                                WHERE wfnn2.notificacao_id =
                                      @notificacaoId
                            )");
                    }

                    var lookup =
                        new Dictionary<long, WorkflowAprovacao>();

                    await database.Conexao.QueryAsync<
                        WorkflowAprovacao,
                        WorkflowAprovacaoNivel,
                        Notificacao,
                        Usuario,
                        WorkflowAprovacao>(
                        query.ToString(),
                        (workflow, workflowNivel, notificacao, usuario) =>
                        {
                            if (!lookup.TryGetValue(
                                    workflow.Id,
                                    out var workflowAprovacao))
                            {
                                workflowAprovacao = workflow;
                                lookup.Add(
                                    workflow.Id,
                                    workflowAprovacao);
                            }

                            // O relacionamento precisa ser mantido
                            // explicitamente no objeto filho.
                            workflowNivel.WorkflowId =
                                workflowAprovacao.Id;

                            workflowNivel.Workflow =
                                workflowAprovacao;

                            workflowAprovacao.Adicionar(
                                workflowNivel);

                            if (notificacao != null &&
                                notificacao.Id > 0)
                            {
                                workflowAprovacao.Adicionar(
                                    workflowNivel.Id,
                                    notificacao,
                                    usuario);
                            }

                            return workflowAprovacao;
                        },
                        param: new
                        {
                            workflowId,
                            notificacaoId
                        },
                        splitOn:
                            "NivelInicio," +
                            "NotificacaoInicio," +
                            "UsuarioInicio");

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