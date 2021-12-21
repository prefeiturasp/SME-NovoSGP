using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaAula : IRepositorioPendenciaAula
    {
        private readonly ISgpContext database;

        public RepositorioPendenciaAula(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task Excluir(long pendenciaId, long aulaId)
        {
            await database.Conexao.ExecuteScalarAsync(@"delete from pendencia_aula where aula_id = @aulaId and pendencia_id = @pendenciaId", new { aulaid = aulaId, pendenciaId });
        }

        public async Task Salvar(long aulaId, string motivo, long pendenciaId)
        {
            await database.Conexao.InsertAsync(new PendenciaAula()
            {
                AulaId = aulaId,
                Motivo = motivo,
                PendenciaId = pendenciaId
            });
        }

        public void SalvarVarias(long pendenciaId, IEnumerable<long> aulas)
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
    }
}