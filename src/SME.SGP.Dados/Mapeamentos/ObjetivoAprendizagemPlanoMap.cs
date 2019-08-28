using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemPlanoMap : BaseMap<ObjetivoAprendizagemPlano>
    {
        public ObjetivoAprendizagemPlanoMap()
        {
            ToTable("objetivo_aprendizagem_plano");
            Map(c => c.ObjetivoAprendizagemId).ToColumn("objetivo_aprendizagem_id");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}