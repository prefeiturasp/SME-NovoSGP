using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public abstract class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly ISgpContext database;
        private readonly IServicoAuditoria servicoAuditoria;

        protected RepositorioBase(ISgpContext database, IServicoAuditoria servicoAuditoria)
        {
            this.database = database;
            this.servicoAuditoria = servicoAuditoria ?? throw new ArgumentNullException(nameof(servicoAuditoria));
        }

        public virtual async Task<IEnumerable<T>> ListarAsync()
        {
            return await database.Conexao.GetAllAsync<T>();
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
            if (entidade.NaoEhNulo())
                AuditarAsync(entidade.Id, "E").Wait();
        }

        public virtual void Remover(T entidade)
        {
            database.Conexao.Delete(entidade);
            AuditarAsync(entidade.Id, "E").Wait();
        }
        
        public virtual async Task RemoverAsync(T entidade)
        {
            await database.Conexao.DeleteAsync(entidade);
            await AuditarAsync(entidade.Id, "E");
        }

        public virtual long Salvar(T entidade)
        {
            if (entidade.Id > 0)
            {
                entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
                entidade.AlteradoPor = database.UsuarioLogadoNomeCompleto;
                entidade.AlteradoRF = database.UsuarioLogadoRF;
                database.Conexao.Update(entidade);
                AuditarAsync(entidade.Id, "A").Wait();
            }
            else
            {
                entidade.CriadoPor = database.UsuarioLogadoNomeCompleto;
                entidade.CriadoRF = database.UsuarioLogadoRF;
                entidade.Id = (long)database.Conexao.Insert(entidade);
                AuditarAsync(entidade.Id, "I").Wait();
            }

            return entidade.Id;
        }

        public virtual async Task<long> SalvarAsync(T entidade)
        {
            if (entidade.Id > 0)
            {                
                entidade.AlteradoEm = DateTimeExtension.HorarioBrasilia();
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

        public virtual async Task<bool> Exists(long id, string coluna = null)
        {
            var tableName = Dommel.Resolvers.Table(typeof(T), database.Conexao);
            var columName = coluna ?? "id";
            return await database.Conexao.ExecuteScalarAsync<bool>($"select count(1) from {tableName} where {columName}=@id", new { id });
        }

        public virtual async Task<long> RemoverLogico(long id, string coluna = null)
        {
            var tableName = Dommel.Resolvers.Table(typeof(T), database.Conexao);
            var columName = coluna ?? "id";

            var query = $@"update {tableName} 
                            set excluido = true
                              , alterado_por = @alteradoPor
                              , alterado_rf = @alteradoRF 
                              , alterado_em = @alteradoEm
                        where {columName}=@id RETURNING id";

            return await database.Conexao.ExecuteScalarAsync<long>(query
                , new
                {
                    id,
                    alteradoPor = database.UsuarioLogadoNomeCompleto,
                    alteradoRF = database.UsuarioLogadoRF,
                    alteradoEm = DateTimeExtension.HorarioBrasilia()
                });
        }
        
        public virtual async Task<bool> RemoverLogico(long[] ids, string coluna = null)
        {
            var tableName = Dommel.Resolvers.Table(typeof(T), database.Conexao);
            var columName = coluna ?? "id";

            var query = $@"update {tableName} 
                            set excluido = true
                              , alterado_por = @alteradoPor
                              , alterado_rf = @alteradoRF 
                              , alterado_em = @alteradoEm
                        where {columName}= ANY(@id)";

            return await database.Conexao.ExecuteScalarAsync<bool>(query
                , new
                {
                    id = ids,
                    alteradoPor = database.UsuarioLogadoNomeCompleto,
                    alteradoRF = database.UsuarioLogadoRF,
                    alteradoEm = DateTimeExtension.HorarioBrasilia()
                });
        }

        protected async Task AuditarAsync(long identificador, string acao)
        {
            var perfil = database.PerfilUsuario != String.Empty ? Guid.Parse(database.PerfilUsuario) : (Guid?)null;

            var auditoria = new Auditoria()
            {
                Data = DateTimeExtension.HorarioBrasilia(),
                Entidade = typeof(T).Name.ToLower(),
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Perfil = perfil,
                Acao = acao,
                Administrador = database.Administrador
            };

            await servicoAuditoria.Auditar(auditoria);
        }
    }
}