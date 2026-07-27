using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemPlanoMap : BaseEntityMap<ObjetivoAprendizagemPlano>
    {
        public ObjetivoAprendizagemPlanoMap()
        {
            ToTable("objetivo_aprendizagem_plano");
            Map(nameof(ObjetivoAprendizagemPlano.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(ObjetivoAprendizagemPlano.ObjetivoAprendizagemJuremaId), "objetivo_aprendizagem_jurema_id");
            Map(nameof(ObjetivoAprendizagemPlano.PlanoId), "plano_id");
        }
    }
}