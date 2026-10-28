using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public abstract class RepositorioBase<T>
        : IRepositorioBase<T>
        where T : EntidadeBase
    {
        protected readonly ISgpContext database;

        private readonly IServicoAuditoria servicoAuditoria;

        protected RepositorioBase(
            ISgpContext database,
            IServicoAuditoria servicoAuditoria)
        {
            this.database = database ??
                throw new ArgumentNullException(
                    nameof(database));

            this.servicoAuditoria = servicoAuditoria ??
                throw new ArgumentNullException(
                    nameof(servicoAuditoria));
        }

        public virtual async Task<IEnumerable<T>>
            ListarAsync()
        {
            return await database.Conexao
                .GetAllMappedAsync<T>();
        }

        public virtual IEnumerable<T> Listar()
        {
            return database.Conexao
                .GetAllMapped<T>();
        }

        public virtual T ObterPorId(long id)
        {
            return database.Conexao
                .GetMapped<T>(id);
        }

        public virtual async Task<T>
            ObterPorIdAsync(long id)
        {
            return await database.Conexao
                .GetMappedAsync<T>(id);
        }

        public virtual void Remover(long id)
        {
            var entidade =
                database.Conexao.GetMapped<T>(id);

            if (entidade == null)
                return;

            database.Conexao.DeleteMapped(entidade);

            AuditarAsync(
                entidade.Id,
                "E")
                .GetAwaiter()
                .GetResult();
        }

        public virtual void Remover(T entidade)
        {
            if (entidade == null)
                return;

            database.Conexao.DeleteMapped(entidade);

            AuditarAsync(
                entidade.Id,
                "E")
                .GetAwaiter()
                .GetResult();
        }

        public virtual async Task RemoverAsync(
            T entidade)
        {
            if (entidade == null)
                return;

            await database.Conexao
                .DeleteMappedAsync(entidade);

            await AuditarAsync(
                entidade.Id,
                "E");
        }

        public virtual long Salvar(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentNullException(
                    nameof(entidade));
            }

            if (entidade.Id > 0)
            {
                PreencherDadosAlteracao(entidade);

                database.Conexao
                    .UpdateMapped(entidade);

                AuditarAsync(
                    entidade.Id,
                    "A")
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                PreencherDadosCriacao(entidade);

                entidade.Id =
                    database.Conexao
                        .InsertMapped(entidade);

                AuditarAsync(
                    entidade.Id,
                    "I")
                    .GetAwaiter()
                    .GetResult();
            }

            return entidade.Id;
        }

        public virtual async Task<long>
            SalvarAsync(T entidade)
        {
            if (entidade == null)
            {
                throw new ArgumentNullException(
                    nameof(entidade));
            }

            if (entidade.Id > 0)
            {
                PreencherDadosAlteracao(entidade);

                await database.Conexao
                    .UpdateMappedAsync(entidade);

                await AuditarAsync(
                    entidade.Id,
                    "A");
            }
            else
            {
                PreencherDadosCriacao(entidade);

                entidade.Id =
                    await database.Conexao
                        .InsertMappedAsync(entidade);

                await AuditarAsync(
                    entidade.Id,
                    "I");
            }

            return entidade.Id;
        }


        [SuppressMessage("Security", "S2077:Formatting SQL queries is security-sensitive")]

        public virtual async Task<bool> Exists(
            long id,
            string coluna = null)
        {
            var map = MapRegistry.GetMap(typeof(T));

            var columnName = string.IsNullOrWhiteSpace(coluna)
                ? map.GetColumnName("Id")
                : map.GetColumnName(coluna);

            var sql =
                "SELECT EXISTS (" +
                "SELECT 1 FROM " +
                QuoteIdentifier(map.TableName) +
                " WHERE " +
                QuoteIdentifier(columnName) +
                " = @id);";

            return await database.Conexao
                .ExecuteScalarAsync<bool>(
                    sql,
                    new { id });
        }


        [SuppressMessage("Security", "S2077:Formatting SQL queries is security-sensitive")]

        public virtual async Task<long>
            RemoverLogico(
                long id,
                string coluna = null)
        {
            var map = MapRegistry.GetMap(typeof(T));

            var columnName = string.IsNullOrWhiteSpace(coluna)
                ? map.GetColumnName("Id")
                : map.GetColumnName(coluna);

            var sql =
                "UPDATE " +
                QuoteIdentifier(map.TableName) +
                " SET " +
                QuoteIdentifier(
                    map.GetColumnName("Excluido")) +
                " = TRUE, " +
                QuoteIdentifier(
                    map.GetColumnName("AlteradoPor")) +
                " = @alteradoPor, " +
                QuoteIdentifier(
                    map.GetColumnName("AlteradoRF")) +
                " = @alteradoRF, " +
                QuoteIdentifier(
                    map.GetColumnName("AlteradoEm")) +
                " = @alteradoEm " +
                "WHERE " +
                QuoteIdentifier(columnName) +
                " = @id " +
                "RETURNING " +
                QuoteIdentifier(
                    map.GetColumnName("Id")) +
                ";";

            
            return await database.Conexao
                .ExecuteScalarAsync<long>(
                    sql,
                    new
                    {
                        id,
                        alteradoPor =
                            database.UsuarioLogadoNomeCompleto,
                        alteradoRF =
                            database.UsuarioLogadoRF,
                        alteradoEm =
                            DateTimeExtension
                                .HorarioBrasilia()
                    });
        }

        public virtual async Task<bool>
            RemoverLogico(
                long[] ids,
                string coluna = null)
        {
            if (ids == null || ids.Length == 0)
                return false;

            var map = MapRegistry.GetMap(typeof(T));

            var columnName = string.IsNullOrWhiteSpace(coluna)
                ? map.GetColumnName("Id")
                : map.GetColumnName(coluna);

            var sql =
                "UPDATE " +
                QuoteIdentifier(map.TableName) +
                " SET " +
                QuoteIdentifier(
                    map.GetColumnName("Excluido")) +
                " = TRUE, " +
                QuoteIdentifier(
                    map.GetColumnName("AlteradoPor")) +
                " = @alteradoPor, " +
                QuoteIdentifier(
                    map.GetColumnName("AlteradoRF")) +
                " = @alteradoRF, " +
                QuoteIdentifier(
                    map.GetColumnName("AlteradoEm")) +
                " = @alteradoEm " +
                "WHERE " +
                QuoteIdentifier(columnName) +
                " = ANY(@ids);";

            var affectedRows =
                await database.Conexao
                    .ExecuteAsync(
                        sql,
                        new
                        {
                            ids,
                            alteradoPor =
                                database.UsuarioLogadoNomeCompleto,
                            alteradoRF =
                                database.UsuarioLogadoRF,
                            alteradoEm =
                                DateTimeExtension
                                    .HorarioBrasilia()
                        });

            return affectedRows > 0;
        }

        private void PreencherDadosCriacao(
            T entidade)
        {
            entidade.CriadoPor =
                database.UsuarioLogadoNomeCompleto;

            entidade.CriadoRF =
                database.UsuarioLogadoRF;

            if (entidade.CriadoEm == default)
            {
                entidade.CriadoEm =
                    DateTimeExtension
                        .HorarioBrasilia();
            }
        }

        private void PreencherDadosAlteracao(
            T entidade)
        {
            entidade.AlteradoEm =
                DateTimeExtension
                    .HorarioBrasilia();

            entidade.AlteradoPor =
                database.UsuarioLogadoNomeCompleto;

            entidade.AlteradoRF =
                database.UsuarioLogadoRF;
        }

        protected async Task AuditarAsync(
            long identificador,
            string acao)
        {
            var perfil =
                !string.IsNullOrWhiteSpace(
                    database.PerfilUsuario)
                        ? Guid.Parse(
                            database.PerfilUsuario)
                        : (Guid?)null;

            var auditoria = new Auditoria
            {
                Data =
                    DateTimeExtension
                        .HorarioBrasilia(),

                Entidade =
                    typeof(T).Name.ToLower(),

                Chave = identificador,

                Usuario =
                    database.UsuarioLogadoNomeCompleto,

                RF =
                    database.UsuarioLogadoRF,

                Perfil = perfil,

                Acao = acao,

                Administrador =
                    database.Administrador
            };

            await servicoAuditoria
                .Auditar(auditoria);
        }

        private static string QuoteIdentifier(
            string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentException(
                    "Identificador SQL inválido.",
                    nameof(identifier));
            }

            return "\"" +
                   identifier.Replace(
                       "\"",
                       "\"\"") +
                   "\"";
        }
    }
}