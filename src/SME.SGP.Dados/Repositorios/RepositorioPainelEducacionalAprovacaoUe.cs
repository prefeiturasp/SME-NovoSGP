using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Dapper;
using System.Text;



namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAprovacaoUe : IRepositorioPainelEducacionalAprovacaoUe
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalAprovacaoUe(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_aprovacao_ue 
                    (codigo_dre, codigo_ue, turma, modalidade, total_promocoes, total_retencoes_ausencias, total_retencoes_notas, ano_letivo, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Turma, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.Modalidade, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.TotalPromocoes, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TotalRetencoesAusencias, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.TotalRetencoesNotas, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            var sql = @"TRUNCATE painel_educacional_consolidacao_aprovacao_ue";

            await database.ExecuteAsync(sql);
        }
        public async Task<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>> ObterAprovacao(
      int anoLetivo,
      string codigoUe,
      int modalidadeId,
      Paginacao paginacao)
        {
            var query = MontarQueryCompleta(paginacao);

            var parametros = new
            {
                anoLetivo,
                codigoUe,
                modalidadeId
            };

            var retorno = new PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>();

            using (var multi = await database.QueryMultipleAsync(query, parametros))
            {
                retorno.Items = multi.Read<PainelEducacionalAprovacaoUeDto>();
                retorno.TotalRegistros = paginacao.QuantidadeRegistros <= 1 ? 1 : multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
               ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
               : 1;

            return retorno;
        }

        private static string MontarQueryCompleta(Paginacao paginacao)
        {
            var sql = new StringBuilder();

            MontarQueryConsulta(sql, paginacao, contador: false);
            sql.AppendLine(";");
            MontarQueryConsulta(sql, paginacao, contador: true);

            return sql.ToString();
        }

        private static void MontarQueryConsulta(StringBuilder sql, Paginacao paginacao, bool contador)
        {
            if (contador)
            {
                sql.AppendLine("SELECT COUNT(*) ");
            }
            else
            {
                sql.AppendLine(@"SELECT 
                            codigo_dre AS CodigoDre,
                            codigo_ue AS CodigoUe,
                            turma AS Turma,
                            modalidade AS Modalidade,
                            total_promocoes AS TotalPromocoes,
                            total_retencoes_ausencias AS TotalRetencoesAusencias,
                            total_retencoes_notas AS TotalRetencoesNotas,
                            ano_letivo AS AnoLetivo");
            }

            sql.AppendLine(" FROM painel_educacional_consolidacao_aprovacao_ue ");
            sql.AppendLine(" WHERE ano_letivo = @anoLetivo AND modalidade_codigo = @modalidadeId AND codigo_ue = @codigoUe ");

            if (!contador)
            {
                sql.AppendLine(" ORDER BY codigo_ue ");

                if (paginacao.QuantidadeRegistros > 0)
                    sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");
            }
        }

    }
}
