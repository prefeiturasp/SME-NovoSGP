using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCompensacaoAusenciaAluno : RepositorioBase<CompensacaoAusenciaAluno>, IRepositorioCompensacaoAusenciaAluno
    {
        public RepositorioCompensacaoAusenciaAluno(ISgpContext database) : base(database)
        {
        }

        public Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId)
        {
            var query = @"select * 
                            from compensacao_ausencia_aluno 
                        where not excluido 
                          and compensacao_ausencia_id = @compensacaoId";

            return await database.Conexao.QueryAsync<CompensacaoAusenciaAluno>(query, new { compensacaoId });
        }
    }
}
