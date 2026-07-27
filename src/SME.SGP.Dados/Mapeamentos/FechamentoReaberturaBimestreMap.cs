using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoReaberturaBimestreMap : BaseEntityMap<FechamentoReaberturaBimestre>
    {
        public FechamentoReaberturaBimestreMap()
        {
            ToTable("fechamento_reabertura_bimestre");
            Map(nameof(FechamentoReaberturaBimestre.Bimestre), "bimestre");
            Map(nameof(FechamentoReaberturaBimestre.FechamentoAberturaId), "fechamento_reabertura_id");
        }
    }
}