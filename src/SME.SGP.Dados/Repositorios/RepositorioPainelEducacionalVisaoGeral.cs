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
                                          codigo_dre as codigoDre
                                        , codigo_ue as codigoUe
                                        , ano_letivo as anoLetivo
                                        , 'IDEP' as indicador
                                        , etapa as serie
                                        , media_geral as valor
                                    from painel_educacional_consolidacao_idep
                                ),
                                frequenciaGlobal as (
                                    select 
                                        codigo_dre as codigoDre
                                      , codigo_ue as codigoUe
                                      , ano_letivo as anoLetivo
                                      , 'Frequência global' as indicador
                                      , NULL as serie
                                      , percentual_frequencia as valor
                                    from painel_educacional_registro_frequencia_agrupamento_mensal
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
