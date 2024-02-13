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

        public async Task<PaginacaoResultadoDto<RegistroColetivoListagemDto>> ListarPaginado(long dreId, long? ueId,
                                                                                             DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId, 
                                                                                             Paginacao paginacao)
        {
            var query = MontaQueryCompleta(paginacao,  dreId, ueId,
                                          dataReuniaoInicio, dataReuniaoFim,
                                          tiposReuniaoId);

            var parametros = new
            {
                dreId,
                ueId,
                dataReuniaoInicio,
                dataReuniaoFim,
                tiposReuniaoId
            };

            var retorno = new PaginacaoResultadoDto<RegistroColetivoListagemDto>();

            using (var registrosColetivos = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var registrosColetivosUes = registrosColetivos.Read<RegistroColetivoUeListagemDto>();
                retorno.Items = registrosColetivosUes.GroupBy(rcue => rcue.Id)
                                                     .Select(rc => new RegistroColetivoListagemDto()
                                                     {
                                                         Id = rc.Key,
                                                         CriadoPor = rc.FirstOrDefault().CriadoPor,
                                                         DataReuniao = rc.FirstOrDefault().DataReuniao,
                                                         TipoReuniaoDescricao = rc.FirstOrDefault().TipoReuniaoDescricao,
                                                         NomesUe = rc.Select(rcue => rcue.NomeTipoUe).ToArray(),
                                                     });
                retorno.TotalRegistros = registrosColetivos.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryCompleta(Paginacao paginacao, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            var sql = new StringBuilder();

            MontaQueryConsulta(paginacao, sql, contador: false, dreId, ueId,
                                          dataReuniaoInicio, dataReuniaoFim,
                                          tiposReuniaoId);
            sql.AppendLine(";");
            MontaQueryConsulta(paginacao, sql, contador: true, dreId, ueId,
                                          dataReuniaoInicio, dataReuniaoFim,
                                          tiposReuniaoId);
            return sql.ToString();
        }

        private void MontaQueryConsulta(Paginacao paginacao, StringBuilder sql, bool contador, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            ObterCabecalho(sql, contador);

            ObterFiltro(sql, dreId, ueId,
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
                sql.AppendLine("count(distinct rc.id) ");
            else
            {
                sql.AppendLine(@"rc.id, rc.data_registro as dataReuniao,
                                 trn.titulo as tipoReuniaoDescricao,
                                 rc.criado_por as nomeUsuarioCriador,
                                 rc.criado_rf as rfUsuarioCriador,
                                 u.tipo_escola as tipoEscola,
                                 u.nome as nomeUe");
            }

            sql.AppendLine(@"from registrocoletivo_ue rcue
                             inner join registrocoletivo rc on rc.id = rcue.registrocoletivo_id 
                             inner join tipo_reuniao_naapa trn on trn.id = rc.tipo_reuniao_id 
                             inner join ue u on u.id = rcue.ue_id");
        }

        private void ObterFiltro(StringBuilder sql, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            sql.AppendLine(@" where not rc.excluido 
                                    and rc.dre_id = @dreId");

            if (ueId.HasValue)
                sql.AppendLine(@" and rc.id in (select rcue.registrocoletivo_id from registrocoletivo_ue rcue where rcue.ue_id = @ueId) ");

            if (tiposReuniaoId.PossuiRegistros())
                sql.AppendLine(" and trn.id = ANY(@tiposReuniaoId) ");

            if (dataReuniaoInicio.HasValue)
                sql.AppendLine(" and rc.data_registro >= @dataReuniaoInicio ");

            if (dataReuniaoFim.HasValue)
                sql.AppendLine(" and rc.data_registro <= @dataReuniaoFim");
        }

    }
}
