using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoIndicadoresPap : IRepositorioPainelEducacionalConsolidacaoIndicadoresPap
    {
        private readonly ISgpContext database;
        private readonly IConfiguration configuration;

        public RepositorioPainelEducacionalConsolidacaoIndicadoresPap(IConfiguration configuration, ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public async Task<int?> ObterUltimoAnoConsolidado()
        {
            const string query = @"select max(ano_letivo) from painel_educacional_consolidacao_pap_sme";
            return await database.QueryFirstOrDefaultAsync<int?>(query);
        }

        public async Task<IEnumerable<ContagemDificuldadeIndicadoresPapPorTipoDto>> ObterContagemDificuldadesConsolidadaGeral(IEnumerable<DadosMatriculaAlunoTipoPapDto> dadosMatriculaAlunos, CancellationToken cancellationToken)
        {
            if (dadosMatriculaAlunos == null || !dadosMatriculaAlunos.Any())
            {
                return Enumerable.Empty<ContagemDificuldadeIndicadoresPapPorTipoDto>();
            }
            var valuesClause = new StringBuilder();
            var parameters = new DynamicParameters();
            parameters.Add("qtdIndicadores", PainelEducacionalConstants.QTD_INDICADORES_PAP);

            int index = 0;
            foreach (var filtro in dadosMatriculaAlunos.Select(a => new { codigoTurma = a.CodigoTurma.ToString(), tipoPap = (int)a.TipoPap }).Distinct())
            {
                var pCodigo = $"@p{index++}";
                var pTipo = $"@p{index++}";

                valuesClause.Append($"({pCodigo}, {pTipo}),");

                parameters.Add(pCodigo, filtro.codigoTurma);
                parameters.Add(pTipo, filtro.tipoPap);
            }

            var testeValues = dadosMatriculaAlunos.Select(d => $"('{d.CodigoTurma}',{(int)d.TipoPap})").Distinct().ToList();
            var jointeste = string.Join(",\n", testeValues);

            valuesClause.Length--;
            var query = @$"
           WITH RECURSIVE filtro_turmas (codigoTurma, tipoPap) AS (
                VALUES {valuesClause}
            ),
            ultimo_relatorio AS (
                SELECT
                    relPapTurma.id,
                    filtroTurma.tipoPap,
                    turma.ano_letivo as anoLetivo,
                    dre.dre_id as codigoDre,
                    ue.ue_id as codigoUe,
                    row_number() OVER (PARTITION BY filtroTurma.tipoPap, relPapTurma.turma_id ORDER BY relPapTurma.id DESC) as indexLinha
                FROM relatorio_periodico_pap_turma relPapTurma
                     inner join turma on turma.id = relPapTurma.turma_id 
                     inner join filtro_turmas filtroTurma on filtroTurma.codigoTurma = turma.turma_id
                     inner join ue on ue.id = turma.ue_id
                     inner join dre on dre.id = ue.dre_id 
                WHERE NOT relPapTurma.excluido
            ),
            respostas_dificuldades AS MATERIALIZED (
                SELECT
                    relPapResposta.resposta_id as respostaId,
                    opcaoResposta.nome as nomeDificuldade,
                    ultimoRelatorio.tipoPap,
                    ultimoRelatorio.anoLetivo,
                    ultimoRelatorio.codigoDre,
                    ultimoRelatorio.codigoUe
                FROM ultimo_relatorio ultimoRelatorio
                INNER JOIN relatorio_periodico_pap_aluno relPapAluno ON relPapAluno.relatorio_periodico_pap_turma_id = ultimoRelatorio.id
                INNER JOIN relatorio_periodico_pap_secao relPapSecao ON relPapSecao.relatorio_periodico_pap_aluno_id = relPapAluno.id
                INNER JOIN relatorio_periodico_pap_questao relPapQuestao ON relPapQuestao.relatorio_periodico_pap_secao_id = relPapSecao.id
                INNER JOIN questao ON questao.id = relPapQuestao.questao_id
                INNER JOIN relatorio_periodico_pap_resposta relPapResposta ON relPapResposta.relatorio_periodico_pap_questao_id = relPapQuestao.id
                INNER JOIN opcao_resposta opcaoResposta ON opcaoResposta.id = relPapResposta.resposta_id
                WHERE ultimoRelatorio.indexLinha = 1 
                  AND NOT relPapAluno.excluido 
                  AND NOT relPapSecao.excluido 
                  AND NOT relPapQuestao.excluido 
                  AND NOT relPapResposta.excluido 
                  AND NOT questao.excluido 
                  AND NOT opcaoResposta.excluido
                  AND relPapResposta.resposta_id IS NOT NULL
                  AND questao.nome_componente = 'DIFIC_APRESENTADAS'
            ),
            uniao_visoes AS (
                -- Visão UE
                SELECT
                    'UE' as abrangencia,
                    respostasDificuldades.tipoPap,
                    respostasDificuldades.codigoDre,
                    respostasDificuldades.codigoUe,
                    respostasDificuldades.anoLetivo,
                    CASE WHEN td.respostaId IS NOT NULL THEN respostasDificuldades.respostaId ELSE {PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP} END AS respostaId,
                    CASE WHEN td.respostaId IS NOT NULL THEN respostasDificuldades.nomeDificuldade ELSE '{PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP}' END AS nomeDificuldade
                FROM respostas_dificuldades respostasDificuldades
                LEFT JOIN (
                    SELECT respostaId, codigoUe, anoLetivo 
                      FROM (SELECT respostaId, codigoUe, anoLetivo, 
                                   RANK() OVER (PARTITION BY codigoUe, anoLetivo ORDER BY COUNT(*) DESC) as ranking
                              FROM respostas_dificuldades
                             GROUP BY respostaId, codigoUe, anoLetivo) classificados
                      WHERE ranking <= @qtdIndicadores) td ON respostasDificuldades.respostaId = td.respostaId AND 
                                                              respostasDificuldades.codigoUe = td.codigoUe AND respostasDificuldades.anoLetivo = td.anoLetivo

                UNION ALL

                -- Visão DRE
                SELECT
                    'DRE' as abrangencia,
                    respostasDificuldades.tipoPap,
                    respostasDificuldades.codigoDre,
                    NULL as codigoUe,
                    respostasDificuldades.anoLetivo,
                    CASE WHEN td.respostaId IS NOT NULL THEN respostasDificuldades.respostaId ELSE {PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP} END AS respostaId,
                    CASE WHEN td.respostaId IS NOT NULL THEN respostasDificuldades.nomeDificuldade ELSE '{PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP}' END AS nomeDificuldade
                FROM respostas_dificuldades respostasDificuldades
                LEFT JOIN (
                    SELECT respostaId, codigoDre, anoLetivo 
                      FROM (SELECT respostaId, codigoDre, anoLetivo, 
                                   RANK() OVER (PARTITION BY codigoDre, anoLetivo ORDER BY COUNT(*) DESC) as ranking
                              FROM respostas_dificuldades
                             GROUP BY respostaId, codigoDre, anoLetivo) classificados
                    WHERE ranking <= @qtdIndicadores) td ON respostasDificuldades.respostaId = td.respostaId AND 
                                                            respostasDificuldades.codigoDre = td.codigoDre AND 
                                                            respostasDificuldades.anoLetivo = td.anoLetivo

                UNION ALL

                -- Visão SME
                SELECT
                    'SME' as abrangencia,
                    respostasDificuldades.tipoPap,
                    NULL as codigoDre,
                    NULL as codigoUe,
                    respostasDificuldades.anoLetivo,
                    CASE WHEN td.respostaId IS NOT NULL THEN respostasDificuldades.respostaId ELSE {PainelEducacionalConstants.ID_OUTRAS_DIFICULDADES_PAP} END AS respostaId,
                    CASE WHEN td.respostaId IS NOT NULL THEN respostasDificuldades.nomeDificuldade ELSE '{PainelEducacionalConstants.NOME_OUTRAS_DIFICULDADES_PAP}' END AS nomeDificuldade
                FROM respostas_dificuldades respostasDificuldades
                LEFT JOIN (
                    SELECT respostaId, anoLetivo 
                      FROM (SELECT respostaId, anoLetivo, 
                                   RANK() OVER (PARTITION BY anoLetivo ORDER BY COUNT(*) DESC) as ranking
                              FROM respostas_dificuldades
                             GROUP BY respostaId, anoLetivo) classificados
                     WHERE ranking <= @qtdIndicadores) td ON respostasDificuldades.respostaId = td.respostaId AND 
                                                             respostasDificuldades.anoLetivo = td.anoLetivo
            )
            SELECT
                abrangencia,
                tipoPap,
                codigoDre,
                codigoUe,
                anoLetivo,
                respostaId,
                nomeDificuldade,
                COUNT(*) as quantidade
            FROM uniao_visoes
            GROUP BY
                abrangencia,
                tipoPap,
                codigoDre,
                codigoUe,
                anoLetivo,
                respostaId,
                nomeDificuldade;";

            return await database.Conexao.QueryAsync<ContagemDificuldadeIndicadoresPapPorTipoDto>(query,
                parameters,
                commandTimeout: 600);
        }

        public async Task ExcluirConsolidacaoPorAno(int ano)
        {
            const string query = @"delete from painel_educacional_consolidacao_pap_dre where ano_letivo = @ano;
                                   delete from painel_educacional_consolidacao_pap_ue where ano_letivo = @ano;
                                   delete from painel_educacional_consolidacao_pap_sme where ano_letivo = @ano;
                                -- sincronizar a sequence das tabelas
                                SELECT setval(
                                    pg_get_serial_sequence('painel_educacional_consolidacao_pap_dre', 'id'),
                                    (SELECT MAX(id) FROM painel_educacional_consolidacao_pap_dre)
                                );
                                SELECT setval(
                                    pg_get_serial_sequence('painel_educacional_consolidacao_pap_ue', 'id'),
                                    (SELECT MAX(id) FROM painel_educacional_consolidacao_pap_ue)
                                );
                                SELECT setval(
                                    pg_get_serial_sequence('painel_educacional_consolidacao_pap_sme', 'id'),
                                    (SELECT MAX(id) FROM painel_educacional_consolidacao_pap_sme)
                                );";

            await database.ExecuteAsync(query, new { ano });
        }

        public async Task SalvarConsolidacaoDre(IList<PainelEducacionalConsolidacaoPapDre> consolidacao)
        {
            //await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            //await conn.OpenAsync();

            //// Iniciar o COPY em modo bin�rio
            //await using var writer = conn.BeginBinaryImport(@"
            //    COPY painel_educacional_consolidacao_pap_dre 
            //         (ano_letivo,tipo_pap,codigo_dre,total_turmas,total_alunos,total_alunos_com_frequencia_inferior_limite,total_alunos_dificuldade_top_1,total_alunos_dificuldade_top_2,total_alunos_dificuldade_outras,nome_dificuldade_top_1,nome_dificuldade_top_2,criado_em) 
            //    FROM STDIN (FORMAT BINARY)");

            //foreach (var item in consolidacao)
            //{
            //    writer.StartRow();
            //    writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write((int)item.TipoPap, NpgsqlTypes.NpgsqlDbType.Smallint);
            //    writer.Write(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.TotalTurmas, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.NomeDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.NomeDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            //}

            //await writer.CompleteAsync();

            const string copyCommand = @"
                COPY painel_educacional_consolidacao_pap_dre
                    (ano_letivo, tipo_pap, codigo_dre, total_turmas, total_alunos, total_alunos_com_frequencia_inferior_limite, total_alunos_dificuldade_top_1, total_alunos_dificuldade_top_2, total_alunos_dificuldade_outras, nome_dificuldade_top_1, nome_dificuldade_top_2, criado_em)
                FROM STDIN (FORMAT BINARY)";

            await ExecutarBulkCopy(consolidacao, copyCommand, (writer, item) => EscreverDadosBase(writer, item));
        }

        public async Task SalvarConsolidacaoSme(IList<PainelEducacionalConsolidacaoPapSme> consolidacao)
        {
            //await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            //await conn.OpenAsync();

            //await using var writer = conn.BeginBinaryImport(@"
            //    COPY painel_educacional_consolidacao_pap_sme 
            //         (ano_letivo,tipo_pap,total_turmas,total_alunos,total_alunos_com_frequencia_inferior_limite,total_alunos_dificuldade_top_1,total_alunos_dificuldade_top_2,total_alunos_dificuldade_outras,nome_dificuldade_top_1,nome_dificuldade_top_2,criado_em) 
            //    FROM STDIN (FORMAT BINARY)");
            //foreach (var item in consolidacao)
            //{
            //    writer.StartRow();
            //    writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write((int)item.TipoPap, NpgsqlTypes.NpgsqlDbType.Smallint);
            //    writer.Write(item.TotalTurmas, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.NomeDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.NomeDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            //}
            //await writer.CompleteAsync();

            const string copyCommand = @"
                COPY painel_educacional_consolidacao_pap_sme
                    (ano_letivo, tipo_pap, total_turmas, total_alunos, total_alunos_com_frequencia_inferior_limite, total_alunos_dificuldade_top_1, total_alunos_dificuldade_top_2, total_alunos_dificuldade_outras, nome_dificuldade_top_1, nome_dificuldade_top_2, criado_em)
                FROM STDIN (FORMAT BINARY)";

            await ExecutarBulkCopy(consolidacao, copyCommand, (writer, item) => EscreverDadosBase(writer, item));
        }

        public async Task SalvarConsolidacaoUe(IList<PainelEducacionalConsolidacaoPapUe> consolidacao)
        {
            //await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            //await conn.OpenAsync();
            //await using var writer = conn.BeginBinaryImport(@"
            //    COPY painel_educacional_consolidacao_pap_ue 
            //         (ano_letivo,tipo_pap,codigo_dre,codigo_ue,total_turmas,total_alunos,total_alunos_com_frequencia_inferior_limite,total_alunos_dificuldade_top_1,total_alunos_dificuldade_top_2,total_alunos_dificuldade_outras,nome_dificuldade_top_1,nome_dificuldade_top_2,criado_em) 
            //    FROM STDIN (FORMAT BINARY)");
            //foreach (var item in consolidacao)
            //{
            //    writer.StartRow();
            //    writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write((int)item.TipoPap, NpgsqlTypes.NpgsqlDbType.Smallint);
            //    writer.Write(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.TotalTurmas, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlTypes.NpgsqlDbType.Integer);
            //    writer.Write(item.NomeDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.NomeDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Varchar);
            //    writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            //}
            //await writer.CompleteAsync();


            const string copyCommand = @"
                COPY painel_educacional_consolidacao_pap_ue
                    (ano_letivo, tipo_pap, codigo_dre, codigo_ue, total_turmas, total_alunos, total_alunos_com_frequencia_inferior_limite, total_alunos_dificuldade_top_1, total_alunos_dificuldade_top_2, total_alunos_dificuldade_outras, nome_dificuldade_top_1, nome_dificuldade_top_2, criado_em)
                FROM STDIN (FORMAT BINARY)";

            await ExecutarBulkCopy(consolidacao, copyCommand, (writer, item) => EscreverDadosBase(writer, item));
        }

        private async Task ExecutarBulkCopy<T>(IList<T> colecao, string comandoCopy, Action<NpgsqlBinaryImporter, T> escreverCampos) where T : PainelEducacionalConsolidacaoPapBase
        {
            await using var conexao = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conexao.OpenAsync();
            await using var writer = conexao.BeginBinaryImport(comandoCopy);

            foreach (var item in colecao)
            {
                writer.StartRow();
                escreverCampos(writer, item);
            }

            await writer.CompleteAsync();
        }

        private void EscreverDadosBase(NpgsqlBinaryImporter writer, PainelEducacionalConsolidacaoPapBase item)
        {
            writer.Write(item.AnoLetivo, NpgsqlDbType.Integer);
            writer.Write((int)item.TipoPap, NpgsqlDbType.Smallint);

            if (item is PainelEducacionalConsolidacaoPapDre dreItem)
                writer.Write(dreItem.CodigoDre, NpgsqlDbType.Varchar);
            if (item is PainelEducacionalConsolidacaoPapUe ueItem)
            {
                writer.Write(ueItem.CodigoDre, NpgsqlDbType.Varchar);
                writer.Write(ueItem.CodigoUe, NpgsqlDbType.Varchar);
            }

            writer.Write(item.TotalTurmas, NpgsqlDbType.Integer);
            writer.Write(item.TotalAlunos, NpgsqlDbType.Integer);
            writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlDbType.Integer);
            writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlDbType.Integer);
            writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlDbType.Integer);
            writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlDbType.Integer);
            writer.Write(item.NomeDificuldadeTop1, NpgsqlDbType.Varchar);
            writer.Write(item.NomeDificuldadeTop2, NpgsqlDbType.Varchar);
            writer.Write(item.CriadoEm, NpgsqlDbType.Timestamp);
        }
    }
}