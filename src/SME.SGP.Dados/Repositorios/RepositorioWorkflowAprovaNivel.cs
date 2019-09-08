using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovaNivel : RepositorioBase<WorkflowAprovaNivel>, IRepositorioWorkflowAprovaNivel
    {
        public RepositorioWorkflowAprovaNivel(ISgpContext conexao) : base(conexao)
        {
        }

        public IEnumerable<WorkflowAprovaNivel> ObterNiveisPorCodigo(string codigo)
        {
            return database.Conexao.Query<WorkflowAprovaNivel>("select * from WorkflowAprovaNivel w where w.codigo = @codigo ", new { codigo })
                .AsList();
        }
    }
}