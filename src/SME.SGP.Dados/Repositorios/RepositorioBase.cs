using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
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
            return database.Conexao.GetAll<T>();
        }

        public virtual T ObterPorId(long id)
        {
            return database.Conexao.Get<T>(id);
        }

        public virtual void Remover(long id)
        {
            var entidade = database.Conexao.Get<T>(id);
            database.Conexao.Delete(entidade);
            Auditar(entidade.Id, "E");
        }

        public virtual long Salvar(T entidade)
        {
            if (entidade.Id > 0)
            {
                entidade.AlteradoEm = DateTime.Now;
                entidade.AlteradoPor = database.UsuarioLogadoNomeCompleto;
                entidade.AlteradoRF = database.UsuarioLogadoRF;
                database.Conexao.Update(entidade);
                Auditar(entidade.Id, "A");
            }
            else
            {
                entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;
                entidade.CriadoRF = database.UsuarioLogadoRF;
                entidade.Id = (long)database.Conexao.Insert(entidade);
                Auditar(entidade.Id, "I");
            }

            return entidade.Id;
        }

        private void Auditar(long identificador, string acao)
        {
            database.Conexao.Insert<Auditoria>(new Auditoria()
            {
                Data = DateTime.Now,
                Entidade = typeof(T).Name.ToLower(),
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Acao = acao
            });
        }
    }
}