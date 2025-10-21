using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalReclassificacao : IRepositorioPainelEducacionalReclassificacao
    {
        private readonly ISgpContext database;
        private readonly IConfiguration configuration;
        public RepositorioPainelEducacionalReclassificacao(ISgpContext database, IConfiguration configuration)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
            this.configuration = configuration;
        }

        public async Task ExcluirReclassificacaoAnual(int anoLetivo)
        {
            const string comando = @"delete from public.painel_educacional_consolidacao_reclassificacao where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteAsync(comando, new { anoLetivo });
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalReclassificacao> registros)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_reclassificacao
                    (codigo_dre, codigo_ue, ano_letivo, modalidade_turma, ano_turma, quantidade_alunos_reclassificados, criado_em)
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var r in registros)
            {
                writer.StartRow();
                writer.Write(r.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write((int)r.ModalidadeTurma, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.AnoTurma, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(r.QuantidadeAlunosReclassificados, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(r.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }
    }
}
