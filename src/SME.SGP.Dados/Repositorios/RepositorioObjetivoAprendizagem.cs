using Dapper;
using Dommel;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagem : IRepositorioObjetivoAprendizagem
    {
        private readonly string connectionString;

        public RepositorioObjetivoAprendizagem(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("SGP_Postgres");
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

        public async Task<bool> ExistePorCodigoCompletoEAnoTurmaAsync(string codigoCompleto, string anoTurma)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                var existe = await conexao.QueryFirstOrDefaultAsync<bool>(
                    "SELECT EXISTS(SELECT 1 FROM objetivo_aprendizagem WHERE codigo = @codigoCompleto AND excluido = false AND ano_turma = @anoTurma)", 
                    new { codigoCompleto, anoTurma });
                conexao.Close();
                return existe;
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

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> ObterPorAnoEComponenteCurricularId(AnoTurma ano, long componenteCurricularId)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                var objetivos = await conexao.QueryAsync<ObjetivoAprendizagemDto>($@"id, descricao, codigo, 
                        ano_turma as ano, componente_curricular_id as idComponenteCurricular, componente_curricular_id as ComponenteCurricularEolId 
                        from objetivo_aprendizagem 
                        where ano_turma = @ano and 
                        componente_curricular_id = @componente",
                        new { ano = ano.Name(), componente = componenteCurricularId });
                conexao.Close();
                return objetivos;
            }
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> ObterPorComponenteCurricularJuremaIds(long[] juremaIds)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                var objetivos = await conexao.QueryAsync<ObjetivoAprendizagemDto>($@"select id, descricao, codigo, 
                        ano_turma as ano, componente_curricular_id as idComponenteCurricular, componente_curricular_id as ComponenteCurricularEolId 
                        from objetivo_aprendizagem 
                        where componente_curricular_id = ANY(@componentes)",
                        new { componentes = juremaIds });
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

        public async Task InserirAsync(ObjetivoAprendizagem objetivoAprendizagem)
        {
            using (var conexao = new NpgsqlConnection(connectionString))
            {
                await conexao.OpenAsync();

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

                await conexao.ExecuteAsync(query,
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
            }
        }
    }
}