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


        public async Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendenciaAula tipoPendenciaAula, string tabelaReferencia)
        {
            var query = $@"select aula.id as Id from aula 
                    left join
	                    pendencia_aula on aula.id = aula_id and pendencia_aula.tipo = @tipo
                    left join
	                    {tabelaReferencia} on aula.id = {tabelaReferencia}.aula_id
                    where 
                        not aula.excluido and 
                        aula.data_aula < @hoje and
                        pendencia_aula.id is null and 
                        {tabelaReferencia}.id is null
                    group by aula.id";

            return (await database.Conexao.QueryAsync<Aula>(query, new { hoje = DateTime.Today, tipo = tipoPendenciaAula}));
        }


        public async Task<IEnumerable<Aula>> ListarPendenciasAtividadeAvaliativa()
        {
            var sql = @"select a.id as Id from atividade_avaliativa aa 
                    inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = aa.id
                    left join notas_conceito n on n.atividade_avaliativa = aa.id
                    inner join aula a on aa.turma_id = a.turma_id 
                     where 
                        not a.excluido and 
                        a.data_aula < @hoje and  
                        n.id is null and
                    a.id not in (select aula_id from pendencia_aula pa where pa.tipo = @tipo)
            ";

            return (await database.Conexao.QueryAsync<Aula>(sql.ToString(), new { hoje = DateTime.Today, tipo = TipoPendenciaAula.Avaliacao }));
        }


        public async Task<PendenciaAula> ObterPendenciaPorAulaIdETipo(TipoPendenciaAula tipoPendenciaAula, long aulaId)
        {
            var query = $@"select id as Id, aula_id as AulaId, tipo as TipoPendenciaAula from pendencia_aula 
                    WHERE tipo = @tipo AND aula_id = @aulaid";

            return (await database.Conexao.QueryFirstOrDefaultAsync<PendenciaAula>(query, new { aulaid = aulaId, tipo = tipoPendenciaAula }));
        }

        public async Task Excluir(TipoPendenciaAula tipoPendenciaAula, long aulaId)
        {
            await database.Conexao.ExecuteScalarAsync("delete from pendencia_aula where aula_id= @aulaid and tipo = @tipo", new { aulaid = aulaId, tipo = tipoPendenciaAula });
        }

        public async Task Salvar(PendenciaAula pendencia)
        {
            await database.Conexao.InsertAsync(pendencia);
        }

        public void SalvarVarias(IEnumerable<Aula> aulas, TipoPendenciaAula tipoPendenciaAula)
        {
            var sql = @"copy pendencia_aula (aula_id, tipo)
                            from
                            stdin (FORMAT binary)";
            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var aula in aulas)
                {
                    writer.StartRow();
                    writer.Write(aula.Id, NpgsqlDbType.Bigint);
                    writer.Write((long)tipoPendenciaAula, NpgsqlDbType.Bigint);
                }
                writer.Complete();
            }
        }

        public async Task<long[]> ListarPendenciasPorAulaId(long aulaId)
        {
            var sql = @"select tipo from pendencia_aula
                     where 
                        id = @aula
            ";

            return ((long[])await database.Conexao.QueryAsync<long>(sql.ToString(), new { aula = aulaId }));
        }

        public async Task<long[]> ListarPendenciasPorAulasId(long[] aulas)
        {

            var sql = @"select tipo from pendencia_aula where aula_id =ANY(@aulas) group by tipo";

            return (await database.Conexao.QueryAsync<long>(sql.ToString(), new { aulas })).AsList().ToArray();
        }
    }
}
