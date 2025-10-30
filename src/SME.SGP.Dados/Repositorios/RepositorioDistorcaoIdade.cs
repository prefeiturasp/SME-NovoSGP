using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDistorcaoIdade : IRepositorioDistorcaoIdade
    {
        private readonly ISgpContextConsultas database;
        public RepositorioDistorcaoIdade(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ConsolidacaoDistorcaoIdadeDto>> ObterDistorcaoIdade(FiltroPainelEducacionalDistorcaoIdade filtro)
        {
            string query = @"SELECT 
                                    modalidade
                                  , ano
                                  , quantidade_alunos AS QuantidadeAlunos  
                           FROM painel_educacional_consolidacao_distorcao_serie_idade
                           WHERE 1 = 1 
                           AND ano_letivo = @anoLetivo";

            if (!string.IsNullOrEmpty(filtro.CodigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (!string.IsNullOrEmpty(filtro.CodigoUe))
                query += " AND codigo_ue = @codigoUe ";

            return await database.Conexao.QueryAsync<ConsolidacaoDistorcaoIdadeDto>(query, new
            {
                anoLetivo = filtro.AnoLetivo,
                codigoDre = filtro.CodigoDre,
                codigoUe = filtro.CodigoUe
            });
        }
    }
}
