using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamentoAtividadeAvaliativa : IRepositorioPendenciaFechamentoAtividadeAvaliativa
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaFechamentoAtividadeAvaliativa(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task ExcluirAsync(long id)
        {
            var entidade = await database.Conexao.GetAsync<PendenciaFechamentoAtividadeAvaliativa>(id);
            await database.Conexao.DeleteAsync<PendenciaFechamentoAtividadeAvaliativa>(entidade);
        }

        public async Task SalvarAsync(PendenciaFechamentoAtividadeAvaliativa entidade)
        {
            await database.Conexao.InsertAsync(entidade);
        }
    }
}
