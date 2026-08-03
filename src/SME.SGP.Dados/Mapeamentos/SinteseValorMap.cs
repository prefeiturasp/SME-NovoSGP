using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SinteseValorMap : BaseMap<Sintese>
    {
        public SinteseValorMap()
        {
            ToTable("sintese_valores");

            Map(nameof(Sintese.Aprovado), "aprovado");
            Map(nameof(Sintese.Ativo), "ativo");
            Map(nameof(Sintese.Descricao), "descricao");
            Map(nameof(Sintese.FimVigencia), "fim_vigencia");
            Map(nameof(Sintese.InicioVigencia), "inicio_vigencia");
            Map(nameof(Sintese.Valor), "valor");
        }
    }
}