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
    public class RepositorioPeriodoEscolar : RepositorioBase<PeriodoEscolar>, IRepositorioPeriodoEscolar
    {
        public RepositorioPeriodoEscolar(ISgpContext conexao) : base(conexao) { }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendario(long tipoCalendarioId)
        {
            string query = "select * from periodo_escolar where tipo_calendario_id = @tipoCalendarioId";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { tipoCalendarioId });
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendarioAsync(long tipoCalendarioId)
        {
            string query = "select * from periodo_escolar where tipo_calendario_id = @tipoCalendarioId";

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query, new { tipoCalendarioId }, commandTimeout: 20);
        }

        public async Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime data)
        {
            StringBuilder query = new StringBuilder();
            MontaQueryComTipoCalendario(query);
            query.AppendLine("where pe.tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and pe.periodo_inicio::date <= date(@dataPeriodo)");
            query.AppendLine("and pe.periodo_fim::date >= date(@dataPeriodo)");

            return (await database.Conexao.QueryAsync<PeriodoEscolar, TipoCalendario, PeriodoEscolar>(query.ToString(), (pe, tipoCalendario) =>
            {                
                pe.AdicionarTipoCalendario(tipoCalendario);
                return pe;
            }, new { tipoCalendarioId, dataPeriodo = data.Date }, splitOn: "id")).FirstOrDefault();
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEmAbertoPorTipoCalendarioData(long tipoCalendarioId, DateTime data)
        {
            StringBuilder query = new StringBuilder();
            MontaQuery(query);
            query.AppendLine("where tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and periodo_fim::date >= date(@dataPeriodo)");

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, dataPeriodo = data.Date });
        }

        public async Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime dataInicio, DateTime dataFim)
        {
            StringBuilder query = new StringBuilder();
            MontaQuery(query);
            query.AppendLine("where tipo_calendario_id = @tipoCalendarioId");
            query.AppendLine("and periodo_inicio <= @dataInicio");
            query.AppendLine("and periodo_fim >= @dataFim");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, dataInicio, dataFim });
        }

        private static void MontaQuery(StringBuilder query)
        {
            query.AppendLine("select ");
            query.AppendLine("id,");
            query.AppendLine("bimestre,");
            query.AppendLine("periodo_inicio,");
            query.AppendLine("periodo_fim,");
            query.AppendLine("alterado_por,");
            query.AppendLine("alterado_rf,");
            query.AppendLine("alterado_em,");
            query.AppendLine("criado_por,");
            query.AppendLine("criado_rf,");
            query.AppendLine("criado_em,");
            query.AppendLine("tipo_calendario_id");
            query.AppendLine("from periodo_escolar");
        }

        private static void MontaQueryComTipoCalendario(StringBuilder query)
        {
            query.AppendLine("select ");
            query.AppendLine("pe.*,");
            query.AppendLine("tc.*");
            query.AppendLine("from periodo_escolar pe");
            query.AppendLine("inner join tipo_calendario tc on pe.tipo_calendario_id = tc.id");
        }

        public async Task<PeriodoEscolar> ObterUltimoBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0)
        {
            var query = new StringBuilder(@"select p.* 
                            from tipo_calendario t
                         inner join periodo_escolar p on p.tipo_calendario_id = t.id
                          where t.excluido = false and t.situacao
                            and t.ano_letivo = @anoLetivo
                            and t.modalidade = @modalidade ");

            DateTime dataReferencia = DateTime.MinValue;
            if (modalidade == ModalidadeTipoCalendario.EJA)
            {
                var periodoReferencia = semestre == 1 ? "periodo_inicio < @dataReferencia" : "periodo_fim > @dataReferencia";
                query.AppendLine($"and exists(select 0 from periodo_escolar p where tipo_calendario_id = t.id and {periodoReferencia})");

                // 1/6/ano ou 1/7/ano dependendo do semestre
                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 7, 1);
            }
            query.AppendLine("order by bimestre desc ");
            query.AppendLine("limit 1");

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { anoLetivo, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<int> ObterBimestreAtualAsync(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.bimestre
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and turma_id = @codigoTurma
                                              where tc.modalidade = @modalidade
                                              and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                              and not tc.excluido ");


            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { codigoTurma, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<int> ObterBimestreAtualPorTurmaIdAsync(long turmaId, ModalidadeTipoCalendario modalidade, DateTime dataReferencia)
        {
            var query = new StringBuilder(@"select pe.bimestre
                                              from periodo_escolar pe
                                              left join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                                              left join turma t on t.ano_letivo = tc.ano_letivo and t.id = @turmaId
                                             where tc.modalidade = @modalidade
                                               and pe.periodo_inicio <= @dataReferencia and pe.periodo_fim >= @dataReferencia
                                               and not tc.excluido  ");


            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query.ToString(), new { turmaId, modalidade = (int)modalidade, dataReferencia });
        }

        public async Task<bool> PeriodoEmAbertoAsync(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false)
        {
            var query = new StringBuilder(@"select count(pe.Id)
                          from periodo_escolar pe 
                         where pe.tipo_calendario_id = @tipoCalendarioId
                           and periodo_fim >= @dataReferencia ");

            if (!ehAnoLetivo)
                query.AppendLine("and periodo_inicio <= @dataReferencia");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre");

            return (await database.Conexao.QueryFirstAsync<int>(query.ToString(), new { tipoCalendarioId, dataReferencia, bimestre })) > 0;
        }

        public async Task<PeriodoEscolar> ObterPorTipoCalendarioEBimestreAsync(long tipoCalendarioId, int bimestre)
        {
            var query = $@"select p.* 
                            from periodo_escolar p
                          where p.tipo_calendario_id = @tipoCalendarioId
                            and p.bimestre = @bimestre";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoEscolar>(query.ToString(), new { tipoCalendarioId, bimestre });
        }

        public async Task<IEnumerable<PeriodoEscolar>> ObterPorAnoLetivoEModalidadeTurma(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int semestre = 1)
        {
            var query = new StringBuilder($@"select
	                            distinct pe.*
                            from
	                            periodo_escolar pe
                            inner join tipo_calendario tc on
	                            pe.tipo_calendario_id = tc.id
                            where
	                            tc.modalidade = @modalidadeTipoCalendario
	                            and tc.ano_letivo = @anoLetivo
	                            and tc.situacao
	                            and not tc.excluido");

            var dataReferencia = DateTime.MinValue;
            if (modalidadeTipoCalendario == ModalidadeTipoCalendario.EJA)
            {
                var periodoReferencia = semestre == 1 ? "pe.periodo_inicio < @dataReferencia" : "pe.periodo_fim > @dataReferencia";
                query.AppendLine($" and exists(select 0 from periodo_escolar p where tipo_calendario_id = tc.id and {periodoReferencia})");

                // 1/6/ano ou 1/7/ano dependendo do semestre
                dataReferencia = new DateTime(anoLetivo, semestre == 1 ? 6 : 7, 1);
            }

            return await database.Conexao.QueryAsync<PeriodoEscolar>(query.ToString(), new { modalidadeTipoCalendario, anoLetivo, dataReferencia });
        }
    }
}