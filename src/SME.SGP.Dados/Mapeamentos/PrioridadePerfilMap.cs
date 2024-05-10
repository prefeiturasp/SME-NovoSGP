using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PrioridadePerfilMap : BaseMap<PrioridadePerfil>
    {
        public PrioridadePerfilMap()
        {
            ToTable("prioridade_perfil");
            Map(c => c.CodigoPerfil).ToColumn("codigo_perfil");
            Map(c => c.NomePerfil).ToColumn("nome_perfil");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Tipo).ToColumn("tipo");
        }
    }
}