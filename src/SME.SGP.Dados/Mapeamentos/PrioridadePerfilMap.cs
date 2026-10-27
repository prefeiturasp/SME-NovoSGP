using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PrioridadePerfilMap : BaseMap<PrioridadePerfil>
    {
        public PrioridadePerfilMap()
        {
            ToTable("prioridade_perfil");
            Map(nameof(PrioridadePerfil.CodigoPerfil), "codigo_perfil");
            Map(nameof(PrioridadePerfil.NomePerfil), "nome_perfil");
            Map(nameof(PrioridadePerfil.Ordem), "ordem");
            Map(nameof(PrioridadePerfil.Tipo), "tipo");
        }
    }
}