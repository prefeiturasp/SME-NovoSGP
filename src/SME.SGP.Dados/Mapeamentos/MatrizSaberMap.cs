using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MatrizSaberMap : DommelEntityMap<MatrizSaber>
    {
        public MatrizSaberMap()
        {
            ToTable("matriz_saber");
        }
    }
}