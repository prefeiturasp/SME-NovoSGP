using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalPap : IRepositorioPainelEducacionalPap
    {
        private readonly ISgpContext database;
        private readonly IConfiguration configuration;
        private const string COLUNAS_SELECT = @"tipo_pap tipoPap,
                                                total_turmas quantidadeTurmas,
                                                total_alunos quantidadeEstudantes,
                                                total_alunos_com_frequencia_inferior_limite quantidadeEstudantesComFrequenciaInferiorLimite,
                                                total_alunos_dificuldade_top_1 quantidadeEstudantesDificuldadeTop1,
                                                total_alunos_dificuldade_top_2 quantidadeEstudantesDificuldadeTop2,
                                                total_alunos_dificuldade_outras outrasDificuldadesAprendizagem,
                                                nome_dificuldade_top_1 nomeDificuldadeTop1,
                                                nome_dificuldade_top_2 nomeDificuldadeTop2";

        public RepositorioPainelEducacionalPap(IConfiguration configuration, ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public async Task ExcluirConsolidacaoApartirDoAno(int ano)
        {
            const string query = @"delete from painel_educacional_consolidacao_pap_dre where ano_letivo >= @ano;
                                   delete from painel_educacional_consolidacao_pap_ue where ano_letivo >= @ano;
                                   delete from painel_educacional_consolidacao_pap_sme where ano_letivo >= @ano;";
            await database.ExecuteAsync(query, new { ano });
        }

        public async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterConsolidacoesDrePorAno(int ano, string codigoDre)
        {
            var query = $@"select {COLUNAS_SELECT} 
                             from painel_educacional_consolidacao_pap_dre 
                            where ano_letivo = @ano and codigo_dre = @codigoDre";
            return await database.QueryAsync<PainelEducacionalInformacoesPapDto>(query, new { ano, codigoDre });
        }

        public async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterConsolidacoesSmePorAno(int ano)
        {
            var query = $@"select {COLUNAS_SELECT} 
                             from painel_educacional_consolidacao_pap_dre 
                            where ano_letivo = @ano";
            return await database.QueryAsync<PainelEducacionalInformacoesPapDto>(query, new { ano });
        }

        public async Task<IEnumerable<PainelEducacionalInformacoesPapDto>> ObterConsolidacoesUePorAno(int ano, string codigoDre, string codigoUe)
        {
            var query = $@"select {COLUNAS_SELECT} 
                             from painel_educacional_consolidacao_pap_dre 
                            where ano_letivo = @ano and codigo_dre = @codigoDre and codigo_ue = @codigoUe";
            return await database.QueryAsync<PainelEducacionalInformacoesPapDto>(query, new { ano, codigoDre, codigoUe });
        }

        public async Task<int?> ObterUltimoAnoConsolidado()
        {
            const string query = @"select max(ano_letivo) from painel_educacional_consolidacao_pap_sme";
            return await database.QueryFirstOrDefaultAsync<int?>(query);
        }

        public async Task SalvarConsolidacaoDre(IList<PainelEducacionalConsolidacaoPapDre> consolidacao)
        {
            // Fazer bulk insert
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            // Iniciar o COPY em modo bin�rio
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_pap_dre 
                     (ano_letivo,tipo_pap,dre_codigo,total_turmas,total_alunos,total_alunos_com_frequencia_inferior_limite,total_alunos_dificuldade_top_1,total_alunos_dificuldade_top_2,total_alunos_dificuldade_outras,nome_dificuldade_top_1,nome_dificuldade_top_2,criado_em) 
                FROM STDIN (FORMAT BINARY)");

            foreach (var item in consolidacao)
            {
                writer.StartRow();
                writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TipoPap, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.TotalTurmas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.NomeDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.NomeDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }

        public async Task SalvarConsolidacaoSme(IList<PainelEducacionalConsolidacaoPapSme> consolidacao)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_pap_sme 
                     (ano_letivo,tipo_pap,total_turmas,total_alunos,total_alunos_com_frequencia_inferior_limite,total_alunos_dificuldade_top_1,total_alunos_dificuldade_top_2,total_alunos_dificuldade_outras,nome_dificuldade_top_1,nome_dificuldade_top_2,criado_em) 
                FROM STDIN (FORMAT BINARY)");
            foreach (var item in consolidacao)
            {
                writer.StartRow();
                writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TipoPap, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalTurmas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.NomeDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.NomeDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }
            await writer.CompleteAsync();
        }

        public async Task SalvarConsolidacaoUe(IList<PainelEducacionalConsolidacaoPapUe> consolidacao)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();
            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_pap_ue 
                     (ano_letivo,tipo_pap,dre_codigo,ue_codigo,total_turmas,total_alunos,total_alunos_com_frequencia_inferior_limite,total_alunos_dificuldade_top_1,total_alunos_dificuldade_top_2,total_alunos_dificuldade_outras,nome_dificuldade_top_1,nome_dificuldade_top_2,criado_em) 
                FROM STDIN (FORMAT BINARY)");
            foreach (var item in consolidacao)
            {
                writer.StartRow();
                writer.Write(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TipoPap, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.TotalTurmas, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunos, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosComFrequenciaInferiorLimite, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.TotalAlunosDificuldadeOutras, NpgsqlTypes.NpgsqlDbType.Integer);
                writer.Write(item.NomeDificuldadeTop1, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.NomeDificuldadeTop2, NpgsqlTypes.NpgsqlDbType.Varchar);
                writer.Write(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }
            await writer.CompleteAsync();
        }
    }
}