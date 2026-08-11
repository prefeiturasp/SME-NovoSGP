using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SupervisorEscolaDreMap : BaseMap<SupervisorEscolaDre>
    {
        public SupervisorEscolaDreMap()
        {
            ToTable("supervisor_escola_dre");

            Map(nameof(SupervisorEscolaDre.DreId), "dre_id");
            Map(nameof(SupervisorEscolaDre.EscolaId), "escola_id");
            Map(nameof(SupervisorEscolaDre.SupervisorId), "supervisor_id");
            Map(nameof(SupervisorEscolaDre.Excluido), "excluido");
            Map(nameof(SupervisorEscolaDre.Tipo), "tipo");
        }
    }
}