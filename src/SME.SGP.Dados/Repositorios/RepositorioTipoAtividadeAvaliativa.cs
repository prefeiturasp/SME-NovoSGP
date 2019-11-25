using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoAtividadeAvaliativa : IRepositorioTipoAtividadeAvaliativa
    {
        protected readonly ISgpContext database;

        public RepositorioTipoAtividadeAvaliativa(ISgpContext database)
        {
            this.database = database;
        }

        public IEnumerable<TipoAtividadeAvaliativa> Listar()
        {
            throw new System.NotImplementedException();
        }

        public TipoAtividadeAvaliativa ObterPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(TipoAtividadeAvaliativa entidade)
        {
            throw new System.NotImplementedException();
        }

        public long Salvar(TipoAtividadeAvaliativa entidade)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> SalvarAsync(TipoAtividadeAvaliativa entidade)
        {
            throw new System.NotImplementedException();
        }
    }
}