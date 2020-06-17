using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoReaberturaBimestreMap : BaseMap<FechamentoReaberturaBimestre>
    {
        public FechamentoReaberturaBimestreMap()
        {
            ToTable("fechamento_reabertura_bimestre");
            Map(c => c.FechamentoAbertura).Ignore();
            Map(c => c.FechamentoAberturaId).ToColumn("fechamento_reabertura_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
        }
    }
}