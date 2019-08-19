using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemMap : BaseMap<ObjetivoAprendizagem>
    {
        public ObjetivoAprendizagemMap()
        {
            ToTable("objetivo_aprendizagem");
        }
    }
}