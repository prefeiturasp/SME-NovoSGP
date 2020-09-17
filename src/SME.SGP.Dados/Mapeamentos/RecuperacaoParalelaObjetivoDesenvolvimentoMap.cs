using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RecuperacaoParalelaObjetivoDesenvolvimentoMap : BaseMap<RecuperacaoParalelaObjetivoDesenvolvimento>
    {
        public RecuperacaoParalelaObjetivoDesenvolvimentoMap()
        {
            ToTable("recuperacao_paralela_objetivo_desenvolvimento");
        }
    }
}