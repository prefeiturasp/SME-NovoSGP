using Dapper;
using Npgsql;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseAlunoRecomendacao : IRepositorioConselhoClasseAlunoRecomendacao
    {
        protected readonly ISgpContext database;
        public RepositorioConselhoClasseAlunoRecomendacao(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> ObterRecomendacoesDoAlunoPorConselho(string alunoCodigo, int? bimestre, long fechamentoTurmaId, long[] conselhoClasseIds)
        {
            var sql = new StringBuilder();

            sql.AppendLine("select ccr.id as Id, ccr.recomendacao as Recomendacao, ccr.tipo as Tipo from conselho_classe_aluno_recomendacao ccar");
            sql.AppendLine(" inner join conselho_classe_recomendacao ccr on ccr.id = ccar.conselho_classe_recomendacao_id");
            sql.AppendLine(" inner join conselho_classe_aluno cca on cca.id = ccar.conselho_classe_aluno_id");
            sql.AppendLine(" inner join conselho_classe cc on cc.id = cca.conselho_classe_id");
            sql.AppendLine(" inner join fechamento_turma ft on ft.id = cc.fechamento_turma_id");
            sql.AppendLine(" left join periodo_escolar pe on pe.id = ft.periodo_escolar_id");
            sql.AppendLine(@" where cca.aluno_codigo = @alunoCodigo");

            if (bimestre.HasValue)
                sql.AppendLine(@" and pe.bimestre = @bimestre");

            sql.AppendLine(@" and ft.id = @fechamentoTurmaId");

            if (conselhoClasseIds.Any())
                sql.AppendLine(@" and cc.id = ANY(@conselhoClasseIds)");

            return await database.Conexao.QueryAsync<RecomendacoesAlunoFamiliaDto>(sql.ToString(), new { alunoCodigo, bimestre, fechamentoTurmaId, conselhoClasseIds});
        }

        public void InserirRecomendacaoAlunoFamilia(long[] recomendacoesId, long conselhoClasseAlunoId)
        {
            var sql = @"copy conselho_classe_aluno_recomendacao ( 
                                        conselho_classe_aluno_id, 
                                        conselho_classe_recomendacao_id)
                            from
                            stdin (FORMAT binary)";
            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var recomendacao in recomendacoesId)
                {
                    writer.StartRow();
                    writer.Write(conselhoClasseAlunoId, NpgsqlTypes.NpgsqlDbType.Bigint);
                    writer.Write(recomendacao, NpgsqlTypes.NpgsqlDbType.Bigint);
                }
                writer.Complete();
            }
        }

        public async Task<IEnumerable<long>> ObterRecomendacoesDoAlunoPorConselhoAlunoId(long conselhoClasseAlunoId)
        {
            var sql = new StringBuilder();

            sql.AppendLine("select conselho_classe_recomendacao_id from conselho_classe_aluno_recomendacao where conselho_classe_aluno_id = @conselhoClasseAlunoId");

            return await database.Conexao.QueryAsync<long>(sql.ToString(), new { conselhoClasseAlunoId });
        }

        public async Task ExcluirRecomendacoesPorConselhoAlunoIdRecomendacaoId(long conselhoClasseAlunoId, long[] recomendacoesId)
        {
            var sql = new StringBuilder();

            sql.AppendLine("delete from conselho_classe_aluno_recomendacao where conselho_classe_aluno_id = @conselhoClasseAlunoId and conselho_classe_recomendacao_id = ANY(@recomendacoesId)");

            await database.Conexao.ExecuteScalarAsync(sql.ToString(), new { conselhoClasseAlunoId, recomendacoesId });
        }
    }
}
