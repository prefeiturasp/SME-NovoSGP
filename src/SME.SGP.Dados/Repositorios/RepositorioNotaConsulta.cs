using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotaConsulta : IRepositorioNotaConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioNotaConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<PaginacaoNotaResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>> ObterNotasVisaoUe(Paginacao paginacao, string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            string querySelect = @"select ano_letivo as AnoLetivo,
                                             codigo_dre as CodigoDre,
                                             codigo_ue as CodigoUe,
                                             bimestre,
                                             modalidade,
                                             serie_turma as AnoTurma,
                                             quantidade_abaixo_media_portugues as QuantidadeAbaixoMediaPortugues,
                                             quantidade_abaixo_media_matematica as QuantidadeAbaixoMediaMatematica,
                                             quantidade_abaixo_media_ciencias as QuantidadeAbaixoMediaCiencias,
                                             quantidade_acima_media_portugues as QuantidadeAcimaMediaPortugues,
                                             quantidade_acima_media_matematica as QuantidadeAcimaMediaMatematica,
                                             quantidade_acima_media_ciencias as QuantidadeAcimaMediaCiencias
                                     from painel_educacional_consolidacao_nota_ue
                                     where 1 = 1 ";

            string queryCount = @"select count(1)
                                     from painel_educacional_consolidacao_nota_ue
                                     where 1 = 1 ";

            string whereClause = "";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                whereClause += " AND codigo_ue = @codigoUe ";

            if (anoLetivo > 0)
                whereClause += " AND ano_letivo = @anoLetivo ";

            if (bimestre > 0)
                whereClause += " AND bimestre = @bimestre ";

            if (modalidade > 0)
                whereClause += " AND modalidade = @modalidade ";

            querySelect += whereClause;
            queryCount += whereClause;

            if (paginacao.QuantidadeRegistros > 0)
                querySelect += $" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ";

            string queryCompleta = querySelect + "; " + queryCount;

            var parametros = new
            {
                codigoUe,
                anoLetivo,
                bimestre,
                modalidade = (int)modalidade
            };

            var retorno = new PaginacaoNotaResultadoDto<PainelEducacionalNotasVisaoUeRetornoSelectDto>();

            using (var multi = await database.Conexao.QueryMultipleAsync(queryCompleta, parametros))
            {
                retorno.Items = multi.Read<PainelEducacionalNotasVisaoUeRetornoSelectDto>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
            ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
            : 1;

            retorno.PaginaAtual = paginacao.QuantidadeRegistros > 0 
                ? (paginacao.QuantidadeRegistrosIgnorados / paginacao.QuantidadeRegistros) + 1 
                : 1;

            retorno.RegistrosPorPagina = paginacao.QuantidadeRegistros;

            return retorno;
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, string anoTurma)
        {
            string query = @"select ano_letivo,
                                         bimestre,
                                         ano_turma,
                                         modalidade,
                                         quantidade_abaixo_media_portugues as QuantidadeAbaixoMediaPortugues,
                                         quantidade_abaixo_media_matematica as QuantidadeAbaixoMediaMatematica,
                                         quantidade_abaixo_media_ciencias as QuantidadeAbaixoMediaCiencias,
                                         quantidade_acima_media_portugues as QuantidadeAcimaMediaPortugues,
                                         quantidade_acima_media_matematica as QuantidadeAcimaMediaMatematica,
                                         quantidade_acima_media_ciencias as QuantidadeAcimaMediaCiencias
                                 from painel_educacional_consolidacao_nota
                                 where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            if (bimestre > 0)
                query += " AND bimestre = @bimestre ";

            if (!string.IsNullOrEmpty(anoTurma))
                query += " AND ano_turma = @anoTurma ";

            return await database.Conexao.QueryAsync<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>(query, new
            {
                codigoDre,
                anoLetivo,
                bimestre,
                anoTurma
            });
        }
    }
}
