using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoPerfilMap : BaseEntityMap<InformativoPerfil>
    {
        public InformativoPerfilMap()
        {
            ToTable("informativo_perfil");
            Map(nameof(InformativoPerfil.InformativoId), "informativo_id");
            Map(nameof(InformativoPerfil.CodigoPerfil), "codigo_perfil");
            Map(nameof(InformativoPerfil.Excluido), "excluido");
        }
    }
}