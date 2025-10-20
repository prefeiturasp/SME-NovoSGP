using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoNota : IRepositorioPainelEducacionalConsolidacaoNota
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;
        public RepositorioPainelEducacionalConsolidacaoNota(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task LimparConsolidacaoAsync(int anoLetivo)
        {
            const string query = @"delete from painel_educacional_consolidacao_nota where ano_letivo >= @anoLetivo;
                                   delete from painel_educacional_consolidacao_nota_ue where ano_letivo >= @anoLetivo;
                                -- sincronizar a sequence das tabelas
                                SELECT setval(
                                    pg_get_serial_sequence('painel_educacional_consolidacao_nota_ue', 'id'),
                                    (SELECT MAX(id) FROM painel_educacional_consolidacao_nota_ue)
                                );
                                SELECT setval(
                                    pg_get_serial_sequence('painel_educacional_consolidacao_nota', 'id'),
                                    (SELECT MAX(id) FROM painel_educacional_consolidacao_nota)
                                );";

            await database.ExecuteAsync(query, new { anoLetivo });
        }

        public async Task SalvarConsolidacaoAsync(IList<PainelEducacionalConsolidacaoNota> indicadores)
        {
            const string comandoCopy = @"
                COPY painel_educacional_consolidacao_nota 
                    (ano_letivo, criado_em, modalidade, bimestre, quantidade_abaixo_media_portugues, quantidade_acima_media_portugues,
                     quantidade_abaixo_media_matematica, quantidade_acima_media_matematica,
                     quantidade_abaixo_media_ciencias, quantidade_acima_media_ciencias, codigo_dre, ano_turma)
                FROM STDIN (FORMAT BINARY)";

            await ExecutarBulkCopy(indicadores, comandoCopy, (writer, item) => EscreverDadosBase(writer, item));
        }

        public async Task SalvarConsolidacaoUeAsync(IList<PainelEducacionalConsolidacaoNotaUe> indicadores)
        {
            const string comandoCopy = @"
                COPY painel_educacional_consolidacao_nota_ue 
                    (ano_letivo, criado_em, modalidade, bimestre, quantidade_abaixo_media_portugues, quantidade_acima_media_portugues,
                     quantidade_abaixo_media_matematica, quantidade_acima_media_matematica,
                     quantidade_abaixo_media_ciencias, quantidade_acima_media_ciencias, codigo_dre, codigo_ue, serie_turma)
                FROM STDIN (FORMAT BINARY)";
            await ExecutarBulkCopy(indicadores, comandoCopy, (writer, item) => EscreverDadosBase(writer, item));
        }

        private async Task ExecutarBulkCopy<T>(IList<T> colecao, string comandoCopy, Action<NpgsqlBinaryImporter, T> escreverCampos) where T : PainelEducacionalConsolidacaoNotaBase
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

        private static void EscreverDadosBase(NpgsqlBinaryImporter writer, PainelEducacionalConsolidacaoNotaBase item)
        {
            writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.TimestampTz);
            writer.Write((int)item.Modalidade, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.Bimestre, NpgsqlTypes.NpgsqlDbType.Smallint);
            writer.Write(item.QuantidadeAbaixoMediaPortugues, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.QuantidadeAcimaMediaPortugues, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.QuantidadeAbaixoMediaMatematica, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.QuantidadeAcimaMediaMatematica, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.QuantidadeAbaixoMediaCiencias, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.QuantidadeAcimaMediaCiencias, NpgsqlTypes.NpgsqlDbType.Integer);
            writer.Write(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);

            if (item is PainelEducacionalConsolidacaoNota dre)
            {
                writer.Write(dre.AnoTurma, NpgsqlTypes.NpgsqlDbType.Char);
            }
            else if (item is PainelEducacionalConsolidacaoNotaUe ue)
            {
                writer.Write(ue.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(ue.SerieTurma, NpgsqlTypes.NpgsqlDbType.Varchar);
            }
        }
    }
}
