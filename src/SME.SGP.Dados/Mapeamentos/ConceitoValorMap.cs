using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConceitoValorMap : BaseMap<Conceito>
    {
        public ConceitoValorMap()
        {
            ToTable("conceito_valores");
            Map(c => c.Aprovado).ToColumn("aprovado");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.Valor).ToColumn("valor");
        }
    }
}