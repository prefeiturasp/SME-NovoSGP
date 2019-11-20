using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtribuicaoEsporadica : RepositorioBase<AtribuicaoEsporadica>, IRepositorioAtribuicaoEsporadica
    {
        public RepositorioAtribuicaoEsporadica(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<AtribuicaoEsporadica>> ListarPaginada(Paginacao paginacao,int anoLetivo, string dreId, string ueId, string codigoRF)
        {
            var retorno = new PaginacaoResultadoDto<AtribuicaoEsporadica>();

            var sql = MontaQueryCompleta(paginacao, codigoRF);

            var parametros = new { inicioAno = new DateTime(anoLetivo, 1, 1), fimAno = new DateTime(anoLetivo, 12, 31), dreId, ueId, codigoRF };

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, parametros))
            {
                retorno.Items = multi.Read<AtribuicaoEsporadica>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static string MontaQueryCompleta(Paginacao paginacao, string codigoRF)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, codigoRF, contador: false);

            sql.AppendLine(";");

            MontaQueryConsulta(paginacao, sql, codigoRF, contador: true);

            return sql.ToString();
        }

        private static void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, string codigoRF, bool contador = false)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltro(sql, codigoRF);

            if (!contador)
                sql.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");
        }

        private static void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine($"select {(contador ? "count(*)" : "*")} from atribuicao_esporadica where excluido = false");
        }

        private static void ObtenhaFiltro(StringBuilder sql, string codigoRF)
        {
            sql.AppendLine("and data_inicio >= @inicioAno and data_inicio <= @fimAno");
            sql.AppendLine("and data_fim >= @inicioAno and data_fim <= @fimAno");
            sql.AppendLine("and dre_id = @dreId");
            sql.AppendLine("and ue_id = @ueId");

            if (!string.IsNullOrWhiteSpace(codigoRF))
                sql.AppendLine("and professor_rf = @codigoRF");
        }
    }
}