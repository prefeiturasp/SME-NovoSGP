using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConceitoValorMap : BaseMap<Conceito>
    {
        public ConceitoValorMap()
        {
            ToTable("conceito_valores");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
        }
    }
}