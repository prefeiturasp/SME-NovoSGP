using SME.SGP.Dados.Mapeamentos;
using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AvisoMap : BaseMap<Aviso>
    {
        public AvisoMap()
        {
            ToTable("aviso");
            Map(nameof(Aviso.AulaId), "aula_id");
            Map(nameof(Aviso.AvisoClassroomId), "aviso_classroom_id");
            Map(nameof(Aviso.Mensagem), "mensagem");
            Map(nameof(Aviso.Email), "email");
            Map(nameof(Aviso.Excluido), "excluido");
        }
    }
}
