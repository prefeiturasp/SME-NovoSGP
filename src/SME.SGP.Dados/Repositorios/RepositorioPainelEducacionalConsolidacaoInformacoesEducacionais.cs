using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoInformacoesEducacionais : IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoInformacoesEducacionais(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }
        public async Task<IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto>> ObterDadosParaConsolidarInformacoesEducacionais(int anoLetivo, string[] codigosUe)
        {
            var sql = @"WITH idep AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                COUNT(CASE WHEN t1.etapa = 1 THEN 1 END) AS idep_anos_iniciais,
                                COUNT(CASE WHEN t1.etapa = 2 THEN 1 END) AS idep_anos_finais
                            FROM painel_educacional_consolidacao_idep t1
                            GROUP BY t1.ano_letivo, t1.codigo_dre, t1.codigo_ue
                        ),
                        ideb AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                COUNT(CASE WHEN t1.etapa = 1 THEN 1 END) AS ideb_anos_iniciais,
                                COUNT(CASE WHEN t1.etapa = 2 THEN 1 END) AS ideb_anos_finais,
                                COUNT(CASE WHEN t1.etapa = 3 THEN 1 END) AS ideb_ensino_medio
                            FROM painel_educacional_consolidacao_ideb t1
                            GROUP BY t1.ano_letivo, t1.codigo_dre, t1.codigo_ue
                        ),
                        frequencia AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                t1.percentual_frequencia AS percentual_frequencia_global
                            FROM painel_educacional_registro_frequencia_agrupamento_global t1
                        ),
                        pap AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                t1.total_turmas AS quantidade_turmas_pap,
                                t1.total_alunos_com_frequencia_inferior_limite AS percentual_frequencia_alunos_pap
                            FROM painel_educacional_consolidacao_pap_ue t1
                        ),
                        aprovacoes AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                t1.total_promocoes AS quantidade_promocoes,
                                t1.total_retencoes_ausencias AS quantidade_retencoes_frequencia,
                                t1.total_retencoes_notas AS quantidade_retencoes_nota
                            FROM painel_educacional_consolidacao_aprovacao_ue t1
                        ),
                        abandono AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                t1.quantidade_desistencias AS quantidade_alunos_desistentes_abandono
                            FROM painel_educacional_consolidacao_abandono_ue t1
                        ),
                        notas AS (
                            SELECT
                                t1.ano_letivo,
                                t1.codigo_dre,
                                t1.codigo_ue,
                                COALESCE(t1.quantidade_abaixo_media_portugues, 0) +
                                COALESCE(t1.quantidade_abaixo_media_matematica, 0) +
                                COALESCE(t1.quantidade_abaixo_media_ciencias, 0)
                                    AS quantidade_notas_abaixo_media,

                                COALESCE(t1.quantidade_acima_media_portugues, 0) +
                                COALESCE(t1.quantidade_acima_media_matematica, 0) +
                                COALESCE(t1.quantidade_acima_media_ciencias, 0)
                                    AS quantidade_notas_acima_media
                            FROM painel_educacional_consolidacao_nota_ue t1
                        )

                        SELECT 
                            COALESCE(idep.ano_letivo, ideb.ano_letivo, freq.ano_letivo, pap.ano_letivo, aband.ano_letivo, aprov.ano_letivo, notas.ano_letivo) AS anoLetivo,
                            COALESCE(idep.codigo_dre, ideb.codigo_dre, freq.codigo_dre, pap.codigo_dre, aband.codigo_dre, aprov.codigo_dre, notas.codigo_dre) AS codigoDre,
                            COALESCE(idep.codigo_ue, ideb.codigo_ue, freq.codigo_ue, pap.codigo_ue, aband.codigo_ue, aprov.codigo_ue, notas.codigo_ue) AS codigoUe,

                            -- IDEP
                            COALESCE(idep.idep_anos_iniciais, 0) AS idepAnosIniciais,
                            COALESCE(idep.idep_anos_finais, 0) AS idepAnosFinais,

                            -- IDEB
                            COALESCE(ideb.ideb_anos_iniciais, 0) AS idebAnosIniciais,
                            COALESCE(ideb.ideb_anos_finais, 0) AS idebAnosFinais,
                            COALESCE(ideb.ideb_ensino_medio, 0) AS idebEnsinoMedio,

                            -- FREQUÊNCIA GLOBAL
                            COALESCE(freq.percentual_frequencia_global, 0) AS percentualFrequenciaGlobal,

                            -- PAP
                            COALESCE(pap.quantidade_turmas_pap, 0) AS quantidadeTurmasPap,
                            COALESCE(pap.percentual_frequencia_alunos_pap, 0) AS percentualFrequenciaAlunosPap,

                            -- ABANDONO
                            COALESCE(aband.quantidade_alunos_desistentes_abandono, 0) AS quantidadeAlunosDesistentesAbandono,

                            -- APROVAÇÕES
                            COALESCE(aprov.quantidade_promocoes, 0) AS quantidadePromocoes,
                            COALESCE(aprov.quantidade_retencoes_frequencia, 0) AS quantidadeRetencoesFrequencia,
                            COALESCE(aprov.quantidade_retencoes_nota, 0) AS quantidadeRetencoesNota,

                            -- NOTAS
                            COALESCE(notas.quantidade_notas_abaixo_media, 0) AS quantidadeNotasAbaixoMedia,
                            COALESCE(notas.quantidade_notas_acima_media, 0) AS quantidadeNotasAcimaMedia
                        FROM idep
                        FULL JOIN ideb ON (idep.ano_letivo = ideb.ano_letivo AND idep.codigo_dre = ideb.codigo_dre AND idep.codigo_ue = ideb.codigo_ue)
                        FULL JOIN frequencia freq ON (idep.ano_letivo = freq.ano_letivo AND idep.codigo_dre = freq.codigo_dre AND idep.codigo_ue = freq.codigo_ue)
                        FULL JOIN pap ON (idep.ano_letivo = pap.ano_letivo AND idep.codigo_dre = pap.codigo_dre AND idep.codigo_ue = pap.codigo_ue)
                        FULL JOIN abandono aband ON (idep.ano_letivo = aband.ano_letivo AND idep.codigo_dre = aband.codigo_dre AND idep.codigo_ue = aband.codigo_ue)
                        FULL JOIN aprovacoes aprov ON (idep.ano_letivo = aprov.ano_letivo AND idep.codigo_dre = aprov.codigo_dre AND idep.codigo_ue = aprov.codigo_ue)
                        FULL JOIN notas ON (idep.ano_letivo = notas.ano_letivo AND idep.codigo_dre = notas.codigo_dre AND idep.codigo_ue = notas.codigo_ue)
                        WHERE COALESCE(
                            idep.codigo_ue,
                            ideb.codigo_ue,
                            freq.codigo_ue,
                            pap.codigo_ue,
                            aband.codigo_ue,
                            aprov.codigo_ue,
                            notas.codigo_ue
                        ) = any(@codigosUe)
                        and COALESCE(
                            idep.ano_letivo,
                            ideb.ano_letivo,
                            freq.ano_letivo,
                            pap.ano_letivo,
                            aband.ano_letivo,
                            aprov.ano_letivo,
                            notas.ano_letivo
                        ) = @anoLetivo;";

            return await database.QueryAsync<DadosParaConsolidarInformacoesEducacionaisDto>(sql, new { codigosUe, anoLetivo });
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoInformacoesEducacionais> indicadores)
        {
                await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
                await conn.OpenAsync();

                await using var writer = conn.BeginBinaryImport(@"
                    COPY painel_educacional_consolidacao_informacoes_educacionais
                        (ano_letivo, 
                         codigo_dre, 
                         codigo_ue,
                         ue,
                         idep_anos_iniciais,
                         idep_anos_finais,
                         ideb_anos_iniciais,
                         ideb_anos_finais,
                         ideb_ensino_medio,
                         percentual_frequencia_global,
                         quantidade_turmas_pap,
                         percentual_frequencia_alunos_pap,
                         quantidade_alunos_desistentes_abandono,
                         quantidade_promocoes,
                         quantidade_retencoes_frequencia,
                         quantidade_retencoes_nota,
                         quantidade_notas_abaixo_media,
                         quantidade_notas_acima_media,
                         criado_em)
                    FROM STDIN (FORMAT BINARY)
                ");

                foreach (var item in indicadores)
                {
                    await writer.StartRowAsync();
                    await writer.WriteAsync(item.AnoLetivo, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.CodigoDre, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.CodigoUe, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.Ue, NpgsqlDbType.Varchar);
                    await writer.WriteAsync(item.IdepAnosIniciais, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.IdepAnosFinais, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.IdebAnosIniciais, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.IdebAnosFinais, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.IdebEnsinoMedio, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.PercentualFrequenciaGlobal, NpgsqlDbType.Numeric);
                    await writer.WriteAsync(item.QuantidadeTurmasPap, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.PercentualFrequenciaAlunosPap, NpgsqlDbType.Numeric);
                    await writer.WriteAsync(item.QuantidadeAlunosDesistentesAbandono, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.QuantidadePromocoes, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.QuantidadeRetencoesFrequencia, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.QuantidadeRetencoesNota, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.QuantidadeNotasAbaixoMedia, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.QuantidadeNotasAcimaMedia, NpgsqlDbType.Integer);
                    await writer.WriteAsync(item.CriadoEm, NpgsqlDbType.TimestampTz);
                }

                await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            var sql = @"DELETE FROM painel_educacional_consolidacao_informacoes_educacionais;
                        SELECT setval(
                            pg_get_serial_sequence('painel_educacional_consolidacao_informacoes_educacionais', 'id'),
                            (SELECT MAX(id) FROM painel_educacional_consolidacao_informacoes_educacionais)
                        );
                        ";

            await database.ExecuteAsync(sql);
        }
    }
}
