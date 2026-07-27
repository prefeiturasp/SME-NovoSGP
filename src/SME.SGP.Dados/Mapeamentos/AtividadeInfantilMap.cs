using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AtividadeInfantilMap : BaseEntityMap<AtividadeInfantil>
    {
        public AtividadeInfantilMap()
        {
            ToTable("atividade_infantil");
            Map(nameof(AtividadeInfantil.AulaId), "aula_id");
            Map(nameof(AtividadeInfantil.AtividadeClassroomId), "atividade_classroom_id");
            Map(nameof(AtividadeInfantil.Titulo), "titulo");
            Map(nameof(AtividadeInfantil.Mensagem), "mensagem");
            Map(nameof(AtividadeInfantil.Email), "email");
            Map(nameof(AtividadeInfantil.Excluido), "excluido");
        }
    }
}