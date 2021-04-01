using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualComponente : RepositorioBase<PlanejamentoAnualComponente>, IRepositorioPlanejamentoAnualComponente
    {
        public RepositorioPlanejamentoAnualComponente(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PlanejamentoAnualComponente>> ObterListaPorPlanejamentoAnualPeriodoEscolarId(long turmaId, long componenteCurricularId, int bimestre)
        {
            var sql = @"select pac.*                             
                            from planejamento_anual_componente pac
                                inner join planejamento_anual_periodo_escolar pape
                                    on pac.planejamento_anual_periodo_escolar_id = pape.id
                                inner join periodo_escolar pe
                                    on pape.periodo_escolar_id = pe.id
                                inner join planejamento_anual pa
                                    on pape.planejamento_anual_id = pa.id
                        where not pa.excluido and
                              pa.turma_id = @turmaId and
                              pe.bimestre = @bimestre and 
                              pac.componente_curricular_id = @componenteCurricularId and                                 
                              pac.excluido = false;";
            return  await database.Conexao.QueryAsync<PlanejamentoAnualComponente>(sql, new { turmaId, bimestre, componenteCurricularId });
        }

        public async Task<PlanejamentoAnualComponente> ObterPorPlanejamentoAnualPeriodoEscolarId(long componenteCurricularId, long id, bool consideraExcluido = false)
        {
            var sql = $@"select * 
                            from planejamento_anual_componente 
                         where planejamento_anual_periodo_escolar_id = @id and 
                               componente_curricular_id = @componenteCurricularId {(!consideraExcluido ? " and not excluido" : string.Empty )}
                         order by id desc;";

            var planejamento = await database.Conexao
                .QueryFirstOrDefaultAsync<PlanejamentoAnualComponente>(sql, new { id, componenteCurricularId });

            return planejamento;
        }

        public async Task RemoverLogicamenteAsync(long[] ids)
        {
            var sql = @"UPDATE planejamento_anual_componente 
                               SET excluido = true
                                 , alterado_por = @alteradoPor
                                 , alterado_rf = @alteradoRF
                                 , alterado_em = @alteradoEm 
                              WHERE ID = any(@ids)";
            await database.Conexao.ExecuteAsync(sql, new { 
                ids,
                alteradoPor = database.UsuarioLogadoNomeCompleto,
                alteradoRF = database.UsuarioLogadoRF,
                alteradoEm = DateTimeExtension.HorarioBrasilia()
            });
        }
    }
}
