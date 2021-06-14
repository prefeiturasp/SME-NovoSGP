using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordo: RepositorioBase<DiarioBordo>, IRepositorioDiarioBordo
    {
        public RepositorioDiarioBordo(ISgpContext conexao) : base(conexao) { }

        public async Task<DiarioBordo> ObterPorAulaId(long aulaId)
        {
            var sql = @"select id, aula_id, devolutiva_id, planejamento, reflexoes_replanejamento,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf
                    from diario_bordo where aula_id = @aulaId";

            var parametros = new { aulaId = aulaId };

            return await database.QueryFirstOrDefaultAsync<DiarioBordo>(sql, parametros);
        }

        public async Task<bool> ExisteDiarioParaAula(long aulaId)
        {
            var query = "select 1 from diario_bordo where aula_id = @aulaId";

            return (await database.Conexao.QueryAsync<int>(query, new { aulaId })).Any();
        }

        public async Task ExcluirDiarioBordoDaAula(long aulaId)
        {
            // Excluir plano de aula
            var command = "update diario_bordo set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorPeriodoPaginado(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao)
        {
            var condicao = @"from diario_bordo db 
                         inner join aula a on a.id = db.aula_id
                         left join devolutiva d on db.devolutiva_id = d.id and not d.excluido
                         where not db.excluido
                           and d.id is null
                           and a.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularCodigo
                           and a.data_aula between @periodoInicio and @periodoFim ";

            var query = $"select count(0) {condicao}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaCodigo, componenteCurricularCodigo = componenteCurricularCodigo.ToString(), periodoInicio, periodoFim });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = $@"select db.planejamento
                            , regexp_replace(
		                        regexp_replace(
		                        regexp_replace(db.planejamento, E'<img[^>]+>', ' [arquivo indisponível nesta visualização]', 'gi')
		                        , E'<video[^>]+>', ' [arquivo indisponível nesta visualização]', 'gi')
		                        , E'<[^>]+>', ' ', 'gi') as PlanejamentoSimples
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data 
                            {condicao} 
                            order by a.data_aula {offSet} ";

            return new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>()
            {
                Items = await database.Conexao.QueryAsync<DiarioBordoDevolutivaDto>(query,
                                                    new
                                                    {
                                                        turmaCodigo,
                                                        componenteCurricularCodigo = componenteCurricularCodigo.ToString(),
                                                        periodoInicio,
                                                        periodoFim,
                                                        qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                                                        qtdeRegistros = paginacao.QuantidadeRegistros
                                                    }),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }
        
        public async Task<IEnumerable<Tuple<long, DateTime>>> ObterDatasPorIds(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim)
        {
            var query = @"select db.id as item1
                               , a.data_aula as item2 
                          from diario_bordo db
                         inner join aula a on a.id = db.aula_id
                         where not db.excluido
                           and db.devolutiva_id is null
                           and a.turma_id = @turmaCodigo
                           and a.disciplina_id = @componenteCurricularCodigo
                           and a.data_aula between @periodoInicio and @periodoFim ";

            var resultado = await database.Conexao.QueryAsync<Tuple<long, DateTime>>(query, new 
            { 
                turmaCodigo, 
                componenteCurricularCodigo = componenteCurricularCodigo.ToString(), 
                periodoInicio, 
                periodoFim 
            });

            return resultado;
        }

        public async Task<IEnumerable<DateTime>> ObterDatasPorIds(IEnumerable<long> diariosBordoIds)
        {
            var query = "select criado_em from diario_bordo db where id in @diariosBordoIds";

            var resultado = await database.Conexao.QueryAsync<DateTime>(query, diariosBordoIds.ToArray());

            return resultado;
        }

        public async Task AtualizaDiariosComDevolutivaId(long devolutivaId, IEnumerable<long> diariosBordoIds)
        {
            var query = "update diario_bordo set devolutiva_id = @devolutivaId where id = ANY(@ids)";

            var ids = diariosBordoIds.ToArray();

            await database.Conexao.ExecuteAsync(query, new { devolutivaId, ids });
        }
        public async Task<IEnumerable<long>> ObterIdsPorDevolutiva(long devolutivaId)
        {
            var query = "select id from diario_bordo where devolutiva_id = @devolutivaId";

            return await database.Conexao.QueryAsync<long>(query, new { devolutivaId });
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorDevolutivaPaginado(long devolutivaId, Paginacao paginacao)
        {
            var query = $"select count(0) from diario_bordo db where db.devolutiva_id = @devolutivaId ";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { devolutivaId });

            query = $@"select db.planejamento
                            , regexp_replace(
		                        regexp_replace(
		                        regexp_replace(db.planejamento, E'<img[^>]+>', ' [arquivo indisponível nesta visualização]', 'gi')
		                        , E'<video[^>]+>', ' [arquivo indisponível nesta visualização]', 'gi')
		                        , E'<[^>]+>', ' ', 'gi') as PlanejamentoSimples
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data
                        from diario_bordo db
                       inner join aula a on a.id = db.aula_id
                       where db.devolutiva_id = @devolutivaId
                      order by a.data_aula
                      offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            return new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>()
            {
                Items = await database.Conexao.QueryAsync<DiarioBordoDevolutivaDto>(query,
                                                    new
                                                    {
                                                        devolutivaId,
                                                        qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                                                        qtdeRegistros = paginacao.QuantidadeRegistros
                                                    }),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }

        public async Task ExcluirReferenciaDevolutiva(long devolutivaId)
        {
            await database.Conexao.ExecuteAsync("update diario_bordo set devolutiva_id = null where devolutiva_id = @devolutivaId", new { devolutivaId });
        }

        public async Task<DateTime?> ObterDataDiarioSemDevolutivaPorTurmaComponente(string turmaCodigo, long componenteCurricularCodigo)
        {
            var query = @"select min(a.data_aula)
                          from diario_bordo db 
                         inner join aula a on a.id = db.aula_id
                         where not db.excluido
                           and db.devolutiva_id is null
                           and a.turma_id = @turmaCodigo
                           and a.disciplina_id = @disciplinaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<DateTime?>(query, new { turmaCodigo, disciplinaId = componenteCurricularCodigo.ToString() });
        }

        public async Task<DiarioBordo> ObterDiarioBordoComAulaETurmaPorCodigo(long diarioBordoId)
        {
            var query = @"select 
                           db.id,
                           db.devolutiva_id as DevolutivaId,
                           db.planejamento,
                           a.id as AulaPaiId,
                           a.ue_id,
                           a.disciplina_id,
                           t.turma_id,
                           t.nome,
                           t.modalidade_codigo,
                           ue.ue_id,
                           ue.nome,
                           ue.tipo_escola,
                           dre.dre_id,
                           dre.abreviacao 
                      from diario_bordo db  
                     inner join aula a on
                      db.aula_id = a.id
                     inner join turma t on 
                      a.turma_id = t.turma_id
                     inner join ue on 
                      t.ue_id = ue.id 
                     inner join dre on 
                      ue.dre_id = dre.id 
                     where db.id =  @diarioBordoId";
            return (await database.QueryAsync<DiarioBordo, Aula, Turma, Ue, Dre, DiarioBordo>(query, (diarioBordo, aula ,turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                aula.AtualizaTurma(turma);
                diarioBordo.AdicionarAula(aula);
                return diarioBordo;
            }, new { diarioBordoId }, splitOn: "DevolutivaId, AulaPaiId, turma_id, ue_id, dre_id")).FirstOrDefault();
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoResumoDto>> ObterListagemDiarioBordoPorPeriodoPaginado(long turmaId, long componenteCurricularCodigo, DateTime? periodoInicio, DateTime? periodoFim, Paginacao paginacao)
        {
            StringBuilder condicao = new StringBuilder();

            condicao.AppendLine(@"from diario_bordo db 
                         inner join aula a on a.id = db.aula_id
                         inner join turma t on a.turma_id = t.turma_id
                         where not db.excluido
                           and t.id = @turmaId
                           and a.disciplina_id = @componenteCurricularCodigo ");


            if (periodoInicio.HasValue)
                condicao.AppendLine(" and a.data_aula::date >= @periodoInicio ");

            if (periodoFim.HasValue)
                condicao.AppendLine(" and a.data_aula::date <= @periodoFim ");

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var query = $"select count(0) {condicao}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaId, componenteCurricularCodigo = componenteCurricularCodigo.ToString(), periodoInicio, periodoFim });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = $"select db.id, a.data_aula DataAula, db.criado_rf CodigoRf, db.criado_por Nome {condicao} order by a.data_aula desc {offSet} ";

            return new PaginacaoResultadoDto<DiarioBordoResumoDto>()
            {
                Items = await database.Conexao.QueryAsync<DiarioBordoResumoDto>(query,
                                                    new
                                                    {
                                                        turmaId,
                                                        componenteCurricularCodigo = componenteCurricularCodigo.ToString(),
                                                        periodoInicio,
                                                        periodoFim,
                                                        qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                                                        qtdeRegistros = paginacao.QuantidadeRegistros
                                                    }),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }

        public async Task<IEnumerable<QuantidadeTotalDiariosEDevolutivasPorAnoETurmaDTO>> ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" select ");
            sql.AppendLine(" distinct ");
            sql.AppendLine(" dashboard.turma, ");
            sql.AppendLine(" SUM(dashboard.QuantidadeTotalDiariosdeBordo) as QuantidadeTotalDiariosdeBordo, ");
            sql.AppendLine(" SUM(dashboard.QuantidadeTotalDiariosdeBordoComDevolutiva) as QuantidadeTotalDiariosdeBordoComDevolutiva ");
            sql.AppendLine(" from ( ");
            sql.AppendLine(" select ");
            sql.AppendLine(" t.nome as turma, ");
            sql.AppendLine(" count(db.id) as QuantidadeTotalDiariosdeBordo, ");
            sql.AppendLine(" 0 as QuantidadeTotalDiariosdeBordoComDevolutiva ");
            sql.AppendLine(" from diario_bordo db  ");
            sql.AppendLine(" inner join aula a on a.id = db.aula_id ");
            sql.AppendLine(" inner join turma t on t.turma_id = a.turma_id  ");
            sql.AppendLine(" inner join ue on ue.id = t.ue_id ");
            sql.AppendLine(" inner join dre on dre.id = ue.dre_id ");
            sql.AppendLine(" where not db.excluido ");
            sql.AppendLine(" and t.ano_letivo = @anoLetivo ");
            sql.AppendLine(" and t.modalidade_codigo = @modalidade ");
            if(dreId > 0)
                sql.AppendLine(" and dre.id = @dreId ");
            if (ueId > 0)
                sql.AppendLine(" and t.ue_id = @ueId ");
            sql.AppendLine(" and db.planejamento is not null ");
            sql.AppendLine(" group by t.nome ");
            sql.AppendLine("  union ");
            sql.AppendLine(" select   ");
            sql.AppendLine(" t.nome as turma, ");
            sql.AppendLine(" 0 as QuantidadeTotalDiariosdeBordo, ");
            sql.AppendLine(" count(db.id) as QuantidadeTotalDiariosdeBordoComDevolutiva ");
            sql.AppendLine(" from diario_bordo db ");
            sql.AppendLine(" inner join aula a on a.id = db.aula_id  ");
            sql.AppendLine(" inner join turma t on t.turma_id = a.turma_id ");
            sql.AppendLine(" inner join ue on ue.id = t.ue_id  ");
            sql.AppendLine(" inner join dre on dre.id = ue.dre_id  ");
            sql.AppendLine(" where not db.excluido ");
            sql.AppendLine(" and t.ano_letivo = @anoLetivo ");
            sql.AppendLine(" and t.modalidade_codigo = @modalidade ");
            if (dreId > 0)
                sql.AppendLine(" and dre.id = @dreId ");
            if (ueId > 0)
                sql.AppendLine(" and t.ue_id = @ueId ");
            sql.AppendLine(" and db.planejamento is not null ");
            sql.AppendLine(" and db.devolutiva_id is not null ");
            sql.AppendLine(" group by t.nome) ");
            sql.AppendLine(" as dashboard ");
            sql.AppendLine(" group by dashboard.turma ");


            return await database.Conexao.QueryAsync<QuantidadeTotalDiariosEDevolutivasPorAnoETurmaDTO>(sql.ToString(), new { anoLetivo, dreId, ueId, modalidade });
        }

        private string wherePorDreUE(long dreId, long ueId)
        {
            var where = "";
            if (dreId > 0)
                where += $" and dre.id = @dreId ";

            if (ueId > 0)
                where += $" and t.ue_id = @ueId ";
            return where;
        }
    }
}
