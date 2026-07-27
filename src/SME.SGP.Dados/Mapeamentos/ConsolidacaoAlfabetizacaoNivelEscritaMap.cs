using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoAlfabetizacaoNivelEscritaMap : SimpleEntityMap<ConsolidacaoAlfabetizacaoNivelEscrita>
    {
        public ConsolidacaoAlfabetizacaoNivelEscritaMap()
        {
            ToTable("consolidacao_alfabetizacao_nivel_escrita");

            Map(nameof(ConsolidacaoAlfabetizacaoNivelEscrita.DreCodigo),"dre_codigo");
            Map(nameof(ConsolidacaoAlfabetizacaoNivelEscrita.UeCodigo),"ue_codigo");
            Map(nameof(ConsolidacaoAlfabetizacaoNivelEscrita.AnoLetivo),"ano_letivo");
            Map(nameof(ConsolidacaoAlfabetizacaoNivelEscrita.Periodo),"periodo");
            Map(nameof(ConsolidacaoAlfabetizacaoNivelEscrita.NivelEscrita),"nivel_escrita");
            Map(nameof(ConsolidacaoAlfabetizacaoNivelEscrita.Quantidade),"quantidade");
        }
    }
}