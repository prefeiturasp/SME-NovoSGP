using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemPlanoMap : BaseMap<ObjetivoAprendizagemPlano>
    {
        public ObjetivoAprendizagemPlanoMap()
        {
            ToTable("objetivo_aprendizagem_plano");
            Map(c => c.ObjetivoAprendizagemJuremaId).ToColumn("objetivo_aprendizagem_jurema_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}