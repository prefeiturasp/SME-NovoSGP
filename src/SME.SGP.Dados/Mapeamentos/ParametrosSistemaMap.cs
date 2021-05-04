using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ParametrosSistemaMap : BaseMap<ParametrosSistema>
    {
        public ParametrosSistemaMap()
        {
            ToTable("parametros_sistema");
        }
    }
}