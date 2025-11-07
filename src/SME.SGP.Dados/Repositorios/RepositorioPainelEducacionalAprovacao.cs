using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalAprovacao : IRepositorioPainelEducacionalAprovacao
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalAprovacao(ISgpContext database, IConfiguration configuration)
        {
            this.database = database;
            this.configuration = configuration;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoAprovacao> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_aprovacao 
                    (codigo_dre, serie_ano, modalidade, total_promocoes, total_retencoes_ausencias, total_retencoes_notas, ano_letivo, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.SerieAno, NpgsqlDbType.Varchar);
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
            var sql = @"TRUNCATE painel_educacional_consolidacao_aprovacao";

            await database.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<PainelEducacionalAprovacao>> ObterAprovacao(int anoLetivo, string codigoDre, string codigoUe)
        {
            var sql = $@"SELECT * FROM painel_educacional_registro_frequencia_agrupamento_global
                        WHERE ano_letivo = @anoLetivo";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                sql += " AND codigo_dre = @codigoDre";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                sql += " AND codigo_ue = @codigoUe";

            return await database.QueryAsync<PainelEducacionalAprovacao>(sql, new { anoLetivo, codigoDre, codigoUe });
        }

        public async Task<IEnumerable<DadosParaConsolidarAprovacao>> ObterIndicadores(long[] turmasIds)
        {
            var sql = @"select 
                                cccat.turma_id as TurmaId,
                                cccat.aluno_codigo as CodigoAluno,        
                                cccat.parecer_conclusivo_id as ParecerConclusivoId,
                                ccp.nome as ParecerDescricao
                        from consolidado_conselho_classe_aluno_turma cccat
                        inner join conselho_classe_parecer ccp  on cccat.parecer_conclusivo_id = ccp.id
                        where cccat.turma_id = any(@turmasIds)
                        and cccat.excluido = false
                        and cccat.parecer_conclusivo_id is not null 
                        and cccat.status = 2"
            ;

            return await database.QueryAsync<DadosParaConsolidarAprovacao>(sql, new { turmasIds });
        }
    }
}