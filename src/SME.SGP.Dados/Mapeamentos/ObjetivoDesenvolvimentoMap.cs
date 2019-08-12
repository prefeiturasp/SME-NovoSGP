using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoDesenvolvimentoMap : BaseMap<ObjetivoDesenvolvimento>
    {
        public ObjetivoDesenvolvimentoMap()
        {
            ToTable("objetivo_desenvolvimento");
        }
    }
}