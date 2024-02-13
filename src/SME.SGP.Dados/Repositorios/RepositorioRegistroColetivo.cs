using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroColetivo : RepositorioBase<RegistroColetivo>, IRepositorioRegistroColetivo
    {
        public RepositorioRegistroColetivo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<RegistroColetivoListagemDto>> ListarPaginado(int anoLetivo, long dreId, long? ueId,
                                                                                             DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId, 
                                                                                             Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao, anoLetivo, dreId, ueId,
                                          dataReuniaoInicio, dataReuniaoFim,
                                          tiposReuniaoId);

            var parametros = new
            {
                anoLetivo,
                dreId,
                ueId,
                dataReuniaoInicio,
                dataReuniaoFim,
                tiposReuniaoId
            };

            var retorno = new PaginacaoResultadoDto<RegistroColetivoListagemDto>();

            using (var registrosColetivos = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = registrosColetivos.Read<RegistroColetivoListagemDto>();
                retorno.TotalRegistros = registrosColetivos.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, int anoLetivo, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, anoLetivo, dreId, ueId,
                                          dataReuniaoInicio, dataReuniaoFim,
                                          tiposReuniaoId);
            sql.AppendLine(";");
            MontaQueryConsulta(paginacao, sql, contador: true, anoLetivo, dreId, ueId,
                                          dataReuniaoInicio, dataReuniaoFim,
                                          tiposReuniaoId);
            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, int anoLetivo, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            ObterCabecalho(sql, contador);

            ObterFiltro(sql, anoLetivo, dreId, ueId,
                        dataReuniaoInicio, dataReuniaoFim,
                        tiposReuniaoId);

            if (!contador)
                sql.AppendLine(" order by rc.data_registro desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
        }

        private static void ObterCabecalho(StringBuilder sql, bool contador)
        {
            var sqlSelect = $@" select ";
            sql.AppendLine(sqlSelect);
            if (contador)
                sql.AppendLine("count(rcue.id) ");
            else
            {
                sql.AppendLine(@"rcue.id, rc.data_registro as dataReuniao,
                                 trn.titulo as tipoReuniaoDescricao,
                                 u.tipo_escola as tipoEscola,
                                 u.nome as nomeUe,
                                 rc.criado_por as nomeUsuarioCriador,
                                 rc.criado_rf as rfUsuarioCriador");
            }

            sql.AppendLine(@"from registrocoletivo_ue rcue
                             inner join registrocoletivo rc on rc.id = rcue.registrocoletivo_id 
                             inner join tipo_reuniao_naapa trn on trn.id = rc.tipo_reuniao_id 
                             inner join ue u on u.id = rcue.ue_id");
        }

        private void ObterFiltro(StringBuilder sql, int anoLetivo, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            sql.AppendLine(@" where not rc.excluido 
                                    and t.ano_letivo = @anoLetivo
                                    and ue.dre_Id = @dreId");

            if (ueId.HasValue)
                sql.AppendLine(@" and u.id = @ueId ");

            if (tiposReuniaoId.PossuiRegistros())
                sql.AppendLine(" and trn.id = ANY(@tiposReuniaoId) ");

            if (dataReuniaoInicio.HasValue)
                sql.AppendLine(" and rc.data_registro >= @dataReuniaoInicio ");

            if (dataReuniaoFim.HasValue)
                sql.AppendLine(" and rc.data_registro <= @dataReuniaoFim");
        }

    }
}
