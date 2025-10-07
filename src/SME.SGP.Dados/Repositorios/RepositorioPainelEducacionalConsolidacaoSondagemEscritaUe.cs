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
    public class RepositorioPainelEducacionalConsolidacaoSondagemEscritaUe : IRepositorioPainelEducacionalConsolidacaoSondagemEscritaUe
    {
        private readonly IConfiguration configuration;
        private readonly ISgpContext database;

        public RepositorioPainelEducacionalConsolidacaoSondagemEscritaUe(IConfiguration configuration, ISgpContext database)
        {
            this.configuration = configuration;
            this.database = database;
        }

        public async Task BulkInsertAsync(IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe> indicadores)
        {
            await using var conn = new NpgsqlConnection(configuration.GetConnectionString("SGP_Postgres"));
            await conn.OpenAsync();

            await using var writer = conn.BeginBinaryImport(@"
                COPY painel_educacional_consolidacao_sondagem_escrita_ue 
                    (codigo_dre, codigo_ue, pre_silabico, silabico_sem_valor, silabico_com_valor, silabico_alfabetico, alfabetico, sem_preenchimento, ano_letivo, serie_ano, quantidade_aluno, bimestre, criado_em) 
                FROM STDIN (FORMAT BINARY)
            ");

            foreach (var item in indicadores)
            {
                await writer.StartRowAsync();
                await writer.WriteAsync(item.CodigoDre, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.CodigoUe, NpgsqlTypes.NpgsqlDbType.Varchar);
                await writer.WriteAsync(item.PreSilabico, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.SilabicoSemValor, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.SilabicoComValor, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.SilabicoAlfabetico, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.Alfabetico, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.SemPreenchimento, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.AnoLetivo, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.SerieAno, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.QuantidadeAluno, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.Bimestre, NpgsqlTypes.NpgsqlDbType.Integer);
                await writer.WriteAsync(item.CriadoEm, NpgsqlTypes.NpgsqlDbType.Timestamp);
            }

            await writer.CompleteAsync();
        }

        public async Task LimparConsolidacao()
        {
            var sql = "DELETE FROM painel_educacional_consolidacao_sondagem_escrita_ue";
            await database.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoSondagemEscritaUe>> ObterConsolidacaoAsync(int anoLetivo, int bimestre, string codigoUe, string codigoDre)
        {
            string query = @"select  
                                    codigo_dre as CodigoDre,
                                    codigo_ue as CodigoUe,
                                    pre_silabico as PreSilabico,
                                    silabico_com_valor as SilabicoComValor,
                                    silabico_sem_valor as SilabicoSemValor,
                                    silabico_alfabetico as SilabicoAlfabetico,
                                    alfabetico as Alfabetico,
                                    sem_preenchimento as SemPreenchimento,
                                    quantidade_aluno as QuantidadeAluno,
                                    ano_letivo as AnoLetivo,      
                                    serie_ano as SerieAno,      
                                    bimestre as Bimestre
                                from painel_educacional_consolidacao_sondagem_escrita_ue
                            where ano_letivo = @anoLetivo
                            AND bimestre = @periodo
                            AND codigo_ue = @codigoUe
                            AND codigo_dre = @codigoDre";

            return await database.Conexao.QueryAsync<PainelEducacionalConsolidacaoSondagemEscritaUe>(query, new
            {
                anoLetivo,
                bimestre,
                codigoUe,
                codigoDre
            });
        }      
    }
}
