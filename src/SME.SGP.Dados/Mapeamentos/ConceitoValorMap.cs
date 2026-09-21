using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConceitoValorMap : BaseMap<Conceito>
    {
        public ConceitoValorMap()
        {
            ToTable("conceito_valores");
            Map(nameof(Conceito.Aprovado), "aprovado");
            Map(nameof(Conceito.Ativo), "ativo");
            Map(nameof(Conceito.Descricao), "descricao");
            Map(nameof(Conceito.FimVigencia), "fim_vigencia");
            Map(nameof(Conceito.InicioVigencia), "inicio_vigencia");
            Map(nameof(Conceito.Valor), "valor");
        }
    }
}