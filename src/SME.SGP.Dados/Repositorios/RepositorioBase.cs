using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dommel;
using Npgsql;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public abstract class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly ISgpContext database;       

        protected RepositorioBase(ISgpContext database)
        {
            this.database = database;            
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
            if (entidade != null)
                Auditar(entidade.Id, "E");
        }

        public virtual void Remover(T entidade)
        {
            database.Conexao.Delete(entidade);
            Auditar(entidade.Id, "E");
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

        private void Auditar(long identificador, string acao)
        {
            var perfil = database.PerfilUsuario != String.Empty ? Guid.Parse(database.PerfilUsuario) : (Guid?)null;
            database.Conexao.Insert<Auditoria>(new Auditoria()
            {
                Data = DateTimeExtension.HorarioBrasilia(),
                Entidade = typeof(T).Name.ToLower(),
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Perfil = perfil,
                Acao = acao
            });
        }

        protected async Task AuditarAsync(long identificador, string acao)
        {
            var perfil = database.PerfilUsuario != String.Empty ? Guid.Parse(database.PerfilUsuario) : (Guid?)null;
            await database.Conexao.InsertAsync<Auditoria>(new Auditoria()
            {
                Data = DateTimeExtension.HorarioBrasilia(),
                Entidade = typeof(T).Name.ToLower(),
                Chave = identificador,
                Usuario = database.UsuarioLogadoNomeCompleto,
                RF = database.UsuarioLogadoRF,
                Perfil = perfil,
                Acao = acao
            });
        }
       // public async Task SalvarVariosAsync(IEnumerable<T> entidades)
       // {
            //var table = DommelMapper.Resolvers.Properties(typeof(T));
            //var tableName = DommelMapper.Resolvers.Table(typeof(T));

            //var str = new StringBuilder();
            //str.AppendLine("copy aula ( ");
            //var sql = @"copy aula ( 
            //                            data_aula, 
            //                            disciplina_id, 
            //                            quantidade, 
            //                            recorrencia_aula, 
            //                            tipo_aula, 
            //                            tipo_calendario_id, 
            //                            turma_id, 
            //                            ue_id, 
            //                            professor_rf,
            //                            criado_em,
            //                            criado_por,
            //                            criado_rf)
            //                from
            //                stdin (FORMAT binary)";

            //using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            //{
            //    foreach (var entidade in entidades)
            //    {
            //        writer.StartRow();
       
            //    }
            //    writer.Complete();
            //}
            //await database.Conexao.InsertAsync<Auditoria>(new Auditoria()
            //{
            //    Data = DateTimeExtension.HorarioBrasilia(),
            //    Entidade = typeof(T).Name.ToLower(),
            //    Chave = identificador,
            //    Usuario = database.UsuarioLogadoNomeCompleto,
            //    RF = database.UsuarioLogadoRF,
            //    Acao = acao
            //});
      //  }
    }
}