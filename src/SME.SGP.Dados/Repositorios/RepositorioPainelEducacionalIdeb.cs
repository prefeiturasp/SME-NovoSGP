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
                              i.ano_letivo As AnoLetivo,
                              i.serie_ano AS SerieAno,
                              i.nota,
                              i.criado_em,
                              d.dre_id as CodigoDre,
                              u.ue_id as CodigoUe
                          FROM ideb i
                          INNER JOIN ue u ON u.ue_id = i.codigo_eol_escola
                          INNER JOIN dre d ON d.id = u.dre_id
                          WHERE i.nota IS NOT NULL
                             AND i.nota BETWEEN 0 AND 10
                             AND i.serie_ano IS NOT NULL;";


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
