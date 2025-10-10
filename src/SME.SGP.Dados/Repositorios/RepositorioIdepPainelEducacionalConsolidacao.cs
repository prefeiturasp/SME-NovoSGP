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
        public RepositorioIdepPainelEducacionalConsolidacao(ISgpContext database,
            IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria) { }

        public async Task<bool> Inserir(IEnumerable<PainelEducacionalConsolidacaoIdep> consolidacoes)
        {
            var sql = @"
             INSERT INTO painel_educacional_consolidacao_idep
                 (ano_letivo, etapa, faixa, quantidade, media_geral, criado_por, criado_rf, alterado_em, alterado_por, alterado_rf, codigo_dre)
             VALUES
                 (@AnoLetivo, @Etapa, @Faixa, @Quantidade, @MediaGeral, @CriadoPor, @CriadoRF, @UltimaAtualizacao, @AlteradoPor, @AlteradoRF, @CodigoDre)
             ON CONFLICT (ano_letivo, etapa, faixa, codigo_dre)
             DO UPDATE SET
                 quantidade   = EXCLUDED.quantidade,
                 media_geral  = EXCLUDED.media_geral,
                 alterado_em  = EXCLUDED.alterado_em,
                 alterado_por = EXCLUDED.alterado_por,
                 alterado_rf  = EXCLUDED.alterado_rf,
                 codigo_dre   = EXCLUDED.codigo_dre
             WHERE painel_educacional_consolidacao_idep.alterado_em < EXCLUDED.alterado_em;";

            using var conn = database.Conexao;

            int rowsAffected = await conn.ExecuteAsync(sql, consolidacoes);

            return rowsAffected > 0;
        }
    }
}