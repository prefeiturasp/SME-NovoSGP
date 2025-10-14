using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAbandonoUe : IRepositorioPainelEducacionalAbandonoUe
    {
        private readonly ISgpContextConsultas database;

        public RepositorioPainelEducacionalAbandonoUe(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalAbandonoUe>> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros)
        {
            var query = MontarQueryConsulta(codigoDre, codigoUe, modalidade);
            var queryPaginacao = MontarQueryConsulta(codigoDre, codigoUe, modalidade, true);

            var paginacao = new Paginacao(numeroPagina, numeroRegistros);

            var parametrosTotalPaginas = MontarParametros(anoLetivo, codigoDre, codigoUe, modalidade, null);
            var parametrosQuery = MontarParametros(anoLetivo, codigoDre, codigoUe, modalidade, paginacao);

            var totalRegistros = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryPaginacao, parametrosTotalPaginas);
            var totalPaginas = totalRegistros == 0 ? 0 : (int)Math.Ceiling((double)totalRegistros / paginacao.QuantidadeRegistros);

            var registros = await database.Conexao.QueryAsync<PainelEducacionalAbandonoUe>(query, parametrosQuery);

            return ObterResultado(registros, totalPaginas, totalRegistros);
        }

        private static string MontarQueryConsulta(string codigoDre, string codigoUe, string modalidade, bool ehContador = false)
        {
            var sb = ObterSelectParaOrdenacaoConsulta(ehContador);
            if (!string.IsNullOrEmpty(codigoUe)) sb.AppendLine(" AND codigo_ue = @codigoUe");
            if (!string.IsNullOrEmpty(codigoDre)) sb.AppendLine(" AND codigo_dre = @codigoDre");
            if (!string.IsNullOrEmpty(modalidade)) sb.AppendLine(" AND modalidade = @modalidade");
            if (!ehContador)
            {
                sb.AppendLine(" ORDER BY nome_turma");
                sb.AppendLine(" OFFSET @offset ROWS FETCH NEXT @numeroRegistros ROWS ONLY");
            }
            return sb.ToString();
        }

        private static StringBuilder ObterSelectParaOrdenacaoConsulta(bool ehContador)
        {
            var sb = new StringBuilder();

            return ehContador
                 ? sb.AppendLine("SELECT COUNT(*) FROM painel_educacional_consolidacao_abandono_ue WHERE ano_letivo = @anoLetivo")
                 : sb.AppendLine(@"SELECT codigo_turma AS CodigoTurma, nome_turma AS NomeTurma, quantidade_desistencias AS QuantidadeDesistencias, modalidade AS Modalidade FROM painel_educacional_consolidacao_abandono_ue WHERE ano_letivo = @anoLetivo");
        }

        private static object MontarParametros(int anoLetivo, string codigoDre, string codigoUe, string modalidade, Paginacao paginacao)
        {
            return paginacao != null
                ? ObterParametrosParaConsultaQuery(anoLetivo, codigoDre, codigoUe, modalidade, paginacao)
                : ObterParametrosParaPaginacao(anoLetivo, codigoDre, codigoUe, modalidade);
        }

        private static object ObterParametrosParaConsultaQuery(int anoLetivo, string codigoDre, string codigoUe, string modalidade, Paginacao paginacao)
        {
            return new
            {
                anoLetivo,
                codigoUe,
                modalidade,
                codigoDre,
                offset = paginacao.QuantidadeRegistrosIgnorados,
                numeroRegistros = paginacao.QuantidadeRegistros
            };
        }

        private static object ObterParametrosParaPaginacao(int anoLetivo, string codigoDre, string codigoUe, string modalidade)
        {
            return new { anoLetivo, codigoDre, codigoUe, modalidade };
        }

        private static PaginacaoResultadoDto<PainelEducacionalAbandonoUe> ObterResultado(IEnumerable<PainelEducacionalAbandonoUe> registros, int totalPaginas, int totalRegistros)
        {
            return new PaginacaoResultadoDto<PainelEducacionalAbandonoUe>
            {
                Items = registros,
                TotalPaginas = totalPaginas,
                TotalRegistros = totalRegistros
            };
        }
    }
}