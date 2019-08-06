using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class MatrizSaberPlanoMap : DommelEntityMap<MatrizSaberPlano>
    {
        public MatrizSaberPlanoMap()
        {
            ToTable("matriz_saber_plano");
        }
    }
}