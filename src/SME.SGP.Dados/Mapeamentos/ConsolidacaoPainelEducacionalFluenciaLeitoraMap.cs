using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConsolidacaoPainelEducacionalFluenciaLeitoraMap : SimpleEntityMap<ConsolidacaoPainelEducacionalFluenciaLeitora>
    {
        public ConsolidacaoPainelEducacionalFluenciaLeitoraMap()
        {
            ToTable("consolidacao_painel_educacional_fluencia_leitora");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.Fluencia), "fluencia");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.DescricaoFluencia), "descricao_fluencia");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.DreCodigo), "dre_codigo");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.Percentual), "percentual");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.Ano), "ano");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.Periodo), "periodo");
            Map(nameof(ConsolidacaoPainelEducacionalFluenciaLeitora.QuantidadeAlunos), "quantidade_alunos");
        }
    }
}