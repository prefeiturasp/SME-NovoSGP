using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class SupervisorEscolaDreMap : BaseMap<SupervisorEscolaDre>
    {
        public SupervisorEscolaDreMap()
        {
            ToTable("supervisor_escola_dre");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.EscolaId).ToColumn("escola_id");
            Map(c => c.SupervisorId).ToColumn("supervisor_id");
            Map(a => a.Excluido).ToColumn("excluido");
        }
    }
}