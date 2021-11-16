using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AtividadeInfantilMap : BaseMap<AtividadeInfantil>
    {
        public AtividadeInfantilMap()
        {
            ToTable("atividade_infantil");
            Map(a => a.AulaId).ToColumn("aula_id");
            Map(a => a.AtividadeClassroomId).ToColumn("atividade_classroom_id");
            Map(a => a.Titulo).ToColumn("titulo");
            Map(a => a.Mensagem).ToColumn("mensagem");
            Map(a => a.Email).ToColumn("email");
            Map(a => a.Excluido).ToColumn("excluido");
        }
    }
}
