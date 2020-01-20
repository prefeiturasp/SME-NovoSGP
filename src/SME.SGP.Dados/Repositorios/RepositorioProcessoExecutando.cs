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
