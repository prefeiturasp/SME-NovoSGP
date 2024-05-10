using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaFechamentoAula : IRepositorioPendenciaFechamentoAula
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaFechamentoAula(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task ExcluirAsync(long id)
        {
            var entidade = await database.Conexao.GetAsync<PendenciaFechamentoAula>(id);
            await database.Conexao.DeleteAsync<PendenciaFechamentoAula>(entidade);
        }

        public async Task SalvarAsync(PendenciaFechamentoAula entidade)
        {
            await database.Conexao.InsertAsync(entidade);
        }
    }
}
