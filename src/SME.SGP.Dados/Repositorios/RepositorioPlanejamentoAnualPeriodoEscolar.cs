using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualPeriodoEscolar : RepositorioBase<PlanejamentoAnualPeriodoEscolar>, IRepositorioPlanejamentoAnualPeriodoEscolar
    {
        public RepositorioPlanejamentoAnualPeriodoEscolar(ISgpContext database) : base(database)
        {
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> ObterPorPlanejamentoAnualIdEPeriodoId(long id, long periodoEscolarId, bool consideraExcluido = false)
        {
            var sql = $@"select * 
                             from planejamento_anual_periodo_escolar 
                         where planejamento_anual_id = @id and 
                               periodo_escolar_id = @periodoEscolarId {(!consideraExcluido ? "and not excluido" : string.Empty)} 
                         order by id desc;";

            return await database.Conexao
                .QueryFirstOrDefaultAsync<PlanejamentoAnualPeriodoEscolar>(sql, new { id, periodoEscolarId });
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> ObterPlanejamentoAnualPeriodoEscolarPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId, long periodoEscolarId)
        {
            var sql = @"select
	                        distinct pape.*,
                            pe.*,
	                        pac.*,
	                        paoa.*
                        from
	                        planejamento_anual pa
                        inner join planejamento_anual_periodo_escolar pape on
	                        pape.planejamento_anual_id = pa.id
                        inner join planejamento_anual_componente pac on
	                        pac.planejamento_anual_periodo_escolar_id = pape.id
                        left join planejamento_anual_objetivos_aprendizagem paoa on
	                        paoa.planejamento_anual_componente_id = pac.id
                        inner join periodo_escolar pe on pape.periodo_escolar_id = pe.id
                        where
	                        turma_id = @turmaId
	                        and pac.componente_curricular_id = @componenteCurricularId
	                        and pape.periodo_escolar_id = @periodoEscolarId
                            and pape.excluido = false 
                            and pa.excluido = false 
                            and pac.excluido = false
                            and (paoa.excluido is null or paoa.excluido = false)
                        order by paoa.objetivo_aprendizagem_id";

            var periodos = new List<PlanejamentoAnualPeriodoEscolar>();
            await database.Conexao.QueryAsync<PlanejamentoAnualPeriodoEscolar, PeriodoEscolar, PlanejamentoAnualComponente, PlanejamentoAnualObjetivoAprendizagem, PlanejamentoAnualPeriodoEscolar>(sql,
                (periodo, periodoEscolar, componente, objetivo) =>
                {
                    var periodoAdicionado = periodos.FirstOrDefault(c => c.Id == periodo.Id);
                    if (periodoAdicionado == null)
                    {
                        componente.ObjetivosAprendizagem.Add(objetivo);
                        periodo.ComponentesCurriculares.Add(componente);
                        periodo.PeriodoEscolar = periodoEscolar;
                        periodos.Add(periodo);
                    }
                    else
                    {
                        var componenteCurricular = periodoAdicionado.ComponentesCurriculares.FirstOrDefault(c => c.Id == componente.Id);
                        if (componenteCurricular != null)
                        {
                            var objetivoAprendizagem = componenteCurricular.ObjetivosAprendizagem.FirstOrDefault(c => c.Id == objetivo.Id);
                            if (objetivoAprendizagem == null)
                            {
                                componenteCurricular.ObjetivosAprendizagem.Add(objetivo);
                            }
                        }
                        else
                        {
                            componenteCurricular = componente;
                            componenteCurricular.ObjetivosAprendizagem.Add(objetivo);
                            periodoAdicionado.ComponentesCurriculares.Add(componenteCurricular);
                        }
                    }
                    return periodo;
                },
                new { turmaId, componenteCurricularId, periodoEscolarId });

            return periodos.FirstOrDefault();
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolar>> ObterCompletoPorIdAsync(long[] ids)
        {
            List<PlanejamentoAnualPeriodoEscolar> retorno = new List<PlanejamentoAnualPeriodoEscolar>();

            var sql = @"select
                            id,
	                        periodo_escolar_id,
	                        planejamento_anual_id
                        from
	                        planejamento_anual_periodo_escolar
                        where
	                        planejamento_anual_periodo_escolar.id = ANY(@ids) and excluido = false;

                        select
                            id,
	                        componente_curricular_id,
	                        descricao,
	                        planejamento_anual_periodo_escolar_id
                        from
	                        planejamento_anual_componente
                        where planejamento_anual_periodo_escolar_id = ANY(@ids) and excluido = false;

                        select
                            planejamento_anual_objetivos_aprendizagem.id,
	                        planejamento_anual_objetivos_aprendizagem.planejamento_anual_componente_id,
	                        planejamento_anual_objetivos_aprendizagem.objetivo_aprendizagem_id
                        from
	                        planejamento_anual_objetivos_aprendizagem
                        inner join planejamento_anual_componente on
	                        planejamento_anual_objetivos_aprendizagem.planejamento_anual_componente_id = planejamento_anual_componente.id
                        where
	                        planejamento_anual_componente.planejamento_anual_periodo_escolar_id = ANY(@ids) 
                            and planejamento_anual_objetivos_aprendizagem.excluido = false 
                            and planejamento_anual_componente.excluido = false;";

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, new { ids }))
            {
                retorno = multi.Read<PlanejamentoAnualPeriodoEscolar>().ToList();
                var componentes = multi.Read<PlanejamentoAnualComponente>().ToList();
                var objetivoAprendizagems = multi.Read<PlanejamentoAnualObjetivoAprendizagem>().ToList();

                componentes.ForEach(c => c.ObjetivosAprendizagem.AddRange(objetivoAprendizagems.Where(oa => oa.PlanejamentoAnualComponenteId == c.Id)));

                retorno.ForEach(pe =>
                    pe.ComponentesCurriculares.AddRange(componentes.Where(c =>
                        c.PlanejamentoAnualPeriodoEscolarId == pe.Id)));
            }

            return retorno;
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> ObterPorPlanejamentoAnualId(long planejamentoAnualId, int[] bimestresConsiderados)
        {
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("select pape.id,");
            sqlQuery.AppendLine("       pe.bimestre");
            sqlQuery.AppendLine("   from planejamento_anual_periodo_escolar pape");
            sqlQuery.AppendLine("       inner join periodo_escolar pe");
            sqlQuery.AppendLine("           on pape.periodo_escolar_id = pe.id");
            sqlQuery.AppendLine("where pape.planejamento_anual_id = @planejamentoAnualId and");
            if (bimestresConsiderados.Any())
                sqlQuery.AppendLine("      pe.bimestre = any(@bimestresConsiderados) and");
            sqlQuery.AppendLine("      not pape.excluido;");

            return await database.Conexao
                .QueryAsync<PlanejamentoAnualPeriodoEscolarResumoDto>(sqlQuery.ToString(), new { planejamentoAnualId, bimestresConsiderados });
        }

        public async Task<bool> PlanejamentoPossuiObjetivos(long planejamentoAnualPeriodoId)
        {
            var query = @"select pc.id
                            from planejamento_anual_componente pc 
                           where pc.planejamento_anual_periodo_escolar_id = @planejamentoAnualPeriodoId";

            return (await database.Conexao.QueryAsync<int>(query, new { planejamentoAnualPeriodoId })).Any();
        }

        public async Task RemoverLogicamenteAsync(long id)
        {
            var sql = "UPDATE planejamento_anual_periodo_escolar SET EXCLUIDO = TRUE WHERE ID = @id";
            await database.Conexao.ExecuteAsync(sql, new { id });
        }

        public async Task RemoverLogicamentePorTurmaBimestreAsync(long idTurma, int bimestre)
        {
            var sql = @"update planejamento_anual_periodo_escolar pape
                        set excluido = true
                          , alterado_por = @alteradoPor
                          , alterado_rf = @alteradoRF
                          , alterado_em = @alteradoEm 
                        where pape.id in (select pape2.id
                                            from planejamento_anual pa
                                                inner join planejamento_anual_periodo_escolar pape2
                                                    on pa.id = pape2.planejamento_anual_id
                                                inner join periodo_escolar pe
                                                    on pape2.periodo_escolar_id = pe.id
                                          where pa.turma_id = @idTurma and
                                                pe.bimestre = @bimestre and
                                                not pa.excluido and
                                                not pape2.excluido);";
            await database.Conexao.ExecuteAsync(sql, new
            {
                idTurma,
                bimestre,
                alteradoPor = database.UsuarioLogadoNomeCompleto,
                alteradoRF = database.UsuarioLogadoRF,
                alteradoEm = DateTimeExtension.HorarioBrasilia()
            });
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> ObterPlanejamentosAnuaisPeriodosTurmaPorPlanejamentoAnualId(long planejamentoAnualId)
        {
            var sql = @"select distinct pape.id,
                        	            pe.bimestre
                        	from planejamento_anual_periodo_escolar pape
                        		inner join planejamento_anual pa
                        			on pape.planejamento_anual_id = pa.id
                        		inner join periodo_escolar pe
                        			on pape.periodo_escolar_id = pe.id
                        		inner join (select pa2.turma_id,
                        						   pa2.componente_curricular_id
                        						from planejamento_anual_periodo_escolar pape2
                        							inner join planejamento_anual pa2
                        								on pape2.planejamento_anual_id = pa2.id
                        					where pape2.planejamento_anual_id = @planejamentoAnualId and
                        						  not pape2.excluido and
                        						  not pa2.excluido) tc
                        			on pa.turma_id = tc.turma_id and
                        			   pa.componente_curricular_id = tc.componente_curricular_id
                        where not pape.excluido and
                        	  not pape.excluido and
                        	  not pa.excluido
                        order by pe.bimestre;";
            return await database.Conexao.QueryAsync<PlanejamentoAnualPeriodoEscolarResumoDto>(sql, new { planejamentoAnualId });
        }
    }
}
