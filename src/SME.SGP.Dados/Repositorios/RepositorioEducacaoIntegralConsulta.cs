using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEducacaoIntegralConsulta : IRepositorioEducacaoIntegralConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioEducacaoIntegralConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<DadosParaConsolidarEducacaoIntegralDto>> ObterEducacaoIntegral(FiltroPainelEducacionalEducacaoIntegral filtro)
        {
            string query = @"SELECT 
                                    modalidade_turma AS modalidadeTurma
                                  , ano
                                  , quantidade_alunos_integral AS QuantidadeAlunosIntegral
                                  , quantidade_alunos_parcial AS QuantidadeAlunosParcial
                           FROM painel_educacional_consolidacao_educacao_integral
                           WHERE ano_letivo = @anoLetivo";

            if (!string.IsNullOrEmpty(filtro.CodigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (!string.IsNullOrEmpty(filtro.CodigoUe))
                query += " AND codigo_ue = @codigoUe ";

            return await database.Conexao.QueryAsync<DadosParaConsolidarEducacaoIntegralDto>(query, new
            {
                anoLetivo = filtro.AnoLetivo,
                codigoDre = filtro.CodigoDre,
                codigoUe = filtro.CodigoUe
            });
        }
    }
}
