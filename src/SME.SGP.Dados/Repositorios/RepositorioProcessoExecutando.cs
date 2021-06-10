using Dapper;
using Dommel;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioProcessoExecutando : IRepositorioProcessoExecutando
    {
        protected readonly ISgpContext database;
        private readonly IConfiguration configuration;

        public RepositorioProcessoExecutando(ISgpContext database, IConfiguration configuration)
        {
            this.database = database;
            this.configuration = configuration;
        }

        public async Task<bool> ObterAulaEmManutencaoAsync(long aulaId)
        {
            var query = @"select 1
                            from processo_executando
                           where tipo_processo = 2
                             and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new { aulaId }) != null;
        }

        public async Task<IEnumerable<long>> ObterIdsPorFiltrosAsync(int bimestre, string disciplinaId, string turmaId)
        {
            var query = @"select id from processo_executando where   
                             turma_id = @turmaId
                             and disciplina_id = @disciplinaId
                             and bimestre = @bimestre ";

            return await database.Conexao.QueryAsync<long>(query, new { turmaId, disciplinaId, bimestre });
        }

        public async Task<ProcessoExecutando> ObterProcessoCalculoFrequenciaAsync(string turmaId, string disciplinaId, int bimestre, TipoProcesso tipoProcesso)
        {
            var query = @"select * 
                            from processo_executando
                           where tipo_processo = @tipoProcesso
                             and turma_id = @turmaId
                             and disciplina_id = @disciplinaId
                             and bimestre = @bimestre";


            using (var conexao = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres")))
            {
                return await conexao.QueryFirstOrDefaultAsync<ProcessoExecutando>(query, new { turmaId, disciplinaId, bimestre, tipoProcesso = (int)tipoProcesso });
            }
        }

        public async Task<IEnumerable<ProcessoExecutando>> ObterProcessosEmExecucaoAsync(string turmaId, string disciplinaId, int bimestre, TipoProcesso tipoProcesso)
        {
            var query = @"select * 
                            from processo_executando
                           where tipo_processo = @tipoProcesso
                             and turma_id = @turmaId
                             and disciplina_id = @disciplinaId
                             and bimestre = @bimestre
                           order by id";

            return await database.Conexao.QueryAsync<ProcessoExecutando>(query, new { turmaId, disciplinaId, bimestre, tipoProcesso = (int)tipoProcesso });            
        }

        public async Task<bool> ProcessoEstaEmExecucao(TipoProcesso tipoProcesso)
        {
            var query = "select 1 from processo_executando where tipo_processo = @tipoProcesso";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { tipoProcesso = (int)tipoProcesso });
        }

        public void Remover(ProcessoExecutando processo)
            => database.Conexao.Delete(processo);

        public async Task RemoverAsync(ProcessoExecutando processo)
            => await database.Conexao.DeleteAsync(processo);

        public async Task RemoverIdsAsync(long[] ids)
        {
            var query = @"delete
                            from processo_executando
                           where id = ANY(@ids)";

            await database.Conexao.ExecuteAsync(query, new { ids });
        }


        public async Task RemoverPorId(long id)
        {
            var query = @"delete
                            from processo_executando
                           where id = @id";

            await database.Conexao.ExecuteAsync(query, new { id });
        }

        public async Task<long> SalvarAsync(ProcessoExecutando entidade)
        {
            if (entidade.Id > 0)
                await database.Conexao.UpdateAsync(entidade);
            else
                entidade.Id = (long)(await database.Conexao.InsertAsync(entidade));

            return entidade.Id;
        }
    }
}
