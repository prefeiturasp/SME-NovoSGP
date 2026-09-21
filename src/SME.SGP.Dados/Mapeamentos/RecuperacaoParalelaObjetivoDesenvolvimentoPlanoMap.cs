using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaObjetivoDesenvolvimentoPlanoMap : BaseMap<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>
    {
        public RecuperacaoParalelaObjetivoDesenvolvimentoPlanoMap()
        {
            ToTable("recuperacao_paralela_objetivo_desenvolvimento_plano");
            Map(nameof(RecuperacaoParalelaObjetivoDesenvolvimentoPlano.ObjetivoDesenvolvimentoId), "objetivo_desenvolvimento_id");
            Map(nameof(RecuperacaoParalelaObjetivoDesenvolvimentoPlano.PlanoId), "plano_id");
        }
    }
}