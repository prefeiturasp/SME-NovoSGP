using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWfAprovacaoNotaFechamento: IRepositorioWfAprovacaoNotaFechamento
    {
        protected readonly ISgpContext database;

        public RepositorioWfAprovacaoNotaFechamento(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Excluir(WfAprovacaoNotaFechamento wfAprovacaoNota)
        {
            await database.Conexao.DeleteAsync(wfAprovacaoNota);
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamento>> ObterPorNotaId(long fechamentoNotaId)
        {
            var query = @"select * from wf_aprovacao_nota_fechamento where fechamento_nota_id = @fechamentoNotaId";

            return await database.Conexao.QueryAsync<WfAprovacaoNotaFechamento>(query, new { fechamentoNotaId });
        }

        public async Task SalvarAsync(WfAprovacaoNotaFechamento entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                await database.Conexao.InsertAsync(entidade);
        }
    }
}
