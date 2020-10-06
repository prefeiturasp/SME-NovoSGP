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

        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIds(long[] ids)
        {
            var query = $@"select
                            id,
	                        id as CodigoComponenteCurricular,
                            area_conhecimento_id as AreaConhecimentoId,
                            componente_curricular_pai_id as CdComponenteCurricularPai,
                            case
		                        when descricao_sgp is not null then descricao_sgp
		                        else descricao
	                        end as Nome,
                            eh_base_nacional as EhBaseNacional,
                            eh_compartilhada as Compartilhada,
                            eh_regencia as Regencia,
                            eh_territorio as TerritorioSaber,
                            grupo_matriz_id as GrupoMatrizId,
                            permite_lancamento_nota as LancaNota,
                            permite_registro_frequencia as RegistraFrequencia
                        from
	                        componente_curricular WHERE id = ANY(@ids)";
            return (await database.Conexao.QueryAsync<DisciplinaDto>(query, new { ids }));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> ListarComponentesCurriculares()
        {
            var query = $@"select
	                        id as Codigo,
                            case
		                        when descricao_sgp is not null then descricao_sgp
		                        else descricao
	                        end as descricao
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

        public async Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesRegenciaPorAnoETurno(long ano, long turno)
        {
            var query = $@"select
	                    distinct cc.id as CodigoComponenteCurricular,
	                    cc.area_conhecimento_id as AreaConhecimentoId,
	                    cc.componente_curricular_pai_id as CdComponenteCurricularPai,
	                    case
		                    when cc.descricao_sgp is not null then cc.descricao_sgp
		                    else cc.descricao
	                    end as Nome,
	                    cc.eh_base_nacional as EhBaseNacional,
	                    cc.eh_compartilhada as Compartilhada,
	                    cc.eh_regencia as Regencia,
	                    cc.eh_territorio as TerritorioSaber,
	                    cc.grupo_matriz_id as GrupoMatrizId,
	                    cc.permite_lancamento_nota as LancaNota,
	                    cc.permite_registro_frequencia as RegistraFrequencia
                    from
	                    componente_curricular_regencia ccr
                    inner join componente_curricular cc on
	                    ccr.componente_curricular_id = cc.id
                    where
	                    (turno = @turno
	                    and ano = @ano)
	                    or turno is null";

            return (await database.Conexao.QueryAsync<DisciplinaDto>(query, new { turno, ano }));
        }
    }

}