using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class AtividadeInfantilMap : BaseMap<AtividadeInfantil>
    {
        public AtividadeInfantilMap()
        {
            ToTable("atividade_infantil");
            Map(a => a.AulaId).ToColumn("aula_id");
            Map(a => a.AvisoClassroomId).ToColumn("aviso_classroom_id");
        }
    }
}
