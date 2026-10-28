using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseParecerAnoMap : BaseMap<ConselhoClasseParecerAno>
    {
        public ConselhoClasseParecerAnoMap()
        {
            ToTable("conselho_classe_parecer_ano");
            Map(nameof(ConselhoClasseParecerAno.ParecerId), "parecer_id");
            Map(nameof(ConselhoClasseParecerAno.AnoTurma), "ano_turma");
            Map(nameof(ConselhoClasseParecerAno.Modalidade), "modalidade");
            Map(nameof(ConselhoClasseParecerAno.InicioVigencia), "inicio_vigencia");
            Map(nameof(ConselhoClasseParecerAno.FimVigencia), "fim_vigencia");
        }
    }
}