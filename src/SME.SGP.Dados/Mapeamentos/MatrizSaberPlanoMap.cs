using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MatrizSaberPlanoMap : BaseMap<MatrizSaberPlano>
    {
        public MatrizSaberPlanoMap()
        {
            ToTable("matriz_saber_plano");
            Map(c => c.MatrizSaberId).ToColumn("matriz_id");
            Map(c => c.PlanoId).ToColumn("plano_id");
        }
    }
}