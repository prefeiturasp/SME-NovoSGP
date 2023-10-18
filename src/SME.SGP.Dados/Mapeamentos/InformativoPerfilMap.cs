using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoPerfilMap : BaseMap<InformativoPerfil>
    {
        public InformativoPerfilMap()
        {
            ToTable("informativo_perfil");
            Map(c => c.InformativoId).ToColumn("informativo_id");
            Map(c => c.CodigoPerfil).ToColumn("codigo_perfil");
        }
    }
}
