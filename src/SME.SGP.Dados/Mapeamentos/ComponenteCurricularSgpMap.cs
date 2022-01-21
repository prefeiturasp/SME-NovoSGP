using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ComponenteCurricularSgpMap : BaseMap<ComponenteCurricularSgp>
    {
        public ComponenteCurricularSgpMap()
        {
            ToTable("componente_curricular");
            Map(c => c.ComponenteCurricularPaiId).ToColumn("componente_curricular_pai_id");
            Map(c => c.AreaConhecimentoId).ToColumn("area_conhecimento_id");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.EhBaseNacional).ToColumn("eh_base_nacional");
            Map(c => c.EhCompatilhada).ToColumn("eh_compartilhada");
            Map(c => c.EhRegenciaClasse).ToColumn("eh_regencia");
            Map(c => c.EhTerritorio).ToColumn("eh_territorio");
            Map(c => c.GrupoMatrizId).ToColumn("grupo_matriz_id");
            Map(c => c.PermiteLancamentoNota).ToColumn("permite_lancamento_nota");
            Map(c => c.PermiteRegistroFrequencia).ToColumn("permite_registro_frequencia");
            Map(c => c.DescricaoSGP).ToColumn("descricao_sgp");
            Map(c => c.DescricaoInfantil).ToColumn("descricao_infantil");
        }
    }
}