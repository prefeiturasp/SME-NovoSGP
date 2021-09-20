using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AvisoMap : BaseMap<Aviso>
    {
        public AvisoMap()
        {
            ToTable("aviso");
            Map(a => a.AulaId).ToColumn("aula_id");
            Map(a => a.AvisoClassroomId).ToColumn("aviso_classroom_id");
        }
    }
}
