using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricular : IRepositorioComponenteCurricular
    {
        private readonly ISgpContext database;

        public RepositorioComponenteCurricular(ISgpContext database)
        {
            this.database = database;
        }

        public void SalvarVarias(IEnumerable<ComponenteCurricularDto> componentesCurriculares)
        {
            var sql = @"copy componente_curricular (id, 
                                                    descricao, 
                                                    eh_regencia,
                                                    eh_compartilhada, 
                                                    eh_territorio, 
                                                    eh_base_nacional, 
                                                    permite_registro_frequencia, 
                                                    permite_lancamento_nota)
                            from
                            stdin (FORMAT binary)";
            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var componenteCurricular in componentesCurriculares)
                {
                    writer.StartRow();
                    writer.Write(long.Parse(componenteCurricular.Codigo), NpgsqlDbType.Bigint);
                    writer.Write(componenteCurricular.Descricao, NpgsqlDbType.Varchar);
                    writer.Write(false, NpgsqlDbType.Boolean);
                    writer.Write(false, NpgsqlDbType.Boolean);
                    writer.Write(false, NpgsqlDbType.Boolean);
                    writer.Write(false, NpgsqlDbType.Boolean);
                    writer.Write(true, NpgsqlDbType.Boolean);
                    writer.Write(true, NpgsqlDbType.Boolean);
                }
                writer.Complete();
            }
        }

        public async Task Atualizar(ComponenteCurricularDto componenteCurricular)
        {
            try
            {
                var sql = @"UPDATE componente_curricular SET descricao = @descricao WHERE id = @id";

                await database.Conexao.QueryAsync<string>(sql.ToString(), new { descricao = componenteCurricular.Descricao, id = long.Parse(componenteCurricular.Codigo) });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }           

        }
    }
    
}

