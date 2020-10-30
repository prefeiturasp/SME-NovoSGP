using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemAulaMap : BaseMap<ObjetivoAprendizagemAula>
    {
        public ObjetivoAprendizagemAulaMap()
        {
            ToTable("objetivo_aprendizagem_aula");
            Map(c => c.PlanoAulaId).ToColumn("plano_aula_id");
            Map(c => c.ObjetivoAprendizagemId).ToColumn("objetivo_aprendizagem_id");
            Map(c => c.ComponenteCurricularId).ToColumn("componente_curricular_id");
        }
    }
}
