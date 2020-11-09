using Dapper;
using Dommel;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaAula : IRepositorioPendenciaAula
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaAula(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades)
        {
            var query = $@"select
	                        aula.id as Id
                        from
	                        aula
                        inner join turma on 
	                        aula.turma_id = turma.turma_id
                        left join pendencia_aula pa on
	                        aula.id = pa.aula_id
	                    left join pendencia p on p.id = pa.pendencia_id and p.tipo = @tipo
                        left join {tabelaReferencia} on
	                        aula.id = {tabelaReferencia}.aula_id
                        where
	                        not aula.excluido
	                        and aula.data_aula < @hoje
	                        and turma.modalidade_codigo = ANY(@modalidades)
	                        and pendencia_aula.id is null
	                        and {tabelaReferencia}.id is null
                        group by
	                        aula.id";

            return (await database.Conexao.QueryAsync<Aula>(query, new { hoje = DateTime.Today, tipo = tipoPendenciaAula, modalidades }));
        }

        public async Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa()
        {
            var sql = @"select
	                        a.id as Id
                        from
	                        atividade_avaliativa aa
                        inner join atividade_avaliativa_disciplina aad on
	                        aad.atividade_avaliativa_id = aa.id
                        left join notas_conceito n on
	                        aa.id = n.atividade_avaliativa
                        inner join aula a on
	                        aa.turma_id = a.turma_id
	                        and aa.data_avaliacao::date = a.data_aula::date
	                        and aad.disciplina_id = a.disciplina_id
                        left join pendencia_aula pa on
	                        a.id = pa.aula_id
                        left join pendencia p on p.id = pa.pendencia_id
	                        and p.tipo = @tipo
                        where
	                        not a.excluido
	                        and a.data_aula::date < @hoje
	                        and n.id is null
	                        and pa.id is null
	                        and p.id is null
                        group by
	                        a.id";

            return (await database.Conexao.QueryAsync<Aula>(sql.ToString(), new { hoje = DateTime.Today, tipo = TipoPendencia.Avaliacao }));
        }

        public async Task Excluir(TipoPendencia tipoPendenciaAula, long aulaId)
        {
            await database.Conexao.ExecuteScalarAsync(@"delete from pendencia_aula pa
                                                     inner join pendencia p on p.id = pa.pendencia_id
                                                    where pa.aula_id= @aulaid and p.tipo = @tipo", new { aulaid = aulaId, tipo = tipoPendenciaAula });
        }

        public async Task Salvar(PendenciaAula pendencia)
        {
            await database.Conexao.InsertAsync(pendencia);
        }

        public async Task SalvarVarias(long pendenciaId, IEnumerable<long> aulas)
        {
            var sql = @"copy pendencia_aula (pendencia_id, aula_id)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var aulaId in aulas)
                {
                    writer.StartRow();
                    writer.Write(pendenciaId, NpgsqlDbType.Bigint);
                    writer.Write(aulaId, NpgsqlDbType.Bigint);
                }
                writer.Complete();
            }
        }

        public async Task<long[]> ListarPendenciasPorAulaId(long aulaId)
        {
            var sql = @"select tipo from pendencia_aula where aula_id = @aula group by tipo";

            return (await database.Conexao.QueryAsync<long>(sql.ToString(), new { aula = aulaId })).AsList().ToArray();
        }

        public async Task<long[]> ListarPendenciasPorAulasId(long[] aulas)
        {

            var sql = @"select tipo from pendencia_aula where aula_id =ANY(@aulas) group by tipo";

            return (await database.Conexao.QueryAsync<long>(sql.ToString(), new { aulas })).AsList().ToArray();
        }

        public async Task<Turma> ObterNomeTurmaPorPendencia(long pendenciaId)
        {
            var query = @"select t.* 
                         from pendencia_aula pa
                        inner join aula a on a.id = pa.aula_id
                        inner join turma t on t.turma_id = a.turma_id
                        where pa.pendencia_id = @pendenciaId ";

            return await database.Conexao.QueryFirstAsync<Turma>(query, new { pendenciaId });
        }

        public async Task<IEnumerable<PendenciaAulaDto>> ObterPendenciasAulasPorPendencia(long pendenciaId)
        {
            var query = @"select a.data_aula as DataAula, pa.Motivo
                           from pendencia_aula pa
                          inner join aula a on a.id = pa.aula_id
                          where pa.pendencia_id = @pendenciaId";

            return await database.Conexao.QueryAsync<PendenciaAulaDto>(query, new { pendenciaId });
        }
    }
}
