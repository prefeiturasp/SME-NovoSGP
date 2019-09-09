using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovacao : RepositorioBase<WorkflowAprovacao>, IRepositorioWorkflowAprovacao
    {
        public RepositorioWorkflowAprovacao(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<WorkflowAprovacao> ObterNiveisPorCodigo(string codigo)
        {
            return database.Conexao.Query<WorkflowAprovacao>("select * from WorkflowAprovaNivel w where w.codigo = @codigo ", new { codigo })
                .AsList();
        }
    }
}