using Dapper;
using Dommel;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagem : IRepositorioObjetivoAprendizagem
    {
        private readonly string connectionString;

        public RepositorioObjetivoAprendizagem(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("SGP-PostgresDev");
        }

        public async Task AtualizarAsync(ObjetivoAprendizagem objetivoAprendizagem)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                await conexao.UpdateAsync(objetivoAprendizagem);
                conexao.Close();
            }
        }

        public async Task<IEnumerable<ObjetivoAprendizagem>> ListarAsync()
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                var objetivos = await conexao.QueryAsync<ObjetivoAprendizagem>("select * from objetivo_aprendizagem");
                conexao.Close();
                return objetivos;
            }
        }

        public async Task<ObjetivoAprendizagem> ObterPorIdAsync(long id)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                var objetivo = await conexao.QueryFirstOrDefaultAsync<ObjetivoAprendizagem>("select * from objetivo_aprendizagem where id = @id", new { id });
                conexao.Close();
                return objetivo;
            }
        }

        public async Task ReativarAsync(long id)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                await conexao.ExecuteAsync("update objetivo_aprendizagem set excluido = false where id = @id", new { id });
                conexao.Close();
            }
        }

        public async Task SalvarAsync(ObjetivoAprendizagem objetivoAprendizagem)
        {
            var query = @"INSERT
	                        INTO
	                        objetivo_aprendizagem (ano_turma,
	                        atualizado_em,
	                        codigo,
	                        componente_curricular_id,
	                        criado_em,
	                        descricao,
	                        id)
                        VALUES (@anoTurma,
                        @atualizadoEm,
                        @codigo,
                        @componenteCurricularId,
                        @criadoEm,
                        @descricao,
                        @id)";

            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                await conexao.ExecuteAsync(
                    query,
                    new
                    {
                        anoTurma = objetivoAprendizagem.AnoTurma,
                        atualizadoEm = objetivoAprendizagem.AtualizadoEm,
                        codigo = objetivoAprendizagem.CodigoCompleto,
                        componenteCurricularId = objetivoAprendizagem.ComponenteCurricularId,
                        criadoEm = objetivoAprendizagem.CriadoEm,
                        descricao = objetivoAprendizagem.Descricao,
                        id = objetivoAprendizagem.Id
                    });
                conexao.Close();
            }
        }
    }
}