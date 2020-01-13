using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroPoa : RepositorioBase<RegistroPoa>, IRepositorioRegistroPoa
    {
        public RepositorioRegistroPoa(ISgpContext sgpContext) : base(sgpContext)
        {
        }

        public async Task<PaginacaoResultadoDto<RegistroPoa>> ListarPaginado(string codigoRf, string dreId, int mes, string ueId, string titulo, int anoLetivo, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<RegistroPoa>();

            var sql = MontaQueryCompleta(paginacao, titulo, mes);

            var parametros = new { codigoRf, mes, ueId, dreId, titulo = $"%{titulo?.ToLower()}%", anoLetivo };

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, parametros))
            {
                retorno.Items = multi.Read<RegistroPoa>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, string titulo, int mes)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(sql, paginacao, titulo, mes, contador: false);

            sql.AppendLine(";");

            MontaQueryConsulta(sql, paginacao, titulo, mes, contador: true);

            return sql.ToString();
        }

        private void MontaQueryConsulta(StringBuilder sql, Paginacao paginacao, string titulo, int mes, bool contador)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltros(sql, titulo, mes);

            if (!contador)
                sql.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
        }

        private void ObtenhaCabecalho(StringBuilder sql, bool contador)
        {
            sql.AppendLine($"select {ObtenhaCampos(contador)} from registro_poa");
        }

        private string ObtenhaCampos(bool contador)
        {
            return contador ? "count(*)" : "id, codigo_rf, mes, titulo, ano_letivo, descricao, dre_id, ue_id ";
        }

        private void ObtenhaFiltros(StringBuilder sql, string titulo, int mes)
        {
            sql.AppendLine("where excluido = false");
            sql.AppendLine("and ano_letivo = @anoLetivo");
            sql.AppendLine("and codigo_rf = @codigoRf");
            sql.AppendLine("and ue_id = @ueId");
            sql.AppendLine("and dre_id = @dreId");

            if (mes > 0)
                sql.AppendLine("and mes = @mes");

            if (!string.IsNullOrWhiteSpace(titulo))
                sql.AppendLine("and lower(titulo) like @titulo");
        }
    }
}