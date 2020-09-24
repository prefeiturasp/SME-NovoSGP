using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComponenteCurricularMap : DommelEntityMap<ComponenteCurricular>
    {
        public ComponenteCurricularMap()
        {
            ToTable("componente_curricular");
            Map(c => c.ComponenteCurricularPaiId).ToColumn("componente_curricular_pai_id");
            Map(c => c.GrupoMatrizId).ToColumn("grupo_matriz_id");
            Map(c => c.AreaConhecimentoId).ToColumn("area_conhecimento_id");

            Map(c => c.EhRegenciaClasse).ToColumn("eh_regencia");
            Map(c => c.EhCompatilhado).ToColumn("eh_compartilhada");
            Map(c => c.EhTerritorio).ToColumn("eh_territorio");
            Map(c => c.EhBaseNacional).ToColumn("eh_base_nacional");
            Map(c => c.PermiteRegistroFrequencia).ToColumn("permite_registro_frequencia");
            Map(c => c.PermiteLancamentoNota).ToColumn("permite_lancamento_nota");            
            
        }
    }
}