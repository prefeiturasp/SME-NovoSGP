using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioImportacaoLog : RepositorioBase<ImportacaoLog>, IRepositorioImportacaoLog
    {
        public RepositorioImportacaoLog(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<ImportacaoLog>> ObterImportacaoLogPaginada (Paginacao paginacao)
        {
            var sql = @$"
                    select 
                           id as Id
                         , nome_arquivo as NomeArquivo
                         , status_importacao as StatusImportacao
                         , total_registros as TotalRegistros
                         , registros_processados as RegistrosProcessados	                       
                    from importacao_log
                    group by 
                            id
                          , nome_arquivo
                          , status_importacao
                          , criado_em
                          , total_registros
                          , registros_processados
                    order by criado_em desc ";

            sql += $" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY; ";

            sql += "select count(id) from importacao_log;";

            var retorno = new PaginacaoResultadoDto<ImportacaoLog>();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString()))
            {
                retorno.Items = await multi.ReadAsync<ImportacaoLog>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
                retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);
            }
            return retorno;
        }
    }
}
