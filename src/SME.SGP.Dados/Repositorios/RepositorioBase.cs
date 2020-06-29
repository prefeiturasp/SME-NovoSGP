using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public virtual async Task<T> ObterPorIdAsync(long id)
        {
            return await database.Conexao.GetAsync<T>(id);
        }

        public virtual void Remover(long id)
        {
            var entidade = database.Conexao.Get<T>(id);
            database.Conexao.Delete(entidade);
            Auditar(entidade.Id, "E");
        }

        public virtual void Remover(T entidade)
        {
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

        public virtual async Task<long> SalvarAsync(T entidade)
        {
            if (entidade.Id > 0)
            {
                entidade.AlteradoEm = DateTime.Now;
                entidade.AlteradoPor = database.UsuarioLogadoNomeCompleto;
                entidade.AlteradoRF = database.UsuarioLogadoRF;
                await database.Conexao.UpdateAsync(entidade);
                await AuditarAsync(entidade.Id, "A");
            }
            else
            {
                entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;
                entidade.CriadoRF = database.UsuarioLogadoRF;
                entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));
                await AuditarAsync(entidade.Id, "I");
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

        private async Task AuditarAsync(long identificador, string acao)
        {
            await database.Conexao.InsertAsync<Auditoria>(new Auditoria()
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