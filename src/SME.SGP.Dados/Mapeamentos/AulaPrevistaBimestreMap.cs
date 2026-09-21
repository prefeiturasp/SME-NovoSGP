using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaPrevistaBimestreMap : BaseMap<AulaPrevistaBimestre>
    {
        public AulaPrevistaBimestreMap()
        {
            ToTable("aula_prevista_bimestre");
            Map(nameof(AulaPrevistaBimestre.AulaPrevistaId), "aula_prevista_id");
            Map(nameof(AulaPrevistaBimestre.Previstas), "aulas_previstas");
            Map(nameof(AulaPrevistaBimestre.Bimestre), "bimestre");
        }
    }
}