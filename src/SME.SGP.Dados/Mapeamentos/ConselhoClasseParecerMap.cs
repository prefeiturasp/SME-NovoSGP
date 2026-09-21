using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseParecerMap : BaseMap<ConselhoClasseParecer>
    {
        public ConselhoClasseParecerMap()
        {
            ToTable("conselho_classe_parecer");
            Map(nameof(ConselhoClasseParecer.Nome), "nome");
            Map(nameof(ConselhoClasseParecer.Aprovado), "aprovado");
            Map(nameof(ConselhoClasseParecer.Frequencia), "frequencia");
            Map(nameof(ConselhoClasseParecer.Conselho), "conselho");
            Map(nameof(ConselhoClasseParecer.InicioVigencia), "inicio_vigencia");
            Map(nameof(ConselhoClasseParecer.FimVigencia), "fim_vigencia");
            Map(nameof(ConselhoClasseParecer.Nota), "nota");
        }
    }
}