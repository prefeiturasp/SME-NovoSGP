using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDevolutiva : RepositorioBase<Devolutiva>, IRepositorioDevolutiva
    {
        public RepositorioDevolutiva(ISgpContext conexao) : base(conexao) { }

        public async Task<PaginacaoResultadoDto<DevolutivaResumoDto>> ListarDevolutivasPorTurmaComponentePaginado(string turmaCodigo, long componenteCurricularCodigo, DateTime? dataReferencia, Paginacao paginacao)
        {
            var query = $"select count(distinct d.id) {ObterQuery(dataReferencia)}";
            var totalRegistrosDaQuery = await database.Conexao.QueryFirstAsync<int>(query, new { turmaCodigo, componenteCurricularCodigo = componenteCurricularCodigo.ToString(), dataReferencia });

            query = $@"select distinct d.Id
	                         , d.periodo_inicio as PeriodoInicio
	                         , d.periodo_fim as PeriodoFim
	                         , d.criado_em as CriadoEm
	                         , CONCAT(d.criado_por, ' (', d.criado_rf, ')') as CriadoPor
                       {ObterQuery(dataReferencia)}
                    offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only ";

            return new PaginacaoResultadoDto<DevolutivaResumoDto>()
            {
                Items = await database.Conexao.QueryAsync<DevolutivaResumoDto>(query, new
                {
                    turmaCodigo,
                    componenteCurricularCodigo = componenteCurricularCodigo.ToString(),
                    dataReferencia,
                    qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                    qtdeRegistros = paginacao.QuantidadeRegistros
                }),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }

        private string ObterQuery(DateTime? dataReferencia)
        {
            var query = new StringBuilder(@"from devolutiva d
                         inner join diario_bordo db on db.devolutiva_id = d.id
                         inner join aula a on a.id = db.aula_id
                         where not d.excluido
                           and a.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularCodigo");

            if (dataReferencia.HasValue)
                query.Append(" and @dataReferencia between d.periodo_inicio and d.periodo_fim");

            return query.ToString();
        }

        public async Task<DateTime> ObterUltimaDataDevolutiva(string turmaCodigo, long componenteCurricularCodigo)
        {
            var query = @"select d.periodo_fim
                          from devolutiva d
                         inner join diario_bordo db on db.devolutiva_id = d.id
                         inner join aula a on a.id = db.aula_id
                         where a.turma_id = @turmaCodigo
                           and d.componente_curricular_codigo = @componenteCurricularCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<DateTime>(query, new { turmaCodigo, componenteCurricularCodigo });
        }

        public async Task<IEnumerable<long>> ObterDevolutivasPorTurmaComponenteNoPeriodo(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim)
        {
            var query = @"select d.id 
                          from devolutiva d 
                         inner join diario_bordo db on db.devolutiva_id = d.id
                         inner join aula a on a.id = db.id
                         where not d.excluido
                           and a.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularCodigo
                           and ((d.periodo_inicio <= TO_DATE(@periodoInicio, 'yyyy/mm/dd') and d.periodo_fim >= TO_DATE(@periodoInicio, 'yyyy/mm/dd'))
                             or (d.periodo_inicio <= TO_DATE(@periodoFim, 'yyyy/mm/dd') and d.periodo_fim >= TO_DATE(@periodoFim, 'yyyy/mm/dd'))
                             or (d.periodo_inicio >= TO_DATE(@periodoInicio, 'yyyy/mm/dd') and d.periodo_fim <= TO_DATE(@periodoFim, 'yyyy/mm/dd'))
                            ) ";

            return await database.Conexao.QueryAsync<long>(query, new
            {
                turmaCodigo,
                componenteCurricularCodigo = componenteCurricularCodigo.ToString(),
                periodoInicio = periodoInicio.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
                periodoFim = periodoFim.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo),
            });
        }

        public async Task<ConsolidacaoDevolutivaTurmaDTO> ObterDevolutivasPorTurmaEAnoLetivo(string turmaCodigo, int anoLetivo)
        {
            var query = @" select 
	                            ue.ue_id as ueId,
	                            ue.dre_id as dreId,
                                a.turma_id as turmaId,
                                count(db.planejamento) as quantidadeRegistradaDevolutivas
                            from devolutiva d 
                             inner join diario_bordo db on db.devolutiva_id = d.id
                             inner join aula a on a.id = db.aula_id
                             inner join turma t on t.turma_id = a.turma_id 
                             inner join ue ue on ue.id = t.ue_id 
                            where not d.excluido 
	                            and not db.excluido 
	                            and t.ano_letivo = @anoLetivo 
	                            and t.turma_id = @turmaCodigo
	                            and a.data_aula < current_date
                            group by a.turma_id, ue.ue_id, ue.dre_id ";

            return await database.Conexao.QueryFirstOrDefaultAsync<ConsolidacaoDevolutivaTurmaDTO>(query, new { turmaCodigo, anoLetivo });
        }

        public async Task<IEnumerable<DevolutivaTurmaDTO>> ObterTurmasInfantilComDevolutivasPorAno(int anoLetivo)
        {
            var query = @" select 
	                        distinct
                            t.turma_id as turmaId,
                            t.ano_letivo as anoLetivo
                        from diario_bordo db 
                            inner join aula a on a.id = db.aula_id
                            inner join turma t on t.turma_id = a.turma_id 
                            inner join ue ue on ue.id = t.ue_id 
                        where not db.excluido 
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade_codigo in (1,2)
                            and a.data_aula < current_date ";

            return await database.Conexao.QueryAsync<DevolutivaTurmaDTO>(query, new { anoLetivo });
        }

        public async Task<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO> ObterDiariosDeBordoPorTurmaEAnoLetivo(string turmaCodigo, int anoLetivo)
        {
            var query = @" select 
	                            ue.ue_id as ueid,
	                            ue.dre_id as dreId,
                                a.turma_id as turmaid,
                                count(db.id) as quantidadeDiarioBordoRegistrado
                            from diario_bordo db
	                            inner join aula a on a.id = db.aula_id
	                            inner join turma t on t.turma_id = a.turma_id 
	                            inner join ue ue on ue.id = t.ue_id	
                            where not db.excluido
                                and t.ano_letivo = @anoLetivo 
                                and t.turma_id = @turmaCodigo
                                and a.data_aula < current_date 
                            group by
	                            ue.ue_id,
	                            ue.dre_id,
                                a.turma_id";

            return await database.Conexao.QueryFirstOrDefaultAsync<QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO>(query, new { turmaCodigo, anoLetivo });
        }
    }
}
