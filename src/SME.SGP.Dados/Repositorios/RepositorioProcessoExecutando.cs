using Dapper;
using Dommel;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioProcessoExecutando : IRepositorioProcessoExecutando
    {
        protected readonly ISgpContext database;

        public RepositorioProcessoExecutando(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<bool> ObterAulaEmManutencaoAsync(long aulaId)
        {
            var query = @"select 1
                            from processo_executando
                           where tipo_processo = 2
                             and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<int?>(query, new { aulaId }) != null;
        }

        public async Task<ProcessoExecutando> ObterProcessoCalculoFrequencia(string turmaId, string disciplinaId, int bimestre)
        {
            var query = @"select * 
                            from processo_executando
                           where tipo_processo = 1
                             and turma_id = @turmaId
                             and disciplina_id = @disciplinaId
                             and bimestre = @bimestre";

            return await database.Conexao.QueryFirstOrDefaultAsync<ProcessoExecutando>(query, new { turmaId, disciplinaId, bimestre });
        }

        public void Remover(ProcessoExecutando processo)
            => database.Conexao.Delete(processo);

        public async Task RemoverAsync(ProcessoExecutando processo)
            => await database.Conexao.DeleteAsync(processo);

        public async Task RemoverIdsAsync(long[] ids)
        {

            var query = @"delete
                            from processo_executando
                           where id IN (#ids)";

            await database.Conexao.ExecuteAsync(query.Replace("#ids", string.Join(",", ids)));
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
