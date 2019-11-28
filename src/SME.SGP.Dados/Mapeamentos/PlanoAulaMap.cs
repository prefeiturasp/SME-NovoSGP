using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PlanoAulaMap : BaseMap<PlanoAula>
    {
        public PlanoAulaMap()
        {
            ToTable("plano_aula");
            Map(c => c.AulaId).ToColumn("aula_id");
            Map(c => c.DesenvolvimentoAula).ToColumn("desenvolvimento_aula");
            Map(c => c.RecuperacaoAula).ToColumn("recuperacao_aula");
            Map(c => c.LicaoCasa).ToColumn("licao_casa");
        }
    }
}
