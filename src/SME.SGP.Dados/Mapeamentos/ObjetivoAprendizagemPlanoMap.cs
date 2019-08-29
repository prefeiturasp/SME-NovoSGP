using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ObjetivoAprendizagemPlanoMap : BaseMap<ObjetivoAprendizagemPlano>
    {
        public ObjetivoAprendizagemPlanoMap()
        {
            ToTable("objetivo_aprendizagem_plano");
            Map(c => c.CodigoComponenteEOL).ToColumn("codigo_componente_eol");
            Map(c => c.CodigoComponenteJurema).ToColumn("codigo_componente_jurema");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}