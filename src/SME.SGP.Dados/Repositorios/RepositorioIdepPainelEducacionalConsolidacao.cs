using Polly;
using Polly.Registry;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioIdepPainelEducacionalConsolidacao : RepositorioBase<PainelEducacionalConsolidacaoIdep>, IRepositorioIdepPainelEducacionalConsolidacao
    {
        private readonly IAsyncPolicy _policy;

        public RepositorioIdepPainelEducacionalConsolidacao(ISgpContext database,
            IReadOnlyPolicyRegistry<string> registry,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
            _policy = registry.Get<IAsyncPolicy>(PoliticaPolly.SGP);
        }

        public async Task<bool> Inserir(IEnumerable<PainelEducacionalConsolidacaoIdep> consolidacoes)
        {
            var sql = @"
        INSERT INTO painel_educacional_consolidacao_idep
            (ano_letivo, etapa, faixa, quantidade, media_geral, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf)
        VALUES
            (@AnoLetivo, @Etapa, @Faixa, @Quantidade, @MediaGeral, @CriadoPor, @CriadoRF, @UltimaAtualizacao, @AlteradoPor, @AlteradoRF)
        ON CONFLICT (ano_letivo, etapa, faixa)
        DO UPDATE SET
            quantidade     = EXCLUDED.quantidade,
            media_geral    = EXCLUDED.media_geral,
            alterado_em  = EXCLUDED.alterado_em,
            alterado_por = EXCLUDED.alterado_por,
            alterado_rf  = EXCLUDED.alterado_rf
        WHERE painel_educacional_consolidacao_idep.alterado_em < EXCLUDED.alterado_em;";

            using var conn = database.Conexao;

            int rowsAffected = await conn.ExecuteAsync(sql, consolidacoes);

            return rowsAffected > 0;
        }
    }
}