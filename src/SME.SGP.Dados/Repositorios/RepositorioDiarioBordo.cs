﻿using Dapper;
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
    public class RepositorioDiarioBordo : RepositorioBase<DiarioBordo>, IRepositorioDiarioBordo
    {
        public RepositorioDiarioBordo(ISgpContext conexao) : base(conexao) { }

        public async Task<DiarioBordo> ObterPorAulaId(long aulaId, long componenteCurricularId)
        {
            var sql = @"select id, aula_id, devolutiva_id, planejamento, turma_id,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf, inserido_cj
                    from diario_bordo where aula_id = @aulaId and componente_curricular_id  = @componenteCurricularId; ";

            var parametros = new { aulaId, componenteCurricularId };

            return await database.QueryFirstOrDefaultAsync<DiarioBordo>(sql, parametros);
        }

        public async Task<DiarioBordo> ObterPorAulaId(long aulaId)
        {
            var sql = @"select * from diario_bordo where aula_id = @aulaId";

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
                           and db.componente_curricular_id = @componenteCurricularCodigo
                           and a.data_aula between @periodoInicio and @periodoFim ";

            var query = $"select count(0) {condicao}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaCodigo, componenteCurricularCodigo = componenteCurricularCodigo, periodoInicio, periodoFim });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = $@"select db.planejamento as DescricaoPlanejamento
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data 
                            , db.inserido_cj as InseridoCJ
                            {condicao} 
                            order by a.data_aula {offSet} ";

            return new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>()
            {
                Items = await database.Conexao.QueryAsync<DiarioBordoDevolutivaDto>(query,
                                                    new
                                                    {
                                                        turmaCodigo,
                                                        componenteCurricularCodigo = componenteCurricularCodigo,
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
                           and db.componente_curricular_id = @componenteCurricularCodigo
                           and a.data_aula between @periodoInicio and @periodoFim ";

            return await database.Conexao.QueryAsync<long, DateTime, Tuple<long, DateTime>>(
                query,
                (id, data) =>
                {
                    return new Tuple<long, DateTime>(id, data);
                },
                new
                {
                    turmaCodigo,
                    componenteCurricularCodigo = componenteCurricularCodigo,
                    periodoInicio,
                    periodoFim
                }, null, true, splitOn: "*", null, null, "Query Postgres");
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

            query = $@"select db.planejamento as DescricaoPlanejamento
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data
                            , db.inserido_cj 
                            ,d.descricao
                        from diario_bordo db
                       inner join aula a on a.id = db.aula_id
                       left join devolutiva d on db.devolutiva_id  =d.id
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
            return (await database.QueryAsync<DiarioBordo, Aula, Turma, Ue, Dre, DiarioBordo>(query, (diarioBordo, aula, turma, ue, dre) =>
            {
                ue.AdicionarDre(dre);
                turma.AdicionarUe(ue);
                aula.AtualizaTurma(turma);
                diarioBordo.AdicionarAula(aula);
                return diarioBordo;
            }, new { diarioBordoId }, splitOn: "DevolutivaId, AulaPaiId, turma_id, ue_id, dre_id")).FirstOrDefault();
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoResumoDto>> ObterListagemDiarioBordoPorPeriodoPaginado(long turmaId, string componenteCurricularPaiCodigo, long componenteCurricularFilhoCodigo, DateTime? periodoInicio, DateTime? periodoFim, Paginacao paginacao)
        {
            StringBuilder condicao = new StringBuilder();

            condicao.AppendLine(@"from aula a
                         inner join turma t on a.turma_id = t.turma_id
                         left join diario_bordo db on a.id = db.aula_id and db.componente_curricular_id = @componenteCurricularFilhoCodigo
                         where t.id = @turmaId
                           and a.disciplina_id = @componenteCurricularPaiCodigo 
                           and not a.excluido 
                           and (a.data_aula < NOW() or db.id is not null)");

            if (periodoInicio.HasValue)
                condicao.AppendLine(" and a.data_aula::date >= @periodoInicio ");

            if (periodoFim.HasValue)
                condicao.AppendLine(" and a.data_aula::date <= @periodoFim ");


            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var query = $"select count(0) from (select db.id, a.data_aula DataAula, db.criado_rf CodigoRf, db.criado_por Nome, a.tipo_aula Tipo, a.id AulaId, db.inserido_cj InseridoCJ, false Pendente {condicao}) as DiarioBordo";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
               new { turmaId, componenteCurricularPaiCodigo, componenteCurricularFilhoCodigo, periodoInicio, periodoFim });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = $"select db.id, a.data_aula DataAula, db.criado_rf CodigoRf, db.criado_por Nome, a.tipo_aula Tipo, a.id AulaId, " +
                    $"db.inserido_cj InseridoCJ, " +
                    $"case when db.id is null then true else false end Pendente {condicao} order by dataaula desc {offSet} ";

            return new PaginacaoResultadoDto<DiarioBordoResumoDto>()
            {
                Items = await database.Conexao.QueryAsync<DiarioBordoResumoDto>(query,
                                                    new
                                                    {
                                                        turmaId,
                                                        componenteCurricularPaiCodigo,
                                                        componenteCurricularFilhoCodigo,
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
            var sql = @"";
            if (dreId == 0 && ueId == 0)
            {
                sql = @"select t.ano
                               , count(db.id) as QuantidadeTotalDiariosdeBordo
                               , count(db.id) filter (where db.devolutiva_id is not null) QuantidadeTotalDiariosdeBordoComDevolutiva
                        from diario_bordo db
                             inner join turma t on t.id = db.turma_id
                       where not db.excluido
                             and t.ano_letivo = @anoLetivo
                             and t.modalidade_codigo = @modalidade
                       group by t.ano";
            }

            if (dreId > 0 && ueId == 0)
            {
                sql = @"select t.ano
                               , count(db.id) as QuantidadeTotalDiariosdeBordo
                               , count(db.id) filter (where db.devolutiva_id is not null) QuantidadeTotalDiariosdeBordoComDevolutiva
                        from diario_bordo db
                             inner join turma t on t.id = db.turma_id
                             inner join ue on ue.id = t.ue_id 
	                         inner join dre on dre.id = ue.dre_id 
                       where not db.excluido
                             and t.ano_letivo = @anoLetivo
                             and t.modalidade_codigo = @modalidade
                             and dre.id = @dreId       
                       group by t.ano";
            }

            if (dreId > 0 && ueId > 0)
            {
                sql = @"select t.ano
                               , count(db.id) as QuantidadeTotalDiariosdeBordo
                               , count(db.id) filter (where db.devolutiva_id is not null) QuantidadeTotalDiariosdeBordoComDevolutiva
                        from diario_bordo db
                             inner join turma t on t.id = db.turma_id
                             inner join ue on ue.id = t.ue_id 
	                         inner join dre on dre.id = ue.dre_id 
                       where not db.excluido
                             and t.ano_letivo = @anoLetivo
                             and t.modalidade_codigo = @modalidade
                             and dre.id = @dreId
                             and t.ue_id = @ueId
                       group by t.ano";
            }

            return await database.Conexao.QueryAsync<QuantidadeTotalDiariosEDevolutivasPorAnoETurmaDTO>(sql, new { anoLetivo, dreId, ueId, modalidade });
        }

        public async Task<IEnumerable<QuantidadeTotalDiariosPendentesPorAnoETurmaDTO>> ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade)
        {
            try
            {
                var sql = @"";
                if (dreId == 0 && ueId == 0)
                {
                    sql = @"select
                            distinct 
                            t.ano,
                            sum(c.quantidade_pendentes) as quantidadeTotalDiariosPendentes
                        from consolidacao_diarios_bordo c
                            inner join turma t on t.id = c.turma_id
                            inner join ue on ue.id = t.ue_id 
                            inner join dre on dre.id = ue.dre_id
                            and t.ano <> '0'
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade_codigo = @modalidade
                        group by t.ano  ";
                }

                if (dreId > 0 && ueId == 0)
                {
                    sql = @"select
                            distinct 
                            t.ano,
                            sum(c.quantidade_pendentes) as quantidadeTotalDiariosPendentes
                        from consolidacao_diarios_bordo c
                            inner join turma t on t.id = c.turma_id
                            inner join ue on ue.id = t.ue_id 
                            inner join dre on dre.id = ue.dre_id
                            and t.ano <> '0'
                            and t.ano_letivo = @anoLetivo
                            and dre.id = @dreId
                            and t.modalidade_codigo = @modalidade
                        group by t.ano ";
                }

                if (dreId > 0 && ueId > 0)
                {
                    sql = @"select
                            distinct 
                            t.ano,
                            sum(c.quantidade_pendentes) as quantidadeTotalDiariosPendentes
                        from consolidacao_diarios_bordo c
                            inner join turma t on t.id = c.turma_id
                            inner join ue on ue.id = t.ue_id 
                            inner join dre on dre.id = ue.dre_id
                            and t.ano <> '0'
                            and t.ano_letivo = @anoLetivo
                            and dre.id = @dreId
	                        and t.ue_id = @ueId
                            and t.modalidade_codigo = @modalidade
                        group by t.ano ";
                }

                return await database.Conexao.QueryAsync<QuantidadeTotalDiariosPendentesPorAnoETurmaDTO>(sql, new { anoLetivo, dreId, ueId, modalidade });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<QuantidadeTotalDiariosPendentesEPreenchidosPorAnoOuTurmaDTO>> ObterQuantidadeTotalDeDiariosPreenchidosEPendentesPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade, bool ehPerfilSMEDRE)
        {
            var sql = @"";

            sql = @"select
                            distinct";

            if (ehPerfilSMEDRE && dreId > 0 && ueId == 0)
                sql += @" t.ano as AnoTurma,";
            else if(dreId > 0 && ueId > 0)
                sql += @" t.nome as AnoTurma,";

            sql += @"   sum(c.quantidade_pendentes) as quantidadeTotalDiariosPendentes,
                            sum(c.quantidade_preenchidos) as quantidadeTotalDiariosPreenchidos
                            from consolidacao_diarios_bordo c
                            inner join turma t on t.id = c.turma_id
                            inner join ue on ue.id = t.ue_id 
                            inner join dre on dre.id = ue.dre_id
                            and t.ano <> '0' 
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade_codigo = @modalidade
                            ";

            if (dreId > 0)
                sql += @" and dre.id = @dreId";
            if (ueId > 0)
                sql += @" and t.ue_id = @ueId";

            if (ehPerfilSMEDRE && dreId > 0 && ueId == 0)
                sql += @" group by t.ano ";
            else if (dreId > 0 && ueId > 0) 
                sql += @" group by t.nome ";

            return await database.Conexao.QueryAsync<QuantidadeTotalDiariosPendentesEPreenchidosPorAnoOuTurmaDTO>(sql, new { anoLetivo, dreId, ueId, modalidade });
        }

        public async Task<IEnumerable<QuantidadeDiariosDeBordoComDevolutivaEDevolutivaPendentePorTurmaAnoDto>> ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteAsync(int anoLetivo, Modalidade modalidade, DateTime dataAula, long? dreId, long? ueId)
        {
            var possuiFiltroUe = ueId.HasValue;
            var query = new StringBuilder(DefinirSelectQueryDiariosDeBordoComDevolutivaEDevolutivaPendente(possuiFiltroUe));

            query.AppendLine(@"
                from 
	                diario_bordo db 
                left join
	                devolutiva d 
	                on db.devolutiva_id  = d.id 
                inner join 
	                aula a 
	                on db.aula_id = a.id 
                inner join 
	                turma t 
	                on a.turma_id  = t.turma_id
                inner join 
	                ue u 
	                on t.ue_id  = u.id
                where not db.excluido 
                    and t.ano_letivo = @anoLetivo
	                and t.modalidade_codigo = @modalidade
	                and a.data_aula < @dataAula ");

            if (dreId.HasValue)
                query.AppendLine("and u.dre_id = @dreId ");

            if (ueId.HasValue)
                query.AppendLine("and u.id = @ueId ");

            query.AppendLine(DefinirAgrupamentoQueryDiariosDeBordoComDevolutivaEDevolutivaPendente(possuiFiltroUe));

            var parametros = new
            {
                anoLetivo,
                modalidade,
                dataAula,
                dreId,
                ueId
            };

            return await database.QueryAsync<QuantidadeDiariosDeBordoComDevolutivaEDevolutivaPendentePorTurmaAnoDto>(query.ToString(), parametros);
        }

        private string DefinirSelectQueryDiariosDeBordoComDevolutivaEDevolutivaPendente(bool possuiFiltroDeUe)
            => possuiFiltroDeUe
                ? @"select
                        t.turma_id,
                        t.nome as TurmaAno,
                        count(*) filter (where db.devolutiva_id is null) as DiariosComDevolutivasPendentes,
	                    count(*) filter (where db.devolutiva_id is not null) as DiariosComDevolutivas"
                : @"select
                        t.ano as TurmaAno,
                        count(*) filter (where db.devolutiva_id is null) as DiariosComDevolutivasPendentes,
	                    count(*) filter (where db.devolutiva_id is not null) as DiariosComDevolutivas";

        private string DefinirAgrupamentoQueryDiariosDeBordoComDevolutivaEDevolutivaPendente(bool possuiFiltroDeUe)
            => possuiFiltroDeUe
                ? @"group by
                        t.turma_id,
	                    t.nome"
                : @"group by
                        t.ano";

        public async Task<IEnumerable<DiarioBordo>> ObterIdDiarioBordoAulasExcluidas(string codigoTurma, string[] codigosDisciplinas, long tipoCalendarioId, DateTime[] datasConsideradas)
        {
            var sqlQuery = new StringBuilder();
            sqlQuery.AppendLine("select distinct db.id,");
            sqlQuery.AppendLine("	             db.aula_id,");
            sqlQuery.AppendLine("	             a.id,");
            sqlQuery.AppendLine("	             a.data_aula");
            sqlQuery.AppendLine("	from diario_bordo db");
            sqlQuery.AppendLine("		inner join aula a");
            sqlQuery.AppendLine("			on db.aula_id = a.id");
            sqlQuery.AppendLine("where a.excluido and");
            sqlQuery.AppendLine("	a.turma_id = @codigoTurma and");
            sqlQuery.AppendLine("	a.disciplina_id = any(@codigosDisciplinas) and");
            sqlQuery.AppendLine("	a.tipo_calendario_id = @tipoCalendarioId and");
            sqlQuery.AppendLine("	a.data_aula = any(@datasConsideradas);");

            return await database.Conexao
                .QueryAsync<DiarioBordo, Aula, DiarioBordo>(sqlQuery.ToString(), (diarioBordo, aula) =>
                {
                    diarioBordo.AdicionarAula(aula);
                    return diarioBordo;
                }, new
                {
                    codigoTurma,
                    codigosDisciplinas,
                    tipoCalendarioId,
                    datasConsideradas
                }, splitOn: "id");
        }

    }
}
