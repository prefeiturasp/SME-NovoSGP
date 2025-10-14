using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<PainelEducacionalAbandonoUe>> ObterAbandonoUe(int anoLetivo, string codigoDre, string codigoUe, string modalidade, int numeroPagina, int numeroRegistros)
        {
            var offset = (numeroPagina - 1) * numeroRegistros;
            var query = MontarQuery(codigoDre, codigoUe, modalidade);

            var registros = await database.Conexao.QueryAsync<PainelEducacionalAbandonoUe>(query, new { anoLetivo, codigoUe, modalidade, codigoDre, offset, numeroRegistros });
            var totalRegistros = registros.FirstOrDefault()?.TotalRegistros ?? 0;
            var totalPaginas = totalRegistros == 0 ? 0 : (int)Math.Ceiling((double)totalRegistros / numeroRegistros);

            foreach (var item in registros)
            {
                item.TotalRegistros = totalRegistros;
                item.TotalPaginas = totalPaginas;
            }

            return registros;
        }

        private static string MontarQuery(string codigoDre, string codigoUe, string modalidade)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
                SELECT 
                    codigo_turma AS CodigoTurma,
                    nome_turma AS NomeTurma,
                    quantidade_desistencias AS QuantidadeDesistencias,
                    modalidade AS Modalidade,
                    COUNT(*) OVER() AS TotalRegistros
                FROM painel_educacional_consolidacao_abandono_ue
                WHERE ano_letivo = @anoLetivo");

            if (!string.IsNullOrEmpty(codigoUe)) sb.AppendLine(" AND codigo_ue = @codigoUe");

            if (!string.IsNullOrEmpty(codigoDre)) sb.AppendLine(" AND codigo_dre = @codigoDre");

            if (!string.IsNullOrEmpty(modalidade)) sb.AppendLine(" AND modalidade = @modalidade");

            sb.AppendLine(" ORDER BY nome_turma");
            sb.AppendLine(" OFFSET @offset ROWS FETCH NEXT @numeroRegistros ROWS ONLY");

            return sb.ToString();
        }
    }
}