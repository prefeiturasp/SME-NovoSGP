using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MatrizSaberMap : BaseMap<MatrizSaber>
    {
        public MatrizSaberMap()
        {
            ToTable("matriz_saber");
        }
    }
}