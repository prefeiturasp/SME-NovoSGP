using Dapper;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualObjetivosAprendizagem : RepositorioBase<PlanejamentoAnualObjetivoAprendizagem>, IRepositorioPlanejamentoAnualObjetivosAprendizagem
    {
        public RepositorioPlanejamentoAnualObjetivosAprendizagem(ISgpContext database) : base(database)
        {
        }

        public async Task<IEnumerable<PlanejamentoAnualObjetivoAprendizagem>> ObterPorPlanejamentoAnualComponenteId(long componenteId, bool consideraExcluido = false)
        {
            var sql = $@"select paoa.*
                            from
                                planejamento_anual_objetivos_aprendizagem paoa
                         where
                            paoa.planejamento_anual_componente_id = @componenteId{(!consideraExcluido ? " and paoa.excluido = false" : string.Empty)};";
            return await database.Conexao.QueryAsync<PlanejamentoAnualObjetivoAprendizagem>(sql, new { componenteId });
        }

        public async Task<IEnumerable<PlanejamentoAnualObjetivoAprendizagem>> ObterPorPlanejamentoAnualComponenteId(long[] componentesId)
        {
            var sql = @"select
                            id as Id,
                            planejamento_anual_componente_id as PlanejamentoAnualComponenteId,
	                        objetivo_aprendizagem_id as ObjetivoAprendizagemId
                        from
                            planejamento_anual_objetivos_aprendizagem paoa
                        where
                            paoa.planejamento_anual_componente_id = any(@componentesId) and paoa.excluido = false";
            return await database.Conexao.QueryAsync<PlanejamentoAnualObjetivoAprendizagem>(sql, new { componentesId });
        }

        public async Task RemoverTodosPorPlanejamentoAnualPeriodoEscolarId(long id)
        {
            var sql = @"delete
                            from
                                planejamento_anual_objetivos_aprendizagem
                            where
                                planejamento_anual_componente_id in (
                                select

                                    id
                                from

                                    planejamento_anual_componente
                                where

                                    planejamento_anual_periodo_escolar_id = @id)";
            await database.Conexao.ExecuteAsync(sql, new { id });
        }

        public async Task RemoverTodosPorPlanejamentoAnualPeriodoEscolarIdEComponenteCurricularId(long id, long componenteCurricularId)
        {
            var sql = @"delete
                            from
                                planejamento_anual_objetivos_aprendizagem    
                            where
                                planejamento_anual_componente_id in (
                                select 
                                    id 
                                from
                                    planejamento_anual_componente
                                where
                                    planejamento_anual_periodo_escolar_id = @id and componente_curricular_id = @componenteCurricularId)";
            await database.Conexao.ExecuteAsync(sql, new { id, componenteCurricularId });
        }

        public void SalvarVarios(IEnumerable<PlanejamentoAnualObjetivoAprendizagem> objetivos, long planejamentoAnualComponenteId)
        {
            var sql = @"copy planejamento_anual_objetivos_aprendizagem ( 
                                        planejamento_anual_componente_id,
                                        objetivo_aprendizagem_id,
                                        criado_em,
                                        criado_por,
                                        criado_rf)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var objetivo in objetivos)
                {
                    writer.StartRow();
                    writer.Write(planejamentoAnualComponenteId);
                    writer.Write(objetivo.ObjetivoAprendizagemId);
                    writer.Write(DateTime.Now);
                    writer.Write(database.UsuarioLogadoNomeCompleto);
                    writer.Write(database.UsuarioLogadoRF);
                }
                writer.Complete();
            }
        }
    }
}
