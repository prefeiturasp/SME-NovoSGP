using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public abstract class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly ISgpContext database;

        protected RepositorioBase(ISgpContext database)
        {
            this.database = database;
        }

        public virtual IEnumerable<T> Listar()
        {
            return database.Conexao().GetAll<T>();
        }

        public virtual void Remover(long id)
        {
            var entidade = database.Conexao().Get<T>(id);
            database.Conexao().Delete(entidade);
        }

        public virtual long Salvar(T entidade)
        {
            entidade.Id = (long)database.Conexao().Insert(entidade);
            return entidade.Id;
        }

        public virtual T ObterPorId(long id)
        {
            return database.Conexao().Get<T>(id);
        }
    }
}