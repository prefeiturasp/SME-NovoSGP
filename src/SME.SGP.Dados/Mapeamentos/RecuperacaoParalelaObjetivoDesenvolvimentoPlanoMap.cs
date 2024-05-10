using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaObjetivoDesenvolvimentoPlanoMap : BaseMap<RecuperacaoParalelaObjetivoDesenvolvimentoPlano>
    {
        public RecuperacaoParalelaObjetivoDesenvolvimentoPlanoMap()
        {
            ToTable("recuperacao_paralela_objetivo_desenvolvimento_plano");
            Map(c => c.ObjetivoDesenvolvimentoId).ToColumn("objetivo_desenvolvimento_id");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}