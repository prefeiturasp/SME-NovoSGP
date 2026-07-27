using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AbrangenciaMap : SimpleEntityMap<Abrangencia>
    {
        public AbrangenciaMap()
        {
            ToTable("abrangencia");
            Map(nameof(Abrangencia.DreId), "dre_id");
            Map(nameof(Abrangencia.Perfil), "perfil");
            Map(nameof(Abrangencia.TurmaId), "turma_id");
            Map(nameof(Abrangencia.UeId), "ue_id");
            Map(nameof(Abrangencia.UsuarioId), "usuario_id");
            Map(nameof(Abrangencia.Historico), "historico");
        }
    }
}