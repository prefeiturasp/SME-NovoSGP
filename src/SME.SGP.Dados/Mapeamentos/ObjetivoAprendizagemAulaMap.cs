using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemAulaMap : BaseMap<ObjetivoAprendizagemAula>
    {
        public ObjetivoAprendizagemAulaMap()
        {
            ToTable("objetivo_aprendizagem_aula");
            Map(nameof(ObjetivoAprendizagemAula.PlanoAulaId), "plano_aula_id");
            Map(nameof(ObjetivoAprendizagemAula.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(ObjetivoAprendizagemAula.ObjetivoAprendizagemId), "objetivo_aprendizagem_id");
            Map(nameof(ObjetivoAprendizagemAula.Excluido), "excluido");
        }
    }
}