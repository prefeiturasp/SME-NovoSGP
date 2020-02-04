using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoFinalMap : BaseMap<FechamentoFinal>
    {
        public FechamentoFinalMap()
        {
            ToTable("fechamento_final");
            Map(a => a.AusenciasCompensadas).ToColumn("ausencias_compensadas");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
            Map(a => a.AlunoCodigo).ToColumn("aluno_codigo");
        }
    }
}