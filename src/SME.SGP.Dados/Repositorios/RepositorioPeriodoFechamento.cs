﻿using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPeriodoFechamento : RepositorioBase<PeriodoFechamento>, IRepositorioPeriodoFechamento
    {
        public RepositorioPeriodoFechamento(ISgpContext conexao) : base(conexao)
        {
        }
        public async Task<bool> ExistePeriodoPorUeDataBimestre(long ueId, DateTime dataReferencia, int bimestre)
        {
            string query = @"select  1
                           from periodo_fechamento p
                           left join periodo_fechamento_bimestre pfb ON pfb.periodo_fechamento_id = p.id
                           left join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                           where p.ue_id = @ueId
                           and @dataReferencia between pfb.inicio_fechamento and pfb.final_fechamento
                           and pe.bimestre = @bimestre";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query.ToString(), new
            {
                ueId,
                dataReferencia,
                bimestre
            });
        }

        public async Task<PeriodoFechamento> ObterPeriodoPorUeDataBimestreAsync(long ueId, DateTime dataReferencia, int bimestre, bool ehModalidadeInfantil)
        {
            string query = $@"select p.*, pfb.*
                           from periodo_fechamento p
                           left join periodo_fechamento_bimestre pfb ON pfb.periodo_fechamento_id = p.id
                           left join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                           where p.ue_id = @ueId
                           and @dataReferencia between pfb.inicio_fechamento and pfb.final_fechamento
                           and pe.bimestre {BimestreConstants.ObterCondicaoBimestre(bimestre,ehModalidadeInfantil)}";

            var lookup = new Dictionary<long, PeriodoFechamento>();

            await database.Conexao.QueryAsync<PeriodoFechamento, PeriodoFechamentoBimestre, PeriodoFechamento>(query.ToString(), (periodoFechamento, periodoFechamentoBimestre) => {
                var retorno = new PeriodoFechamento();
                if (!lookup.TryGetValue(periodoFechamento.Id, out retorno))
                {
                    retorno = periodoFechamento;
                    lookup.Add(periodoFechamento.Id, retorno);
                }

                retorno.AdicionarFechamentoBimestre(periodoFechamentoBimestre);

                return retorno;
            },  new
            {
                ueId,
                dataReferencia,
                bimestre
            });

            return lookup.Select( a => a.Value).FirstOrDefault();
        }

        public async Task<PeriodoFechamentoBimestre> ObterPeriodoFechamentoTurma(int anoLetivo, int bimestre, long? periodoEscolarId)
        {
            var validacaoBimestre = bimestre == 0 ? "order by pe.bimestre desc limit 1" : "and pe.bimestre = @bimestre";
            var validacaoPeriodo = periodoEscolarId.HasValue ? "and pe.id = @periodoEscolarId" : "";

            var query = $@"select pfb.* 
                          from periodo_fechamento pf 
                         inner join periodo_fechamento_bimestre pfb on pfb.periodo_fechamento_id = pf.id
                         inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                         inner join tipo_calendario tc on pe.tipo_calendario_id = tc.id 
                         where tc.ano_letivo  = @anoLetivo
                            {validacaoPeriodo} 
                            {validacaoBimestre}";



            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoFechamentoBimestre>(query, new { anoLetivo, bimestre, periodoEscolarId });
        }

        public PeriodoFechamento ObterPorFiltros(long? tipoCalendarioId, long? turmaId)
        {
            var query = new StringBuilder("select f.*,fb.*,p.*, t.*");
            query.AppendLine("from");
            query.AppendLine("periodo_fechamento f");
            query.AppendLine("inner join periodo_fechamento_bimestre fb on");
            query.AppendLine("f.id = fb.periodo_fechamento_id");
            query.AppendLine("inner join periodo_escolar p on");
            query.AppendLine("fb.periodo_escolar_id = p.id");
            query.AppendLine("inner join tipo_calendario t on");
            query.AppendLine("p.tipo_calendario_id = t.id");
            if (turmaId.HasValue)
                query.AppendLine(@"join turma tu on t.modalidade = (case when tu.modalidade_codigo = 5 then 1
                                                                         when tu.modalidade_codigo = 6 then 1
                                                                         when tu.modalidade_codigo = 3 then 2
                                                                         when tu.modalidade_codigo = 1 then 3
                                                                    end)");
           
            query.AppendLine("where 1=1");

            if (tipoCalendarioId.HasValue)
                query.AppendLine("and p.tipo_calendario_id = @tipoCalendarioId");

            query.AppendLine("and f.dre_id is null");
            query.AppendLine("and f.ue_id is null");

            if (turmaId.HasValue)
                query.AppendLine("and tu.id = @turmaId");

            var lookup = new Dictionary<long, PeriodoFechamento>();

            var lista = database.Conexao.Query<PeriodoFechamento, PeriodoFechamentoBimestre, PeriodoEscolar, TipoCalendario, PeriodoFechamento>(query.ToString(), (fechamento, fechamentoBimestre, periodoEscolar, tipoCalendario) =>
               {
                   PeriodoFechamento periodoFechamento;
                   if (!lookup.TryGetValue(fechamento.Id, out periodoFechamento))
                   {
                       periodoFechamento = fechamento;
                       lookup.Add(fechamento.Id, periodoFechamento);
                   }

                   periodoEscolar.AdicionarTipoCalendario(tipoCalendario);
                   fechamentoBimestre.AdicionarPeriodoEscolar(periodoEscolar);
                   periodoFechamento.AdicionarFechamentoBimestre(fechamentoBimestre);
                   return periodoFechamento;
               }, new
               {
                   tipoCalendarioId,
                   turmaId
               });
            return lookup.Values.FirstOrDefault();
        }

        public Task<PeriodoFechamento> ObterPorTurma(long turmaId)
        {
            throw new NotImplementedException();
        }

        public void SalvarBimestres(IEnumerable<PeriodoFechamentoBimestre> fechamentosBimestre, long fechamentoId)
        {
            if (fechamentosBimestre == null || !fechamentosBimestre.Any())
            {
                throw new NegocioException("A lista de bimestres é obrigatória.");
            }

            foreach (var bimestre in fechamentosBimestre)
            {
                bimestre.PeriodoFechamentoId = fechamentoId;
                if (bimestre.Id > 0)
                    database.Conexao.Update(bimestre);
                else bimestre.Id = (long)database.Conexao.Insert(bimestre);
            }
        }

        public bool ValidaRegistrosForaDoPeriodo(DateTime inicioDoFechamento, DateTime finalDoFechamento, long fechamentoId, long periodoEscolarId, long? dreId)
        {
            var query = new StringBuilder("select 1 from periodo_fechamento_bimestre fb ");
            query.AppendLine("where");
            query.AppendLine("(fb.inicio_fechamento::date < @inicioDoFechamento::date");
            query.AppendLine("or fb.final_fechamento::date > @finalDoFechamento::date)");
            query.AppendLine("and fb.periodo_escolar_id = @periodoEscolarId");
            query.AppendLine("and fb.periodo_fechamento_id <> @fechamentoId");
            if (dreId.HasValue)
            {
                query.AppendLine("and fb.periodo_fechamento_id = Any(select id from periodo_fechamento where dre_id = @dreId)");
            }

            return database.Conexao.QueryFirstOrDefault<bool>(query.ToString(), new
            {
                inicioDoFechamento,
                finalDoFechamento,
                periodoEscolarId,
                dreId,
                fechamentoId
            });
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoEscolasPorDataFinal(DateTime dataFinal)
        {
            var query = @"select pf.*, pfb.*, pe.*, tc.*
                          from periodo_fechamento pf                         
                         inner join periodo_fechamento_bimestre pfb on pfb.periodo_fechamento_id = pf.id
                         inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                         inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                         where pfb.final_fechamento = @dataFinal ";

            return await database.Conexao.QueryAsync<PeriodoFechamento, PeriodoFechamentoBimestre, PeriodoEscolar, TipoCalendario, PeriodoFechamentoBimestre>(query,
                (periodoFechamento, periodoFechamentoBimestre, periodoEscolar, tipoCalendario) =>
                {
                    periodoEscolar.TipoCalendario = tipoCalendario;
                    periodoFechamentoBimestre.PeriodoFechamento = periodoFechamento;
                    periodoFechamentoBimestre.PeriodoEscolar = periodoEscolar;

                    return periodoFechamentoBimestre;
                }, new { dataFinal });
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoBimestrePorDataFinal(int modalidade, DateTime dataEncerramento)
        {
            var query = @"select pf.*, pfb.*, pe.*
                          from periodo_fechamento pf
                         inner join periodo_fechamento_bimestre pfb on pfb.periodo_fechamento_id = pf.id
                         inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                         inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                         where pfb.final_fechamento = @dataEncerramento
                           and tc.modalidade = @modalidade";

            return await database.Conexao.QueryAsync<PeriodoFechamento, PeriodoFechamentoBimestre, PeriodoEscolar, PeriodoFechamentoBimestre>(query,
                (periodoFechamento, periodoFechamentoBimestre, periodoEscolar) =>
                {
                    periodoFechamentoBimestre.PeriodoFechamento = periodoFechamento;
                    periodoFechamentoBimestre.PeriodoEscolar = periodoEscolar;

                    return periodoFechamentoBimestre;
                }, new { modalidade, dataEncerramento });
        }

        public async Task<IEnumerable<PeriodoFechamentoBimestre>> ObterPeriodosFechamentoBimestrePorDataInicio(int modalidade, DateTime dataAbertura)
        {
            var query = @"select pf.*,pfb.*, pe.*
                          from periodo_fechamento pf                         
                         inner join periodo_fechamento_bimestre pfb on pfb.periodo_fechamento_id = pf.id
                         inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id
                         inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                         where pfb.inicio_fechamento = @dataAbertura
                           and tc.modalidade = @modalidade";

            return await database.Conexao.QueryAsync<PeriodoFechamento, PeriodoFechamentoBimestre, PeriodoEscolar, PeriodoFechamentoBimestre>(query,
                (periodoFechamento,periodoFechamentoBimestre, periodoEscolar) =>
                {
                    periodoFechamentoBimestre.PeriodoFechamento = periodoFechamento;
                    periodoFechamentoBimestre.PeriodoEscolar = periodoEscolar;

                    return periodoFechamentoBimestre;
                }, new { modalidade, dataAbertura });
        }

        public async Task<PeriodoFechamentoVigenteDto> ObterPeriodoVigentePorAnoModalidade(int anoLetivo, int modalidadeTipoCalendario)
        {
            var query = @"select tc.nome as Calendario
                             , tc.ano_letivo as AnoLetivo
                             , pe.bimestre as Bimestre
                             , pfb.inicio_fechamento as PeriodoFechamentoInicio
                             , pfb.final_fechamento as PeriodoFechamentoFim
                             , pe.periodo_inicio as PeriodoEscolarInicio
                             , pe.periodo_fim as PeriodoEscolarFim
                          from periodo_fechamento pf 
                         inner join periodo_fechamento_bimestre pfb on pfb.periodo_fechamento_id = pf.id
                         inner join periodo_escolar pe on pe.id = pfb.periodo_escolar_id 
                         inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
                         where pf.dre_id is null
                           and pf.ue_id is null
                           and tc.ano_letivo = @anoLetivo
                           and tc.modalidade = @modalidadeTipoCalendario
                           and NOW() between pfb.inicio_fechamento and pfb.final_fechamento";

            return await database.Conexao.QueryFirstOrDefaultAsync<PeriodoFechamentoVigenteDto>(query, new { anoLetivo, modalidadeTipoCalendario });
        }
    }
}