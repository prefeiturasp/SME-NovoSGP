using Dapper;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricularConsulta : IRepositorioComponenteCurricularConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioComponenteCurricularConsulta(ISgpContextConsultas database)
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
                                case                     
                                when (cc.descricao_sgp is not null and cc.descricao_sgp  != '' and cc.descricao_sgp != ' ')
                                then
                                	cc.descricao_sgp
                                else
                                 	cc.descricao 
                                end as Nome,
                                descricao_infantil as NomeComponenteInfantil,
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

        public async Task<DisciplinaDto> ObterDisciplinaPorId(long id)
        {
            var query = $@"select
                                cc.id,
                                cc.id as CodigoComponenteCurricular,
                                cc.area_conhecimento_id as AreaConhecimentoId,
                                cc.componente_curricular_pai_id as CdComponenteCurricularPai,
                                coalesce(cc.descricao_sgp,cc.descricao) as Nome,
                                descricao_infantil as NomeComponenteInfantil,
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
                          WHERE cc.id = @id";
            return (await database.Conexao.QueryFirstOrDefaultAsync<DisciplinaDto>(query, new { id }));
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> ListarComponentesCurriculares()
        {
            try
            {
                var query = $@"select
	                        id as Codigo,
                            permite_lancamento_nota as LancaNota,
                            coalesce(descricao_infantil,descricao_sgp) as descricao,
                            descricao as DescricaoEol,
                            eh_regencia Regencia
                        from
	                        componente_curricular";

                var retorno = (await database.Conexao.QueryAsync<ComponenteCurricularDto>(query, new { }));
                return retorno;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
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

            return (await database.Conexao.QueryAsync<int>(query, new { id })).Any();
        }

        public Task<IEnumerable<DisciplinaDto>> ObterComponentesCurricularesRegenciaPorAnoETurno(long ano, long turno)
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

            return database.Conexao.QueryAsync<DisciplinaDto>(query, new { turno, ano });
        }

        public async Task<bool> VerificarComponenteCurriculareSeERegenciaPorId(long id)
        {
            var query = $@"select
                            eh_regencia
                        from
	                        componente_curricular WHERE id = @id;";
            return (await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { id }));
        }        
        public async Task<bool> LancaNota(long id)
        {
            return await database.Conexao.QueryFirstOrDefaultAsync<bool>("select permite_lancamento_nota from componente_curricular where id = @id", new { id });
        }

        public async Task<IEnumerable<ComponenteCurricularDto>> ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre(int anoLetivo, string[] turmasId, int? bimestre, string codigoAluno)
        {
            var query = $@"	select distinct *
                           from
                           (
                           select
		                        fn.disciplina_id as Codigo,
		                        comp.descricao as Descricao,
		                        comp.permite_lancamento_nota as LancaNota	
	                        from fechamento_turma ft
	                            left join periodo_escolar pe on pe.id = ft.periodo_escolar_id
	                            inner join turma t on t.id = ft.turma_id
	                            inner join ue on ue.id = t.ue_id
	                            inner join fechamento_turma_disciplina ftd on ftd.fechamento_turma_id = ft.id
	                            inner join fechamento_aluno fa on fa.fechamento_turma_disciplina_id = ftd.id
	                            inner join fechamento_nota fn on fn.fechamento_aluno_id = fa.id
	                            inner join componente_curricular comp  on comp.id = fn.disciplina_id
	                        where
		                       {(bimestre.HasValue && bimestre.Value > 0 ? " pe.bimestre = @bimestre " : " pe.bimestre is null ")} 
                               and fa.aluno_codigo = @codigoAluno
                               and t.ano_letivo = @anoLetivo
                               and t.turma_id = ANY(@turmasId)
                                                      
                           union all
                           
                           select
	                            ccn.componente_curricular_codigo as Codigo,
	                            comp.descricao as Descricao,
	                            comp.permite_lancamento_nota as LancaNota
                            from
	                            fechamento_turma ft
                                left join periodo_escolar pe on pe.id = ft.periodo_escolar_id
                                inner join turma t on t.id = ft.turma_id
                                inner join ue on ue.id = t.ue_id
                                inner join conselho_classe cc on cc.fechamento_turma_id = ft.id
                                inner join conselho_classe_aluno cca on cca.conselho_classe_id = cc.id
                                inner join conselho_classe_nota ccn on ccn.conselho_classe_aluno_id = cca.id and not ccn.excluido
                                inner join componente_curricular comp  on comp.id = ccn.componente_curricular_codigo
                            where
                                {(bimestre.HasValue && bimestre.Value > 0 ? " pe.bimestre = @bimestre " : " pe.bimestre is null ")} 
                               and cca.aluno_codigo = @codigoAluno
                               and t.ano_letivo = @anoLetivo
                               and t.turma_id = ANY(@turmasId)
                            ) x   ";

            return (await database.Conexao.QueryAsync<ComponenteCurricularDto>(query, new { bimestre, anoLetivo, turmasId, codigoAluno, }));
        }

        public async Task<string> ObterDescricaoPorId(long id)
        {
            var query = @"select 
                           case 
                            when descricao_infantil is not null and descricao_infantil != '' and descricao_infantil != ' '
                               then 
                                    descricao_infantil 
                               else 
           		                    case
           		                     when descricao_sgp != null and descricao_sgp != '' and descricao_sgp != ' '
           		                        then 
                                            descricao_sgp
           		                        else 
           		                            descricao
           		                    end 
           	                    end
                           from componente_curricular cc where id = @id";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { id });
        }

        public async Task<IEnumerable<ComponenteCurricularDescricaoDto>> ObterDescricaoPorIds(long[] ids)
        {
            var query = @"select id, coalesce(descricao_sgp, descricao) as descricao, descricao_infantil as descricaoinfantil from componente_curricular where id = Any(@ids)";

            return await database.Conexao.QueryAsync<ComponenteCurricularDescricaoDto>(query, new { ids },queryName: "ObterDescricaoPorIds");
        }

        public async Task<string> ObterCodigoComponentePai(long componenteCurricularId)
        {
            var query = @"select coalesce(componente_curricular_pai_id,id) from componente_curricular where id = @componenteCurricularId";

            return await database.Conexao.QueryFirstOrDefaultAsync<string>(query, new { componenteCurricularId });
        }

        public async Task<IEnumerable<ComponenteCurricularSimplesDto>> ObterComponentesSimplesPorIds(long[] ids)
        {
            var query = @"select id, coalesce(descricao_sgp, descricao) as descricao, descricao_infantil as descricaoinfantil, permite_lancamento_nota as permiteLanctoNota from componente_curricular where id = Any(@ids)";

            return await database.Conexao.QueryAsync<ComponenteCurricularSimplesDto>(query, new { ids }, queryName: "ObterComponentesSimplesPorIds");
        }

        public async Task<ComponenteGrupoMatrizDto> ObterComponenteGrupoMatrizPorId(long id)
        {
            var query = @"select cc.id as ComponenteCurricularId, ccgm.id as GrupoMatrizId, ccgm.nome as GrupoMatrizNome from componente_curricular cc
                            left join componente_curricular_grupo_matriz ccgm on ccgm.id = cc.grupo_matriz_id
                         where cc.id = @id";

            return await database.Conexao.QueryFirstOrDefaultAsync<ComponenteGrupoMatrizDto>(query, new { id }, queryName: "ObterComponenteGrupoMatrizPorId");
        }
    }
}

