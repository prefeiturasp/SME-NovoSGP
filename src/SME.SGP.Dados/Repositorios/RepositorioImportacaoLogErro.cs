using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioImportacaoLogErro : RepositorioBase<ImportacaoLogErro>, IRepositorioImportacaoLogErro
    {
        public RepositorioImportacaoLogErro(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<ImportacaoLogErro>> ObterImportacaoLogErroPaginada(Paginacao paginacao, FiltroPesquisaImportacaoDto filtro)
        {
            var sql = @$"
                    select
                           id 
                         , linha_arquivo as LinhaArquivo
                         , motivo_falha as MotivoFalha                  
                    from importacao_log_erro
                    where importacao_log_id = @importacaoLogId
                    group by 
                            id
                          , linha_arquivo
                          , motivo_falha
                    order by criado_em desc ";

            sql += $" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY; ";

            sql += "select count(id) from importacao_log_erro where importacao_log_id = @importacaoLogId";

            var parametros = new { importacaoLogId = filtro.ImportacaoLogId };

            var retorno = new PaginacaoResultadoDto<ImportacaoLogErro>();

            using (var multi = await database.Conexao.QueryMultipleAsync(sql.ToString(), parametros))
            {
                retorno.Items = await multi.ReadAsync<ImportacaoLogErro>();
                retorno.TotalRegistros = multi.ReadFirst<int>();
                retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);
            }
            return retorno;
        }
    }
}
