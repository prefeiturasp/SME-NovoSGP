using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurma: RepositorioBase<FechamentoTurma>, IRepositorioFechamentoTurma
    {
        public RepositorioFechamentoTurma(ISgpContext database): base(database)
        {
        }

        public async Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0)
        {
            var query = new StringBuilder(@"select * 
                            from fechamento_turma 
                           where not excluido 
                            and turma_id = @turmaId ");
            if (periodoId > 0)
                query.AppendLine(" and periodo_escolar_id = @periodoId");

            return await database.Conexao.QueryFirstOrDefaultAsync<FechamentoTurma>(query.ToString(), new { turmaId, periodoId });
        }
    }
}
