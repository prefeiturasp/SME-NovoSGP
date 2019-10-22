using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoTipo : RepositorioBase<EventoTipo>, IRepositorioEventoTipo
    {
        public RepositorioEventoTipo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<PaginacaoResultadoDto<EventoTipo>> ListarTipos(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao)
        {
            var retorno = new PaginacaoResultadoDto<EventoTipo>();

            StringBuilder sql = MontaQueryCompleta(eventoLocalOcorrencia, eventoLetivo, descricao, paginacao);

            var parametros = new { local_ocorrencia = eventoLocalOcorrencia, letivo = eventoLetivo, descricao = $"%{descricao}%" };

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = multi.Read<EventoTipo>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private static StringBuilder MontaQueryCompleta(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao)
        {
            StringBuilder sql = new StringBuilder();

            MontaQueryConsulta(eventoLocalOcorrencia, eventoLetivo, descricao, paginacao, sql, contador: false);

            sql.AppendLine(";");

            MontaQueryConsulta(eventoLocalOcorrencia, eventoLetivo, descricao, paginacao, sql, contador: true);
            return sql;
        }

        private static void MontaQueryConsulta(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, Paginacao paginacao, StringBuilder sql, bool contador)
        {
            ObtenhaCabecalhoConsulta(sql, contador);

            ObtenhaFiltroConsulta(eventoLocalOcorrencia, eventoLetivo, descricao, sql);

            if (!contador)
                sql.AppendLine("order by id desc");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");
        }

        private static void ObtenhaCabecalhoConsulta(StringBuilder sql, bool contador)
        {
            sql.AppendLine($"select {(contador ? "count(*)" : "*")} from evento_tipo where excluido = false");
        }

        private static void ObtenhaFiltroConsulta(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao, StringBuilder sql)
        {
            if (eventoLocalOcorrencia != 0)
                sql.AppendLine("and local_ocorrencia = @local_ocorrencia");

            if (eventoLetivo != 0)
                sql.AppendLine("and letivo = @letivo");

            if (!string.IsNullOrWhiteSpace(descricao))
                sql.AppendLine("and descricao like @descricao");
        }
    }
}