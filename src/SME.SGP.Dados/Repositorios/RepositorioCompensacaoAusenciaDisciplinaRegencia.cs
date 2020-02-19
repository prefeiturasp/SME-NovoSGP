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
    public class RepositorioCompensacaoAusenciaDisciplinaRegencia : RepositorioBase<CompensacaoAusenciaDisciplinaRegencia>, IRepositorioCompensacaoAusenciaDisciplinaRegencia
    {
        public RepositorioCompensacaoAusenciaDisciplinaRegencia(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> ObterPorCompensacao(long compensacaoId)
        {
            var query = @"select * from compensacao_ausencia_disciplina_regencia where not excluido and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaDisciplinaRegencia>(query, new { compensacaoId });
        }
    }
}
