using Dapper;
using Npgsql;
using NpgsqlTypes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
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
                                cc.id,
                                cc.id as CodigoComponenteCurricular,
                                cc.area_conhecimento_id as AreaConhecimentoId,
                                cc.componente_curricular_pai_id as CdComponenteCurricularPai,
                                coalesce(cc.descricao_sgp,cc.descricao) as Nome,
                                cc.eh_base_nacional as EhBaseNacional,
                                cc.eh_compartilhada as Compartilhada,
                                cc.eh_regencia as Regencia,
                                cc.eh_territorio as TerritorioSaber,
                                cc.grupo_matriz_id as GrupoMatrizId,
                                ccgm.nome as GrupoMatrizNome,
                                cc.permite_lancamento_nota as LancaNota,
                                cc.permite_registro_frequencia as RegistraFrequencia
                           from componente_curricular cc 
                           left join componente_curricular_grupo_matriz ccgm on ccgm.id = cc.grupo_matriz_id 
                          WHERE cc.id = ANY(@ids)";
            return (await database.Conexao.QueryAsync<DisciplinaDto>(query, new { ids }));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> ListarComponentesCurriculares()
        {
            var query = $@"select
	                        id as Codigo,
                            permite_lancamento_nota as LancaNota,
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

        public async Task<bool> VerificaPossuiObjetivosAprendizagemPorComponenteCurricularId(long id)
        {
            var query = $@"select
	                           distinct 1
                            from
	                            componente_curriculo_cidade ccc
                            inner join componente_curricular cc on
	                            ccc.componente_curricular_id = cc.id
                            inner join objetivo_aprendizagem oa on
                                ccc.codigo = oa.componente_curricular_id
                            WHERE ccc.componente_curricular_id = @id;";

            return (await database.Conexao.QueryAsync<int>(query, new { id })).Any() ;
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

        public async Task<bool> VerificarComponenteCurriculareSeERegenciaPorId(long id)
        {
            var query = $@"select
                            eh_regencia
                        from
	                        componente_curricular WHERE id = @id;";
            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { id }));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre(int anoLetivo, long turmaId, int bimestre, string codigoAluno)
        {
            var query = @"	                        
                           select
                           distinct *
                           from
                           (
                           select 
                           fn.disciplina_id as Codigo,
                           comp.descricao as Descricao, 
                           comp.permite_lancamento_nota as LancaNota
                           from
                           fechamento_turma ft
                           left join periodo_escolar pe on
                           pe.id = ft.periodo_escolar_id
                           inner join turma t on
                           t.id = ft.turma_id
                           inner join ue on
                           ue.id = t.ue_id
                           inner join fechamento_turma_disciplina ftd on
                           ftd.fechamento_turma_id = ft.id
                           inner join fechamento_aluno fa on
                           fa.fechamento_turma_disciplina_id = ftd.id
                           inner join fechamento_nota fn on
                           fn.fechamento_aluno_id = fa.id
                           inner join componente_curricular comp 
                           on comp.id = fn.disciplina_id
                           inner join conselho_classe cc on
                           cc.fechamento_turma_id = ft.id
                           inner join conselho_classe_aluno cca on
                           cca.conselho_classe_id = cc.id
                           and cca.aluno_codigo = fa.aluno_codigo
                           left join conselho_classe_nota ccn on
                           ccn.conselho_classe_aluno_id = cca.id
                           and ccn.componente_curricular_codigo = fn.disciplina_id
                           where
                           pe.bimestre = @bimestre
                           and cca.aluno_codigo = @codigoAluno
                           and t.ano_letivo = @anoLetivo
                           and ft.turma_id = @turmaId
                           union all
                           
                           select 
                           ccn.componente_curricular_codigo as Codigo,
                           comp.descricao as Descricao, 
                           comp.permite_lancamento_nota as LancaNota
                           from
                           fechamento_turma ft
                           left join periodo_escolar pe on
                           pe.id = ft.periodo_escolar_id
                           inner join turma t on
                           t.id = ft.turma_id
                           inner join ue on
                           ue.id = t.ue_id
                           inner join conselho_classe cc on
                           cc.fechamento_turma_id = ft.id
                           inner join conselho_classe_aluno cca on
                           cca.conselho_classe_id = cc.id
                           inner join conselho_classe_nota ccn on
                           ccn.conselho_classe_aluno_id = cca.id
                           inner join componente_curricular comp 
                           on comp.id = ccn.componente_curricular_codigo
                           left join fechamento_turma_disciplina ftd on
                           ftd.fechamento_turma_id = ft.id
                           left join fechamento_aluno fa on
                           fa.fechamento_turma_disciplina_id = ftd.id
                           and cca.aluno_codigo = fa.aluno_codigo
                           left join fechamento_nota fn on 
                           fn.fechamento_aluno_id = fa.id
                           and ccn.componente_curricular_codigo = fn.disciplina_id
                           where
                           pe.bimestre = @bimestre
                           and cca.aluno_codigo = @codigoAluno
                           and t.ano_letivo = @anoLetivo
                           and ft.turma_id = @turmaId ) x   ";

            return (await database.Conexao.QueryAsync<ComponenteCurricularDto>(query, new { bimestre , anoLetivo, turmaId, codigoAluno, }));
        }

        public async Task<bool> LancaNota(long id)
        {
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>("select permite_lancamento_nota from componente_curricular where id = @id", new { id });
        }



        public async Task<string> ObterDescricaoPorId(long id)
        {
            var query = @"select coalesce(descricao_sgp, descricao) from componente_curricular cc where id = @id";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { id });
        }

        public async Task<IEnumerable<ComponenteCurricularSimplesDto>> ObterDescricaoPorIds(long[] ids)
        {
            var query = @"select id, coalesce(descricao_sgp, descricao) as descricao from componente_curricular where id = Any(@ids)";

            return await database.Conexao.QueryAsync<ComponenteCurricularSimplesDto>(query, new { ids });
        }
    }
    
}

