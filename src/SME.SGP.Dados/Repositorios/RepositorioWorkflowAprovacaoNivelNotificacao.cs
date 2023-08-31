using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovaNivelNotificacao : IRepositorioWorkflowAprovacaoNivelNotificacao
    {
        private readonly ISgpContext dataBase;

        public RepositorioWorkflowAprovaNivelNotificacao(ISgpContext dataBase)
        {
            this.dataBase = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
        }

        public async Task ExcluirPorWorkflowNivelNotificacaoId(long workflowNivelId, long notificacaoId)
        {
            var command = @"delete from wf_aprovacao_nivel_notificacao where wf_aprovacao_nivel_id = @workflowNivelId and
                            notificacao_id = @notificacaoId ";

            await dataBase.Conexao.ExecuteAsync(command, new { workflowNivelId, notificacaoId });
        }

        public void Salvar(WorkflowAprovacaoNivelNotificacao workflowAprovaNivelNotificacao)
        {
            dataBase.Conexao.Insert(workflowAprovaNivelNotificacao);
        }

        public async Task SalvarAsync(WorkflowAprovacaoNivelNotificacao workflowAprovaNivelNotificacao)
        {
            await dataBase.Conexao.InsertAsync(workflowAprovaNivelNotificacao);
        }
    }
}