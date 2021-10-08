using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaPrevistaBimestreMap : BaseMap<AulaPrevistaBimestre>
    {
        public AulaPrevistaBimestreMap()
        {
            ToTable("aula_prevista_bimestre");
            Map(c => c.AulaPrevistaId).ToColumn("aula_prevista_id");
            Map(c => c.Previstas).ToColumn("previstas");
            Map(c => c.Bimestre).ToColumn("bimestre");
        }
    }
}
