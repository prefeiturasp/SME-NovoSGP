using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
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

        public async Task<IEnumerable<ComponenteCurricularDto>> ListarComponentesCurriculares()
        {
            var query = $@"select
	                        id as Codigo,
                            descricao
                        from
	                        componente_curricular";

            return (await database.Conexao.QueryAsync<ComponenteCurricularDto>(query, new { }));
        }
        public async Task<long[]> ListarCodigosJuremaPorComponenteCurricularId(long id)
        {
            var query = $@"select
	                            distinct codigo
                            from
	                            componente_curriculo_cidade ccc
                            inner join componente_curricular cc on
	                            ccc.componente_curricular_id = cc.id
                            WHERE ccc.componente_curricular_id = @id;";

            return (await database.Conexao.QueryAsync<long>(query, new { id })).AsList().ToArray();
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
    }

}