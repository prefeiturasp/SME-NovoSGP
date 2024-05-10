﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPeriodoRelatorioPAP : RepositorioBase<PeriodoRelatorioPAP>, IRepositorioPeriodoRelatorioPAP
    {
        public RepositorioPeriodoRelatorioPAP(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PeriodoRelatorioPAP> ObterComPeriodosEscolares(long id)
        {
            PeriodoRelatorioPAP retorno = null;
            var sql = @"SELECT prp.id, prp.configuracao_relatorio_pap_id, prp.periodo, 
                        crp.id, crp.inicio_vigencia, crp.fim_vigencia, crp.tipo_periodicidade, 
                        perp.id, perp.periodo_relatorio_pap_id, perp.periodo_escolar_id,
                        pe.id, pe.tipo_calendario_id, pe.bimestre, pe.periodo_inicio, pe.periodo_fim  
                        from periodo_relatorio_pap prp
                        inner join configuracao_relatorio_pap crp on prp.configuracao_relatorio_pap_id = crp.id 
                        inner join periodo_escolar_relatorio_pap perp on perp.periodo_relatorio_pap_id = prp.id 
                        inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id 
                        where prp.id = @id"
            ;

            await database.Conexao.QueryAsync<PeriodoRelatorioPAP, ConfiguracaoRelatorioPAP, PeriodoEscolarRelatorioPAP, PeriodoEscolar, PeriodoRelatorioPAP>(sql,
                (periodoPAP, configuracaoPAP, periodoEscolarPAP, periodoEscolar) =>
                {
                    if (retorno.EhNulo())
                    {
                        retorno = periodoPAP;
                        retorno.Configuracao = configuracaoPAP;
                        retorno.PeriodosEscolaresRelatorio = new List<PeriodoEscolarRelatorioPAP>();
                    }

                    periodoEscolarPAP.PeriodoEscolar = periodoEscolar;

                    retorno.PeriodosEscolaresRelatorio.Add(periodoEscolarPAP);

                    return retorno;
                }, new { id });

            return retorno;
        }

        public async Task<IEnumerable<PeriodosPAPDto>> ObterPeriodos(int anoLetivo)
        {
            var sql = @"select distinct crp.id ConfiguracaoPeriodicaRelatorioPAPId, prp.id PeriodoRelatorioPAPId, 
                        crp.tipo_periodicidade TipoConfiguracaoPeriodicaRelatorioPAP, prp.periodo PeriodoRelatorioPAP 
                        from configuracao_relatorio_pap crp
                        inner join periodo_relatorio_pap prp on prp.configuracao_relatorio_pap_id = crp.id 
                        inner join periodo_escolar_relatorio_pap perp on perp.periodo_relatorio_pap_id = prp.id
                        inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id 
                        inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id 
                        where tc.ano_letivo = @anoLetivo
                        order by crp.tipo_periodicidade desc, prp.periodo";

            return await database.Conexao.QueryAsync<PeriodosPAPDto>(sql, new { anoLetivo });
        }

        public async Task<long> ObterIdPeriodoRelatorioPAP(int anoLetivo, int semestre, string tipoPeriodo)
        {
            var sql = @"select prp.id
                        from configuracao_relatorio_pap crp
                        inner join periodo_relatorio_pap prp on prp.configuracao_relatorio_pap_id = crp.id 
                        inner join periodo_escolar_relatorio_pap perp on perp.periodo_relatorio_pap_id = prp.id
                        inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id
                        inner join tipo_calendario tc on tc.id = pe.tipo_calendario_id
                        where tc.ano_letivo = @anoLetivo 
                          and prp.periodo = @semestre
                          and crp.tipo_periodicidade = @tipoPeriodo";

             return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { anoLetivo, semestre, tipoPeriodo });
        }

        public async Task<bool> PeriodoEmAberto(long periodoRelatorioId, DateTime dataReferencia)
        {
            var sql = @"select 1 from periodo_relatorio_pap prp 
                    inner join periodo_escolar_relatorio_pap perp on prp.id = perp.periodo_relatorio_pap_id 
                    inner join periodo_escolar pe on pe.id = perp.periodo_escolar_id 
                    where prp.id = @periodoRelatorioId
                    and periodo_fim::date >= @dataReferencia::date
                    and periodo_inicio <= @dataReferencia";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(sql.ToString(), new { periodoRelatorioId, dataReferencia });
        }
    }
}
