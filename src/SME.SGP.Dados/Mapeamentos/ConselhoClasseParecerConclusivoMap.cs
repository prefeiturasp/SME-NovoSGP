using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseParecerConclusivoMap : BaseEntityMap<ConselhoClasseParecerConclusivo>
    {
        public ConselhoClasseParecerConclusivoMap()
        {
            ToTable("conselho_classe_parecer");
            Map(nameof(ConselhoClasseParecerConclusivo.Nome), "nome");
            Map(nameof(ConselhoClasseParecerConclusivo.Aprovado), "aprovado");
            Map(nameof(ConselhoClasseParecerConclusivo.Frequencia), "frequencia");
            Map(nameof(ConselhoClasseParecerConclusivo.Nota), "nota");
            Map(nameof(ConselhoClasseParecerConclusivo.Conselho), "conselho");
            Map(nameof(ConselhoClasseParecerConclusivo.InicioVigencia), "inicio_vigencia");
            Map(nameof(ConselhoClasseParecerConclusivo.FimVigencia), "fim_vigencia");
        }
    }
}