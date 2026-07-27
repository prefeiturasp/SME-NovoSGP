using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MatrizSaberMap : BaseEntityMap<MatrizSaber>
    {
        public MatrizSaberMap()
        {
            ToTable("matriz_saber");
            Map(nameof(MatrizSaber.Descricao), "descricao");
        }
    }
}