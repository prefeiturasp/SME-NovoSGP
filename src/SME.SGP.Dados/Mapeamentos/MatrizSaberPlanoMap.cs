using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MatrizSaberPlanoMap : BaseEntityMap<MatrizSaberPlano>
    {
        public MatrizSaberPlanoMap()
        {
            ToTable("matriz_saber_plano");
            Map(nameof(MatrizSaberPlano.MatrizSaberId), "matriz_id");
            Map(nameof(MatrizSaberPlano.PlanoId), "plano_id");
        }
    }
}