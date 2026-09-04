

using SME.SGP.Auditoria.Worker.Mapeamentos;

namespace SME.SGP.Auditoria.Worker
{
    public class AuditoriaMap : SimpleAuditoriaMap<Entidade.Auditoria>
    {
        public AuditoriaMap()
        {
            ToTable("auditoria");
            Map(nameof(Entidade.Auditoria.Acao), "acao");
            Map(nameof(Entidade.Auditoria.Chave), "chave");
            Map(nameof(Entidade.Auditoria.Data), "data");
            Map(nameof(Entidade.Auditoria.Entidade), "entidade");
            Map(nameof(Entidade.Auditoria.RF), "rf");
            Map(nameof(Entidade.Auditoria.Usuario), "usuario");
            Map(nameof(Entidade.Auditoria.Perfil), "perfil");
            Map(nameof(Entidade.Auditoria.Administrador), "administrador");
        }
    }
}