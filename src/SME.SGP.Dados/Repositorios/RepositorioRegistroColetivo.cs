using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioRegistroColetivo : RepositorioBase<RegistroColetivo>, IRepositorioRegistroColetivo
    {
        public RepositorioRegistroColetivo(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<RegistroColetivoListagemDto>> ListarPaginado(long dreId, long? ueId,
                                                                                             DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId, 
                                                                                             Paginacao paginacao)
        {
            var query = MontaQueryConsulta(paginacao, contador: false, dreId, ueId,
                                         dataReuniaoInicio, dataReuniaoFim,
                                         tiposReuniaoId);
            
            var queryContador = MontaQueryConsulta(paginacao, contador: true, dreId, ueId,
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
            using (var registrosColetivosMulti = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                var ueRegistrosColetivos = registrosColetivosMulti.Read<UeRegistroColetivoDto>();
                var registrosColetivos = registrosColetivosMulti.Read<RegistroColetivoListagemDto>().ToList();
                registrosColetivos.ForEach(registro =>
                {
                    registro.NomesUe.AddRange(ueRegistrosColetivos.Where(ue => ue.RegistroColetivoId == registro.Id).Select(ue => ue.NomeFormatado));
                });
                retorno.Items = registrosColetivos;
            }

            retorno.TotalRegistros = await database.Conexao.QueryFirstOrDefaultAsync<int>(queryContador, parametros);
            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQueryConsulta(Paginacao paginacao, bool contador, long dreId, long? ueId,
                                          DateTime? dataReuniaoInicio, DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            var sql = new StringBuilder();
            ObterQueryListagem(sql, contador,
                               dreId, ueId,
                               dataReuniaoInicio, dataReuniaoFim,
                               tiposReuniaoId);

            if (!contador)
                sql.AppendLine(" order by rc.data_registro desc ");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");

            return sql.ToString();
        }

        private static void ObterQueryListagem(StringBuilder sql, bool contador,
                                           long dreId, long? ueId,
                                           DateTime? dataReuniaoInicio, 
                                           DateTime? dataReuniaoFim, long[] tiposReuniaoId)
        {
            if (contador)
            {
                sql.AppendLine(@"select count(distinct rc.id) 
                                 from registrocoletivo rc 
                                 inner join tipo_reuniao_naapa trn on trn.id = rc.tipo_reuniao_id ");

                sql.AppendLine(@" where not rc.excluido 
                                    and rc.dre_id = @dreId");

                if (ueId.HasValue)
                    sql.AppendLine(@" and rc.id in (select rcue.registrocoletivo_id from registrocoletivo_ue rcue where rcue.ue_id = @ueId) ");

                if (tiposReuniaoId.PossuiRegistros())
                    sql.AppendLine(" and trn.id = ANY(@tiposReuniaoId) ");

                if (dataReuniaoInicio.HasValue && dataReuniaoFim.HasValue)
                    sql.AppendLine(@" and rc.data_registro::date between @dataReuniaoInicio and @dataReuniaoFim");
            }
            else
            {
                sql.AppendLine(@"select rc.id as registroColetivoId, u.id, u.ue_id as codigo, u.tipo_escola as tipoEscola,
                                 u.nome as nome
                                 from registrocoletivo_ue rcue
                                 inner join registrocoletivo rc on rc.id = rcue.registrocoletivo_id 
                                 inner join tipo_reuniao_naapa trn on trn.id = rc.tipo_reuniao_id 
                                 inner join ue u on u.id = rcue.ue_id
                                 where not rc.excluido 
                                    and rc.dre_id = @dreId");

                if (ueId.HasValue)
                    sql.AppendLine(@" and rc.id in (select rcue.registrocoletivo_id from registrocoletivo_ue rcue where rcue.ue_id = @ueId) ");

                if (tiposReuniaoId.PossuiRegistros())
                    sql.AppendLine(" and trn.id = ANY(@tiposReuniaoId) ");

                if (dataReuniaoInicio.HasValue && dataReuniaoFim.HasValue)
                    sql.AppendLine(@" and rc.data_registro::date between @dataReuniaoInicio and @dataReuniaoFim");

                sql.AppendLine(@";");
                sql.AppendLine(@"select  rc.id, rc.data_registro as dataReuniao,
                                 trn.titulo as tipoReuniaoDescricao,
                                 rc.criado_por as nomeUsuarioCriador,
                                 rc.criado_rf as rfUsuarioCriador
                                 from registrocoletivo rc 
                                 inner join tipo_reuniao_naapa trn on trn.id = rc.tipo_reuniao_id
                                 where not rc.excluido 
                                       and rc.dre_id = @dreId");

                if (ueId.HasValue)
                    sql.AppendLine(@" and rc.id in (select rcue.registrocoletivo_id from registrocoletivo_ue rcue where rcue.ue_id = @ueId) ");

                if (tiposReuniaoId.PossuiRegistros())
                    sql.AppendLine(" and trn.id = ANY(@tiposReuniaoId) ");

                if (dataReuniaoInicio.HasValue && dataReuniaoFim.HasValue)
                    sql.AppendLine(@" and rc.data_registro::date between @dataReuniaoInicio and @dataReuniaoFim");
            }

           
        }


        public async Task<RegistroColetivoCompletoDto> ObterRegistroColetivoCompletoPorId(long id)
        {
            var query = @"select rc.id, d.id as DreId, d.dre_id as codigoDre, d.nome as nomeDre, extract(year from rc.criado_em) as AnoLetivo, trn.id as tipoReuniaoId,
                                 trn.titulo as tipoReuniaoDescricao, rc.data_registro as dataRegistro, 
                                 rc.quantidade_participantes as quantidadeParticipantes,
                                 rc.quantidade_educadores as quantidadeEducadores,
                                 rc.quantidade_educandos as quantidadeEducandos,
                                 rc.quantidade_cuidadores as quantidadeCuidadores,
                                 rc.descricao, rc.observacao,
                                 rc.alterado_em as AlteradoEm,
                                 rc.alterado_por as AlteradoPor,
                                 rc.alterado_rf as AlteradoRF,
                                 rc.criado_em as CriadoEm,
                                 rc.criado_por as CriadoPor,
                                 rc.criado_rf as CriadoRF 
                          from  registrocoletivo rc  
                          inner join tipo_reuniao_naapa trn on trn.id = rc.tipo_reuniao_id 
                          inner join dre d on d.id = rc.dre_id 
                          where rc.id = @id
                                and not rc.excluido
                          ;
                          select u.tipo_escola as tipoEscola,
                                 u.nome as nome, u.id, u.ue_id as codigo 
                          from registrocoletivo_ue ru
                          inner join ue u on u.id = ru.ue_id 
                          where ru.registrocoletivo_id = @id
                          ;
                          select a.id, a.codigo, a.nome  
                          from registrocoletivo_anexo ra 
                          inner join arquivo a ON a.id = ra.arquivo_id  
                          where ra.registrocoletivo_id = @id
                                and not ra.excluido
                          ;";

            var parametros = new
            {
                id
            };

            RegistroColetivoCompletoDto retorno = null;
            using (var registrosColetivos = await database.Conexao.QueryMultipleAsync(query, parametros))
            {
                {
                    retorno = registrosColetivos.ReadFirstOrDefault<RegistroColetivoCompletoDto>();
                    var ues = registrosColetivos.Read<UeRegistroColetivoDto>();
                    var anexos = registrosColetivos.Read<ArquivoAnexoRegistroColetivoDto>();
                    if (retorno.NaoEhNulo())
                    {
                        retorno.Ues = ues;
                        retorno.Anexos = anexos;
                    }
                }
            }

            return retorno;
        }
    }
}
