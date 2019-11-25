using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeAvaliativa : IRepositorioAtividadeAvaliativa
    {
        protected readonly ISgpContext database;

        public RepositorioAtividadeAvaliativa(ISgpContext database)
        {
            this.database = database;
        }

        public IEnumerable<AtividadeAvaliativa> Listar()
        {
            throw new System.NotImplementedException();
        }

        public AtividadeAvaliativa ObterPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Remover(AtividadeAvaliativa entidade)
        {
            throw new System.NotImplementedException();
        }

        public long Salvar(AtividadeAvaliativa entidade)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> SalvarAsync(AtividadeAvaliativa entidade)
        {
            throw new System.NotImplementedException();
        }
    }
}