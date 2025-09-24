using Polly;
using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalIdeb : RepositorioBase<PainelEducacionalConsolidacaoIdeb>, IRepositorioPainelEducacionalIdeb
    {
        private readonly IAsyncPolicy _policy;
        public RepositorioPainelEducacionalIdeb(ISgpContext database, IReadOnlyPolicyRegistry<string> registry,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            _policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task ExcluirIdeb()
        {
            const string comando = @"delete from public.painel_educacional_consolidacao_ideb";
            await _policy.ExecuteAsync(() => database.Conexao.ExecuteAsync(comando));
        }

        public async Task<IEnumerable<PainelEducacionalIdebDto>> ObterIdeb()
        {
            var query = @"SELECT 
                          t1.ano_letivo As AnoLetivo,
                          t1.serie_ano AS etapa,
                          t1.nota,
                          t1.criado_em,
                          t3.dre_id as CodigoDre,
                          t2.ue_id as CodigoUe
                      FROM ideb t1
                      INNER JOIN ue t2 ON t2.ue_id = t1.codigo_eol_escola
                      INNER JOIN dre t3 ON t3.id = t2.dre_id
                      WHERE t1.nota IS NOT NULL
                        AND t1.nota BETWEEN 0 AND 10
                        AND t1.serie_ano IS NOT NULL;";
                                            

            return await _policy.ExecuteAsync(() => database.Conexao.QueryAsync<PainelEducacionalIdebDto>(query));
        }

        public async Task<bool> SalvarIdeb(IEnumerable<PainelEducacionalConsolidacaoIdeb> ideb)
        {
            const string comando = @"
        INSERT INTO painel_educacional_consolidacao_ideb
            (ano_letivo, etapa, faixa, quantidade, media_geral, codigo_dre, codigo_ue, criado_em, criado_por, criado_rf)
        VALUES
            (@AnoLetivo, @Etapa, @Faixa, @Quantidade, @MediaGeral, @CodigoDre, @CodigoUe, NOW(), 'Sistema', '0')
        ON CONFLICT (ano_letivo, etapa, faixa)
        DO UPDATE SET
            quantidade   = EXCLUDED.quantidade,
            media_geral  = EXCLUDED.media_geral,
            codigo_dre   = EXCLUDED.codigo_dre,
            alterado_em  = NOW(),
            alterado_por = 'Sistema',
            alterado_rf  = '0';";

            var parametros = ideb.Select(i => new
            {
                i.AnoLetivo,
                Etapa = (int)i.Etapa,
                i.Faixa,
                i.Quantidade,
                i.MediaGeral,
                i.CodigoDre,
                i.CodigoUe
            }).ToArray();

            var linhasAfetadas = await _policy.ExecuteAsync(() =>
                database.Conexao.ExecuteAsync(comando, parametros));

            return linhasAfetadas > 0;
        }

    }
}
