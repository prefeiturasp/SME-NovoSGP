using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class UsuarioMap : BaseMap<Usuario>
    {
        public UsuarioMap()
        {
            ToTable("usuario");
            Map(a => a.CodigoRf).ToColumn("rf_codigo");
        }
    }
}