using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalVisaoGeral : RepositorioBase<PainelEducacionalVisaoGeral>, IRepositorioPainelEducacionalVisaoGeral
    {
        public RepositorioPainelEducacionalVisaoGeral(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task ExcluirVisaoGeral()
        {
            const string comando = @"delete from public.painel_educacional_visao_geral";
            await database.Conexao.ExecuteAsync(comando);
        }

        public async Task<IEnumerable<PainelEducacionalVisaoGeralDto>> ObterVisaoGeralPainelEducacional()
        {
            var sql = $@"with idep as (
                                    select
                                          t3.dre_id as codigoDre
                                        , t2.ue_id as codigoUe
                                        , t1.ano_letivo as anoLetivo
                                        , 'IDEP' as indicador
                                        , t1.serie_ano::text as serie
                                        , t1.nota as valor
                                    from idep t1
                                    inner join ue t2 on t2.ue_id = t1.codigo_eol_escola 
                                    inner join dre t3 on t3.id = t2.dre_id
                                ),
                                frequenciaGlobal as (
                                    select 
                                        t3.dre_id as codigoDre
                                      , t2.ue_id as codigoUe
                                      , t1.ano_letivo as anoLetivo
                                      , 'Frequência global' as indicador
                                      , NULL as serie
                                      , t1.percentual_frequencia as valor
                                    from painel_educacional_registro_frequencia_agrupamento_mensal t1
                                    inner join ue t2 on t2.ue_id = t1.codigo_ue
                                    inner join dre t3 on t3.id = t2.dre_id
                                )
                                select * from idep
                                union all
                                select * from frequenciaGlobal;";

            return await database.QueryAsync<PainelEducacionalVisaoGeralDto>(sql);
        }

        public async Task<IEnumerable<PainelEducacionalVisaoGeralDto>> ObterVisaoGeralConsolidada(int anoLetivo, string codigoDre, string codigoUe)
        {
            var sql = $@"select 
      	                    indicador
                          , serie
                          , valor
                        from painel_educacional_visao_geral
                        where ano_letivo = @anoLetivo";

            if (!string.IsNullOrEmpty(codigoDre))
                sql += " and codigo_dre = @codigoDre";

            if (!string.IsNullOrEmpty(codigoUe))
                sql += " and codigo_ue = @codigoUe";

            return await database.QueryAsync<PainelEducacionalVisaoGeralDto>(sql, new { anoLetivo, codigoDre, codigoUe });
        }

        public async Task<bool> SalvarVisaoGeral(PainelEducacionalVisaoGeral entidade)
        {
            var retorno = await SalvarAsync(entidade);
            return retorno != 0;
        }
    }
}
