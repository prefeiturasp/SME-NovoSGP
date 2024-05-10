using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AvisoMap : BaseMap<Aviso>
    {
        public AvisoMap()
        {
            ToTable("aviso");
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.AvisoClassroomId).ToColumn("aviso_classroom_id");
            Map(c => c.Mensagem).ToColumn("mensagem");
            Map(c => c.Email).ToColumn("email");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
