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


        public async Task<IEnumerable<Aula>> ListarPendenciasPorTipo(TipoPendenciaAula tipoPendenciaAula)
        {
            var query = $@"select aula.id as Id 
	                    from aula 
                    inner join diario_bordo on aula.id = aula_id 
                    where 
                        not aula.excluido and 
                        aula.data_aula < @hoje and
                        aula.id not in (select aula_id from pendencia_aula pa where pa.tipo = @tipo)
                    group by aula.id";

            return (await database.Conexao.QueryAsync<Aula>(query, new { hoje = DateTime.Today, tipo = tipoPendenciaAula }));
        }

        public async Task Excluir(PendenciaAula pendencia)
        {
            await database.Conexao.DeleteAsync(pendencia);
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

    }
}
