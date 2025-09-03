using Polly;
using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdepPainelEducacionalConsolidacao : RepositorioBase<Dominio.Entidades.PainelEducacionalConsolidacaoIdep>, IRepositorioIdepPainelEducacionalConsolidacao
    {
        private readonly IAsyncPolicy policy;

        public RepositorioIdepPainelEducacionalConsolidacao(ISgpContext database,
            IReadOnlyPolicyRegistry<string> registry,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<bool> Inserir(IEnumerable<PainelEducacionalIdep> consolidacoes)
        {
            var sql = @"
        INSERT INTO painel_educacional_consolidacao_idep
            (ano_letivo, etapa, faixa, quantidade, media_geral, criado_por, criado_rf, atualizado_em, atualizado_por, atualizado_rf)
        VALUES
            (@AnoLetivo, @Etapa, @Faixa, @Quantidade, @MediaGeral, 'worker', 'system', @UltimaAtualizacao, 'worker', 'system')
        ON CONFLICT (ano_letivo, etapa, faixa)
        DO UPDATE SET
            quantidade     = EXCLUDED.quantidade,
            media_geral    = EXCLUDED.media_geral,
            atualizado_em  = EXCLUDED.atualizado_em,
            atualizado_por = EXCLUDED.atualizado_por,
            atualizado_rf  = EXCLUDED.atualizado_rf
        WHERE painel_educacional_consolidacao_idep.atualizado_em < EXCLUDED.atualizado_em;";

            using var conn = database.Conexao;

            var rowsAffected = await conn.ExecuteAsync(sql, consolidacoes);

            return rowsAffected > 0;
        }
    }
}