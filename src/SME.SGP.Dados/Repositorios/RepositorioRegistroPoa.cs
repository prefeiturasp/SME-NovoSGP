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

        public async Task<PaginacaoResultadoDto<RegistroPoa>> ListarPaginado(string codigoRf, string dreId, int mes,string ueId, string titulo, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<RegistroPoa>();

            var sql = MontaQueryCompleta(paginacao, titulo);

            var parametros = new { codigoRf, mes, ueId, dreId, titulo = $"%{titulo}%"};

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, parametros))
            {
                retorno.Items = multi.Read<RegistroPoa>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, string titulo)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(sql, paginacao, titulo, contador: false);

            sql.AppendLine(";");

            MontaQueryConsulta(sql, paginacao, titulo, contador: true);

            return sql.ToString();
        }

        private void MontaQueryConsulta(StringBuilder sql, Paginacao paginacao, string titulo, bool contador)
        {
            ObtenhaCabecalho(sql, contador);

            ObtenhaFiltros(sql, titulo);

            if (!contador)
                sql.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
        }

        private void ObtenhaCabecalho(StringBuilder sql,bool contador)
        {
            sql.AppendLine($"select {ObtenhaCampos(contador)} from registro_poa");
        }

        private void ObtenhaFiltros(StringBuilder sql, string titulo)
        {
            sql.AppendLine("where");
            sql.AppendLine("codigo_rf = @codigoRf");
            sql.AppendLine("and mes = @mes");
            sql.AppendLine("and ue_id = @ueId");
            sql.AppendLine("and dre_id = @dreId");

            if (!string.IsNullOrWhiteSpace(titulo))
                sql.AppendLine("and titulo like @titulo");
        }

        private string ObtenhaCampos(bool contador)
        {
            return contador ? "count(*)" : "id, codigo_rf, mes, titulo, descricao, dre_id, ue_id ";
        }
    }
}