using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAusenciaAluno : RepositorioBase<RegistroAusenciaAluno>, IRepositorioRegistroAusenciaAluno
    {
        private readonly string connectionString;
        public RepositorioRegistroAusenciaAluno(ISgpContext database, IConfiguration configuration) : base(database)
        {
            this.connectionString = configuration.GetConnectionString("SGP_Postgres");
        }

        public bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId)
        {
            var query = @"update
                            registro_ausencia_aluno
                        set
                            excluido = true
                        where
                            registro_frequencia_id = @registroFrequenciaId";

            return database.Conexao.Execute(query, new { registroFrequenciaId }) > 0;
        }

        public async Task SalvarVarios(List<RegistroAusenciaAluno> entidades)
        {
            var sql = @"copy registro_ausencia_aluno (                                         
                                        codigo_aluno, 
                                        numero_aula, 
                                        registro_frequencia_id,
                                        migrado,
                                        criado_por,
                                        criado_rf,
                                        criado_em)
                            from
                            stdin (FORMAT binary)";

            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                using (var writer = conexao.BeginBinaryImport(sql))
                {
                    foreach (var entidade in entidades)
                    {
                        writer.StartRow();
                        writer.Write(entidade.CodigoAluno, NpgsqlDbType.Varchar);
                        writer.Write(entidade.NumeroAula, NpgsqlDbType.Integer); ;
                        writer.Write(entidade.RegistroFrequenciaId, NpgsqlDbType.Bigint);
                        writer.Write(entidade.Migrado, NpgsqlDbType.Boolean);
                        writer.Write(database.UsuarioLogadoNomeCompleto, NpgsqlDbType.Varchar);
                        writer.Write(database.UsuarioLogadoRF, NpgsqlDbType.Varchar);
                        writer.Write(entidade.CriadoEm, NpgsqlDbType.Timestamp);
                    }
                    await Task.FromResult(writer.Complete());
                    conexao.Close();
                }
            }
        }

        public async Task ExcluirVarios(List<long> idsParaExcluir)
        {
            var query = "delete from registro_ausencia_aluno where = any(@idsParaExcluir)";

            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                await conexao.ExecuteAsync(
                    query,
                    new
                    {
                        idsParaExcluir

                    });
                conexao.Close();
            }
        }
    }
}