using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Dados.Repositorios
{
    public abstract class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly SgpContext database;

        protected RepositorioBase(SgpContext database)
        {
            this.database = database;
        }

        public virtual IEnumerable<T> Listar()
        {
            return database.Connection().GetAll<T>();
        }

        public virtual void Remover(long id)
        {
            var entidade = database.Connection().Get<T>(id);
            database.Connection().Delete(entidade);
        }

        public virtual long Salvar(T entidade)
        {
            entidade.Id = (long)database.Connection().Insert(entidade);
            return entidade.Id;
        }
    }
}