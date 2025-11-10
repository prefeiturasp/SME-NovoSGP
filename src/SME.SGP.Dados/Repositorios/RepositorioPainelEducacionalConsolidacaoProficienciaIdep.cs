using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoProficienciaIdep : IRepositorioPainelEducacionalConsolidacaoProficienciaIdep
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;
        public RepositorioPainelEducacionalConsolidacaoProficienciaIdep(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task LimparConsolidacaoPorAnoAsync(int anoLetivo)
        {
            const string query = @"delete from painel_educacional_consolidacao_proficiencia_idep_ue where ano_letivo = @anoLetivo;
                                 -- sincronizar a sequence da tabela
                                 SELECT setval(
                                     pg_get_serial_sequence('painel_educacional_consolidacao_proficiencia_idep_ue', 'id'),
                                     (SELECT MAX(id) FROM painel_educacional_consolidacao_proficiencia_idep_ue)
                                 );";
            await database.ExecuteAsync(query, new { anoLetivo });
        }

        public async Task SalvarConsolidacaoAsync(IList<PainelEducacionalConsolidacaoProficienciaIdepUe> indicadores)
        {
            const string comandoCopy = @"
                COPY painel_educacional_consolidacao_proficiencia_idep_ue 
                     (ano_letivo, codigo_ue, serie_ano, componente_curricular_id, nota, proficiencia, boletim, criado_em)
                FROM STDIN (FORMAT BINARY)";

            await using var conexao = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conexao.OpenAsync();
            await using var writer = conexao.BeginBinaryImport(comandoCopy);

            foreach (var item in indicadores)
            {
                writer.StartRow();
                writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write((short)item.SerieAno, NpgsqlTypes.NpgsqlDbType.Smallint);

                if (item.ComponenteCurricular.HasValue) writer.Write((short)item.ComponenteCurricular, NpgsqlTypes.NpgsqlDbType.Smallint);
                else writer.WriteNull();

                if (item.Nota.HasValue) writer.Write(item.Nota.Value, NpgsqlTypes.NpgsqlDbType.Numeric);
                else writer.WriteNull();

                if (item.Proficiencia.HasValue) writer.Write(item.Proficiencia.Value, NpgsqlTypes.NpgsqlDbType.Numeric);
                else writer.WriteNull();

                if (!string.IsNullOrWhiteSpace(item.Boletim)) writer.Write(item.Boletim, NpgsqlTypes.NpgsqlDbType.Varchar);
                else writer.WriteNull();

                writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.TimestampTz);
            }

            await writer.CompleteAsync();
        }
    }
}