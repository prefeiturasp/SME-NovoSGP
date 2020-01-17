using Dapper;
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
    public class RepositorioProcessoExecutando : RepositorioBase<ProcessoExecutando>, IRepositorioProcessoExecutando
    {
        public RepositorioProcessoExecutando(ISgpContext database): base(database) { }

        public async Task<ProcessoExecutando> ObterProcessoCalculoFrequencia(string turmaId, string disciplinaId)
        {
            var query = @"select * 
                            from processo_executando
                           where tipo_processo = 1
                             and turma_id = @turmaId
                             and disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<ProcessoExecutando>(query, new { turmaId, disciplinaId });
        }
    }
}
