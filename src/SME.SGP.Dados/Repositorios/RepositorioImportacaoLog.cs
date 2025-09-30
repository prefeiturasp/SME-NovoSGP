using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interface;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioImportacaoLog : RepositorioBase<ImportacaoLog>, IRepositorioImportacaoLog
    {
        public RepositorioImportacaoLog(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<ImportacaoLog>> ObterImportacaoLogPaginada(Paginacao paginacao, FiltroPesquisaImportacaoDto filtro)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT id AS Id,");
            query.AppendLine("       nome_arquivo AS NomeArquivo,");
            query.AppendLine("       status_importacao AS StatusImportacao,");
            query.AppendLine("       total_registros AS TotalRegistros,");
            query.AppendLine("       registros_processados AS RegistrosProcessados");
            query.AppendLine("FROM importacao_log");
            query.AppendLine("WHERE 1=1");

            if (filtro.ImportacaoLogId.HasValue)
                query.AppendLine("AND id = @ImportacaoLogId");

            query.AppendLine("ORDER BY criado_em DESC");
            if (paginacao.QuantidadeRegistros > 0)
            {
                query.AppendFormat("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
            }

            query.AppendLine("; SELECT COUNT(id) FROM importacao_log WHERE 1=1");

            if (filtro.ImportacaoLogId.HasValue)
                query.AppendLine("AND id = @ImportacaoLogId");

            var retorno = new PaginacaoResultadoDto<ImportacaoLog>();

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new { ImportacaoLogId = filtro.ImportacaoLogId }))
            {
                retorno.Items = (await multi.ReadAsync<ImportacaoLog>()).ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
                ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
                : 1; 

            return retorno;
        }
    }
}
