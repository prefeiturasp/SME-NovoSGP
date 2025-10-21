using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDiarioBordo : RepositorioBase<DiarioBordo>, IRepositorioDiarioBordo
    {
        private const int ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA = 2024;
        public RepositorioDiarioBordo(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        { }

        public async Task<DiarioBordo> ObterPorAulaId(long aulaId, long componenteCurricularId)
        {
            var sql = @"select id, aula_id, devolutiva_id, planejamento, turma_id,
                    criado_em, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf, inserido_cj
                    from diario_bordo 
                    where aula_id = @aulaId 
                      and componente_curricular_id = @componenteCurricularId
                      and not excluido";

            var parametros = new { aulaId, componenteCurricularId };

            return await database.QueryFirstOrDefaultAsync<DiarioBordo>(sql, parametros);
        }

        public async Task<IEnumerable<DiarioBordo>> ObterPorAulaId(long aulaId)
        {
            const string sql = @"select * from diario_bordo where aula_id = @aulaId and not excluido";
            var parametros = new { aulaId };
            return await database.QueryAsync<DiarioBordo>(sql, parametros);
        }

        public async Task<bool> ExisteDiarioParaAula(long aulaId)
        {
            var query = "select 1 from diario_bordo where aula_id = @aulaId and not excluido";

            return (await database.Conexao.QueryAsync<int>(query, new { aulaId })).Any();
        }

        public async Task ExcluirDiarioBordoDaAula(long aulaId)
        {
            var command = "update diario_bordo set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorPeriodoPaginado(string turmaCodigo, int anoLetivo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao)
        {
            if (anoLetivo >= ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA)
                return await ObterDiarioBordoDevolutivaUnificada(turmaCodigo, componenteCurricularCodigo, periodoInicio, periodoFim, paginacao);

            return await ObterDiarioBordoDevolutiva(turmaCodigo, componenteCurricularCodigo, periodoInicio, periodoFim, paginacao);
        }

        private async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiarioBordoDevolutiva(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao)
        {
            var condicao = @"from diario_bordo db 
                         inner join aula a on a.id = db.aula_id
                         left join devolutiva d on db.devolutiva_id = d.id and not d.excluido
                         where not db.excluido                           
                           and a.turma_id = @turmaCodigo
                           and db.componente_curricular_id = @componenteCurricularCodigo
                           and a.data_aula between @periodoInicio and @periodoFim ";

            var query = $"select count(0) {condicao}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaCodigo, componenteCurricularCodigo, periodoInicio, periodoFim });

            query = $@"select db.planejamento as DescricaoPlanejamento
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data 
                            , db.inserido_cj as InseridoCJ
                            {condicao} 
                            order by a.data_aula
                            offset {paginacao.QuantidadeRegistrosIgnorados} rows fetch next {paginacao.QuantidadeRegistros} rows only ";

            return new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>()
            {
                Items = await database.Conexao.QueryAsync<DiarioBordoDevolutivaDto>(query,
                                                    new
                                                    {
                                                        turmaCodigo,
                                                        componenteCurricularCodigo,
                                                        periodoInicio,
                                                        periodoFim
                                                    }),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }

        private async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiarioBordoDevolutivaUnificada(string turmaCodigo, long componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim, Paginacao paginacao)
        {
            var query = $@"select db.planejamento as DescricaoPlanejamento
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data 
                            , db.inserido_cj as InseridoCJ
                            , cc.descricao_infantil as Componente
                            from diario_bordo db 
                            inner join componente_curricular cc on cc.id = db.componente_curricular_id 
                            inner join aula a on a.id = db.aula_id
                            left join devolutiva d on db.devolutiva_id = d.id and not d.excluido
                            where not db.excluido                           
                            and a.turma_id = @turmaCodigo
                            and db.componente_curricular_id in(select id from componente_curricular 
                            where (componente_curricular_pai_id is null and id = @componenteCurricularCodigo)
                            or componente_curricular_pai_id = @componenteCurricularCodigo)
                            and a.data_aula between @periodoInicio and @periodoFim  
                            order by a.data_aula ";

            var diarios = await database.Conexao.QueryAsync<DiarioBordoDevolutivaDto>(query,
                                                    new
                                                    {
                                                        turmaCodigo,
                                                        componenteCurricularCodigo,
                                                        periodoInicio,
                                                        periodoFim
                                                    });

            var resultado = new List<DiarioBordoDevolutivaDto>();

            foreach( var itemAgrupado in diarios.GroupBy(valor => valor.Data))
            {
                var valor = itemAgrupado.FirstOrDefault();

                var dto = new DiarioBordoDevolutivaDto()
                {
                    AulaCj = valor.AulaCj,
                    InseridoCJ = valor.InseridoCJ,
                    Data = valor.Data
                };

                foreach (var item in itemAgrupado)
                {
                    dto.AdicionarDescricao(item.Componente, item.DescricaoPlanejamento);
                }

                resultado.Add(dto);
            }

            return new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>()
            {
                Items = resultado.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = resultado.Count,
                TotalPaginas = (int)Math.Ceiling((double)resultado.Count / paginacao.QuantidadeRegistros)
            };
        }

        public async Task<IEnumerable<(long Id, DateTime DataAula)>> ObterDatasPorIds(string turmaCodigo, long[] componenteCurricularCodigo, DateTime periodoInicio, DateTime periodoFim)
        {
            var query = @"select db.id as item1
                               , a.data_aula as item2 
                          from diario_bordo db
                         inner join aula a on a.id = db.aula_id
                         where not db.excluido                           
                           and a.turma_id = @turmaCodigo
                           and db.componente_curricular_id = ANY(@componenteCurricularCodigo) 
                           and a.data_aula between @periodoInicio and @periodoFim ";

            return await database.Conexao.QueryAsync<long, DateTime, (long Id, DateTime DataAula)>(
                query,
                (id, data) =>
                {
                    return (id, data);
                },
                new
                {
                    turmaCodigo,
                    componenteCurricularCodigo,
                    periodoInicio,
                    periodoFim
                }, null, true, splitOn: "*", null, null, "Query Postgres");
        }

        public async Task AtualizaDiariosComDevolutivaId(long devolutivaId, IEnumerable<long> diariosBordoIds)
        {
            var query = "update diario_bordo set devolutiva_id = @devolutivaId where id = ANY(@ids)";

            var ids = diariosBordoIds.ToArray();

            await database.Conexao.ExecuteAsync(query, new { devolutivaId, ids });
        }
        
        public async Task<IEnumerable<long>> ObterIdsPorDevolutiva(long devolutivaId)
        {
            var query = "select id from diario_bordo where devolutiva_id = @devolutivaId and not excluido";

            return await database.Conexao.QueryAsync<long>(query, new { devolutivaId });
        }

        public async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorDevolutivaPaginado(long devolutivaId, int anoLetivo, Paginacao paginacao)
        {
            if (anoLetivo >= ANO_LETIVO_INICIO_DEVOLUTIVA_UNIFICADA)
                return await ObterDiariosBordoPorDevolutivaUnificadoPaginado(devolutivaId, paginacao);

            return await ObterDiariosBordoPorDevolutivaPaginado(devolutivaId, paginacao);
        }

        private async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorDevolutivaPaginado(long devolutivaId, Paginacao paginacao)
        {
            var query = "select count(0) from diario_bordo db where db.devolutiva_id = @devolutivaId and not db.excluido ";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { devolutivaId });

            query = @"select db.planejamento as DescricaoPlanejamento
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data
                            , db.inserido_cj 
                            ,d.descricao
                        from diario_bordo db
                       inner join aula a on a.id = db.aula_id
                       left join devolutiva d on db.devolutiva_id  =d.id
                       where db.devolutiva_id = @devolutivaId
                       and not db.excluido
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

        private async Task<PaginacaoResultadoDto<DiarioBordoDevolutivaDto>> ObterDiariosBordoPorDevolutivaUnificadoPaginado(long devolutivaId, Paginacao paginacao)
        {
            var query = @"select db.planejamento as DescricaoPlanejamento
                            , a.aula_cj as AulaCj
                            , a.data_aula as Data
                            , db.inserido_cj 
                            , d.descricao
                            , cc.descricao_infantil as Componente
                        from diario_bordo db
                       inner join componente_curricular cc on cc.id = db.componente_curricular_id  
                       inner join aula a on a.id = db.aula_id
                       left join devolutiva d on db.devolutiva_id  =d.id
                       where db.devolutiva_id = @devolutivaId
                       and not db.excluido
                      order by a.data_aula";

            var diarios = await database.Conexao.QueryAsync<DiarioBordoDevolutivaDto>(query, new { devolutivaId });

            var resultado = new List<DiarioBordoDevolutivaDto>();

            foreach (var itemAgrupado in diarios.GroupBy(valor => valor.Data))
            {
                var valor = itemAgrupado.FirstOrDefault();

                var dto = new DiarioBordoDevolutivaDto()
                {
                    AulaCj = valor.AulaCj,
                    InseridoCJ = valor.InseridoCJ,
                    Data = valor.Data,
                    Descricao = valor.Descricao
                };

                foreach (var item in itemAgrupado)
                {
                    dto.AdicionarDescricao(item.Componente, item.DescricaoPlanejamento);
                }

                resultado.Add(dto);
            }

            return new PaginacaoResultadoDto<DiarioBordoDevolutivaDto>()
            {
                Items = resultado.Skip(paginacao.QuantidadeRegistrosIgnorados).Take(paginacao.QuantidadeRegistros),
                TotalRegistros = resultado.Count,
                TotalPaginas = (int)Math.Ceiling((double)resultado.Count / paginacao.QuantidadeRegistros)
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
                     where db.id =  @diarioBordoId
                     and not db.excluido";

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
                         left join diario_bordo db on a.id = db.aula_id and db.componente_curricular_id = @componenteCurricularFilhoCodigo and not db.Excluido
                         where t.id = @turmaId
                           and a.disciplina_id = @componenteCurricularPaiCodigo 
                           and not a.excluido 
                           and (a.data_aula < NOW() or db.id is not null)");

            if (periodoInicio.HasValue)
                condicao.AppendLine(" and a.data_aula::date >= @periodoInicio ");

            if (periodoFim.HasValue)
                condicao.AppendLine(" and a.data_aula::date <= @periodoFim ");

            if (paginacao.EhNulo() || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var query = $"select count(0) from (select distinct on (a.id, a.data_aula, db.componente_curricular_id) db.id, a.data_aula DataAula, db.criado_rf CodigoRf, db.criado_por Nome, a.tipo_aula Tipo, a.id AulaId, db.inserido_cj InseridoCJ, false Pendente {condicao}) as DiarioBordo";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
               new { turmaId, componenteCurricularPaiCodigo, componenteCurricularFilhoCodigo, periodoInicio, periodoFim });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = "select distinct on (a.id, a.data_aula, db.componente_curricular_id) db.id, a.data_aula DataAula, db.criado_rf CodigoRf, db.criado_por Nome, a.tipo_aula Tipo, a.id AulaId, " +
                    "db.inserido_cj InseridoCJ, " +
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
            sqlQuery.AppendLine("                 db.aula_id,");
            sqlQuery.AppendLine("                 a.id,");
            sqlQuery.AppendLine("                 a.data_aula");
            sqlQuery.AppendLine("    from diario_bordo db");
            sqlQuery.AppendLine("        inner join aula a");
            sqlQuery.AppendLine("            on db.aula_id = a.id");
            sqlQuery.AppendLine("where a.excluido and");
            sqlQuery.AppendLine("    a.turma_id = @codigoTurma and");
            sqlQuery.AppendLine("    a.disciplina_id = any(@codigosDisciplinas) and");
            sqlQuery.AppendLine("    a.tipo_calendario_id = @tipoCalendarioId and");
            sqlQuery.AppendLine("    a.data_aula = any(@datasConsideradas);");

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

        public async Task<IEnumerable<DiarioBordoSemDevolutivaDto>> DiarioBordoSemDevolutiva(long turmaId, string componenteCodigo)
        {
            var sql = $@"WITH DiarioBordo AS(
                            SELECT 
                                min(a.data_aula) AS DataAula,
                                a.tipo_calendario_id AS TipoCalendarioId
                            FROM
                                diario_bordo db
                            INNER JOIN aula a ON
                                db.aula_id = a.id
                            WHERE
                                db.devolutiva_id ISNULL
                                AND db.turma_id = @turmaId
                                AND a.disciplina_id = @componenteCodigo
                                AND NOT db.excluido
                            GROUP BY
                                a.tipo_calendario_id
                            )
                            SELECT 
                                pe.bimestre,
                                pe.periodo_inicio AS PeriodoInicio,
                                pe.periodo_fim  AS PeriodoFim,
                                db.DataAula
                            FROM
                                periodo_escolar pe
                            inner JOIN DiarioBordo db ON
                                pe.tipo_calendario_id = db.TipoCalendarioId
                                WHERE pe.periodo_inicio <= now() ";

            return await database.Conexao.QueryAsync<DiarioBordoSemDevolutivaDto>(sql, new { turmaId, componenteCodigo });
        }

        public async Task<DiarioBordoDetalhesParaPendenciaDto> ObterDadosDiarioBordoParaPendenciaPorid(long diarioBordoId)
        {
            var sql = $@"select db.id, 
                               u.nome as nomeEscola, 
                               coalesce(cc.descricao_sgp, cc.descricao) as descricaocomponenteCurricular,
                               a.professor_rf as ProfessorRf,
                               db.componente_curricular_id as componenteCurricularId, db.turma_id as turmaId, 
                               db.aula_id as aulaId, a.data_aula as dataAula,
                               t.nome nomeTurma, t.turma_id as codigoTurma,
                               t.modalidade_codigo as codModalidadeTurma, pe.id as periodoEscolarId
                              from diario_bordo db 
                        inner join turma t on t.id = db.turma_id 
                        inner join aula a on a.id = db.aula_id 
                        inner join ue u on u.ue_id  = a.ue_id 
                        inner join componente_curricular cc on cc.id = db.componente_curricular_id
                        inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim
                        where db.id = @diariobordo; ";
           
            return await database.Conexao.QueryFirstOrDefaultAsync<DiarioBordoDetalhesParaPendenciaDto>(sql, new { diariobordo = diarioBordoId });
        }

        public async Task<IEnumerable<DiarioBordo>> ObterDiariosDaMesmaAulaPorId(long id)
        {
            var sql = @"select db.*
                        from diario_bordo db 
                        where aula_id in(select aula_id from diario_bordo where id = @id) 
                          and not excluido";

            return await database.Conexao.QueryAsync<DiarioBordo>(sql, new { id });
        }
    }
}
